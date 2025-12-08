# DotValidate

[![CI](https://github.com/saeedeldeeb/DotValidate/actions/workflows/ci.yml/badge.svg)](https://github.com/saeedeldeeb/DotValidate/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/v/DotValidation.svg)](https://www.nuget.org/packages/DotValidation)
[![NuGet Downloads](https://img.shields.io/nuget/dt/DotValidation.svg)](https://www.nuget.org/packages/DotValidation)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

A modern .NET validation library with automatic ASP.NET Core integration.

## Features

- **Attribute-based validation** - Declare validation rules using attributes
- **Automatic validation** - DTOs are validated automatically in ASP.NET Core
- **RFC 7807 Problem Details** - Standard error response format
- **Zero configuration** - Just add attributes and register the services

## Installation

```bash
dotnet add package DotValidation
```

## Quick Start

### 1. Add DotValidate to your services

```csharp
// Program.cs
builder.Services.AddDotValidate();
builder.Services.AddControllers().AddDotValidateFilter(); // For MVC controllers
// OR
var api = app.MapDotValidateGroup(); // For Minimal APIs
```

### 2. Decorate your DTOs with validation attributes

```csharp
using DotValidate.Attributes.Utilities;
using DotValidate.Attributes.Strings;
using DotValidate.Attributes.Numbers;
using DotValidate.Attributes.Collections;
using DotValidate.Attributes.Dates;
using DotValidate.Attributes.Booleans;

public class CreateUserDto
{
    [Required]
    public string Name { get; set; }

    [Required]
    [Email]
    public string Email { get; set; }

    [Range(18, 120)]
    public int Age { get; set; }

    [StringLength(50, MinimumLength = 3)]
    public string Username { get; set; }

    [Before("today")]
    public DateTime BirthDate { get; set; }

    [Contains("user")]
    public List<string> Roles { get; set; }
}
```

### 3. Use in your endpoints - validation is automatic!

```csharp
// MVC Controller
[HttpPost]
public IActionResult Create(CreateUserDto dto)
{
    // dto is already validated!
    // Invalid requests return 400 with Problem Details
    return Ok(dto);
}

// Minimal API
api.MapPost("/users", (CreateUserDto dto) =>
{
    // dto is already validated!
    return Results.Ok(dto);
});
```

## Available Attributes

### Utilities

| Attribute | Description |
|-----------|-------------|
| `[Required]` | Property must not be null or empty |
| `[RequiredIf(property, values)]` | Required if another property equals any of the values |
| `[RequiredUnless(property, values)]` | Required unless another property equals any of the values |
| `[Different(property)]` | Must have a different value than another property |
| `[Same(property)]` | Must have the same value as another property |
| `[GreaterThan(property)]` | Must be greater than another property (any IComparable) |
| `[GreaterThanOrEqual(property)]` | Must be greater than or equal to another property |
| `[LessThan(property)]` | Must be less than another property |
| `[LessThanOrEqual(property)]` | Must be less than or equal to another property |

### Strings

| Attribute | Description |
|-----------|-------------|
| `[Email]` | Property must be a valid email address |
| `[Url]` | Property must be a valid URL (http or https) |
| `[Alpha]` | Must contain only alphabetic characters (Unicode or ASCII) |
| `[AlphaNum]` | Must contain only letters and numbers |
| `[AlphaDash]` | Must contain only letters, numbers, dashes, and underscores |
| `[Ascii]` | Must contain only 7-bit ASCII characters |
| `[StartsWith(values)]` | Must start with one of the given values |
| `[EndsWith(values)]` | Must end with one of the given values |
| `[DoesntStartWith(values)]` | Must not start with any of the given values |
| `[DoesntEndWith(values)]` | Must not end with any of the given values |
| `[Lowercase]` | Must be entirely lowercase |
| `[Uppercase]` | Must be entirely uppercase |
| `[HexColor]` | Must be a valid hexadecimal color (#RGB, #RRGGBB, #RRGGBBAA) |
| `[Ip]` | Must be a valid IP address (IPv4 or IPv6) |
| `[MacAddress]` | Must be a valid MAC address |
| `[Uuid]` | Must be a valid UUID (GUID) |
| `[StringLength(max)]` | String must not exceed maximum length |
| `[Regex(pattern)]` | String must match the regex pattern |
| `[NotRegex(pattern)]` | String must NOT match the regex pattern |

### Numbers

| Attribute | Description |
|-----------|-------------|
| `[Range(min, max)]` | Numeric property must be within range |
| `[DecimalPlaces(min, max)]` | Must have between min and max decimal places |
| `[Digits(min, max)]` | Integer must have between min and max digits |
| `[MultipleOf(value)]` | Must be a multiple of the specified value |

### Collections

| Attribute | Description |
|-----------|-------------|
| `[Contains(values)]` | Collection must contain all specified values |
| `[Between(min, max)]` | Collection must have between min and max items (inclusive) |
| `[Distinct]` | Collection must contain only unique values (no duplicates) |

### Dates

| Attribute | Description |
|-----------|-------------|
| `[After(date)]` | Date must be after the specified date |
| `[AfterOrEqual(date)]` | Date must be after or equal to the specified date |
| `[Before(date)]` | Date must be before the specified date |
| `[BeforeOrEqual(date)]` | Date must be before or equal to the specified date |
| `[DateEquals(date)]` | Date must equal the specified date |

### Booleans

| Attribute | Description |
|-----------|-------------|
| `[Accepted]` | Must be `true` (e.g., Terms of Service) |
| `[Declined]` | Must be `false` (e.g., opt-out confirmation) |
| `[AcceptedIf(property, values)]` | Must be `true` if another property equals any of the values |
| `[DeclinedIf(property, values)]` | Must be `false` if another property equals any of the values |

### Files

| Attribute | Description |
|-----------|-------------|
| `[FileSize(min, max)]` | File size must be within range (in bytes) |
| `[FileExtensions(".pdf", ".doc")]` | File must have one of the allowed extensions |
| `[MimeTypes("image/png", "application/pdf")]` | File must have one of the allowed MIME types |
| `[Image]` | File must be an image (jpeg, png, gif, bmp, webp, svg, ico, tiff) |

### Attribute Options

```csharp
// Required with empty strings allowed
[Required(AllowEmptyStrings = true)]
public string Name { get; set; }

// StringLength with minimum
[StringLength(100, MinimumLength = 10)]
public string Bio { get; set; }

// Custom error message
[Required(ErrorMessage = "Please provide your name")]
public string Name { get; set; }

// Regex with options
[Regex(@"^\d{3}-\d{4}$", Options = RegexOptions.IgnoreCase)]
public string PhoneNumber { get; set; }

// Must NOT match pattern (e.g., block HTML tags)
[NotRegex(@"<[^>]+>")]
public string Comment { get; set; }

// URL validation (http and https only)
[Url]
public string? Website { get; set; }

// Alphabetic characters only (Unicode by default - allows é, ñ, 中, etc.)
[Alpha]
public string FirstName { get; set; }

// ASCII letters only (a-z, A-Z)
[Alpha(Ascii = true)]
public string Username { get; set; }

// Letters and numbers only (no special characters)
[AlphaNum]
public string Code { get; set; }

// Letters, numbers, dashes, underscores (great for slugs/usernames)
[AlphaDash]
public string Slug { get; set; }

// ASCII only version
[AlphaDash(Ascii = true)]
public string Handle { get; set; }

// Must be 7-bit ASCII characters only (no Unicode)
[Ascii]
public string LegacyData { get; set; }

// Must start with allowed prefixes
[StartsWith("https://", "http://")]
public string Website { get; set; }

// Must end with allowed extensions (case-insensitive)
[EndsWith(".pdf", ".doc", ".docx", IgnoreCase = true)]
public string DocumentPath { get; set; }

// Must not start with certain prefixes
[DoesntStartWith("admin", "root", "system")]
public string Username { get; set; }

// Must not end with certain file extensions (case-insensitive)
[DoesntEndWith(".exe", ".bat", ".cmd", IgnoreCase = true)]
public string Filename { get; set; }

// Must be lowercase
[Lowercase]
public string Username { get; set; }

// Must be uppercase
[Uppercase]
public string CountryCode { get; set; }

// Must be a valid hex color (#RGB, #RRGGBB, or #RRGGBBAA)
[HexColor]
public string BackgroundColor { get; set; }

// Must be a valid IP address (IPv4 or IPv6)
[Ip]
public string ServerAddress { get; set; }

// IPv4 only
[Ip(Version = IpVersion.V4)]
public string IPv4Address { get; set; }

// IPv6 only
[Ip(Version = IpVersion.V6)]
public string IPv6Address { get; set; }

// Must be a valid MAC address (00:1A:2B:3C:4D:5E or 00-1A-2B-3C-4D-5E)
[MacAddress]
public string DeviceMac { get; set; }

// Must be a valid UUID/GUID
[Uuid]
public string TransactionId { get; set; }

// Collection must contain all specified values
[Contains("admin", "user")]
public List<string> Roles { get; set; }

// Collection must have between 1 and 5 items
[Between(1, 5)]
public List<string> Tags { get; set; }

// Date must be after a specific date
[After("2024-01-01")]
public DateTime StartDate { get; set; }

// Date must be in the future (supports "now", "today", "utcnow")
[After("today")]
public DateTime EventDate { get; set; }

// Date must be before now (in the past)
[Before("now")]
public DateTime BirthDate { get; set; }

// Combine After and Before for date ranges
[After("today")]
[Before("2025-12-31")]
public DateTime BookingDate { get; set; }

// Terms of Service must be accepted (must be true)
[Accepted]
public bool TermsAccepted { get; set; }

// Must be declined (must be false)
[Declined]
public bool OptOut { get; set; }

// Conditional validation - Required if PaymentMethod is "CreditCard" or "DebitCard"
[RequiredIf(nameof(PaymentMethod), "CreditCard", "DebitCard")]
public string? CardNumber { get; set; }

// Required unless PaymentMethod is "Cash"
[RequiredUnless(nameof(PaymentMethod), "Cash")]
public string? BillingAddress { get; set; }

// Must accept terms when paying by card
[AcceptedIf(nameof(PaymentMethod), "CreditCard", "DebitCard")]
public bool AcceptCardTerms { get; set; }

// Cannot access admin panel when user is Guest or Anonymous
[DeclinedIf(nameof(UserRole), "Guest", "Anonymous")]
public bool CanAccessAdmin { get; set; }

// New password must be different from current password
[Different(nameof(CurrentPassword))]
public string NewPassword { get; set; }

// Confirm password must match password
[Same(nameof(Password))]
public string ConfirmPassword { get; set; }

// End date must be after start date
[GreaterThan(nameof(StartDate))]
public DateTime EndDate { get; set; }

// Max price must be at least min price
[GreaterThanOrEqual(nameof(MinPrice))]
public decimal MaxPrice { get; set; }

// Start date must be before end date
[LessThan(nameof(EndDate))]
public DateTime StartDate { get; set; }

// Min quantity must not exceed max quantity
[LessThanOrEqual(nameof(MaxQuantity))]
public int MinQuantity { get; set; }
```

## Error Response Format

When validation fails, DotValidate returns a standard RFC 7807 Problem Details response:

```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "Name": ["Name is required."],
    "Email": ["Email must be a valid email address."]
  }
}
```

## Manual Validation

You can also validate objects manually:

```csharp
var validator = new Validator();
var result = validator.Validate(myObject);

if (!result.IsValid)
{
    foreach (var error in result.Errors)
    {
        Console.WriteLine($"{error.PropertyName}: {error.ErrorMessage}");
    }
}
```

## Target Frameworks

- .NET 10.0 (LTS)
- .NET 8.0 (LTS)

## License

MIT License - see [LICENSE](LICENSE) file for details.

# DotValidate

A modern .NET validation library with automatic ASP.NET Core integration.

## Features

- **Attribute-based validation** - Declare validation rules using attributes
- **Automatic validation** - DTOs are validated automatically in ASP.NET Core
- **RFC 7807 Problem Details** - Standard error response format
- **Zero configuration** - Just add attributes and register the services

## Installation

```bash
dotnet add package DotValidate
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

### Strings

| Attribute | Description |
|-----------|-------------|
| `[Email]` | Property must be a valid email address |
| `[Url]` | Property must be a valid URL (http or https) |
| `[Alpha]` | Must contain only alphabetic characters (Unicode or ASCII) |
| `[AlphaNum]` | Must contain only letters and numbers |
| `[AlphaDash]` | Must contain only letters, numbers, dashes, and underscores |
| `[Ascii]` | Must contain only 7-bit ASCII characters |
| `[StringLength(max)]` | String must not exceed maximum length |
| `[Regex(pattern)]` | String must match the regex pattern |

### Numbers

| Attribute | Description |
|-----------|-------------|
| `[Range(min, max)]` | Numeric property must be within range |

### Collections

| Attribute | Description |
|-----------|-------------|
| `[Contains(values)]` | Collection must contain all specified values |

### Dates

| Attribute | Description |
|-----------|-------------|
| `[After(date)]` | Date must be after the specified date |
| `[Before(date)]` | Date must be before the specified date |

### Booleans

| Attribute | Description |
|-----------|-------------|
| `[Accepted]` | Must be `true` (e.g., Terms of Service) |
| `[Declined]` | Must be `false` (e.g., opt-out confirmation) |
| `[AcceptedIf(property, values)]` | Must be `true` if another property equals any of the values |
| `[DeclinedIf(property, values)]` | Must be `false` if another property equals any of the values |

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

// Collection must contain all specified values
[Contains("admin", "user")]
public List<string> Roles { get; set; }

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

using DotValidation.Attributes.Numbers;
using DotValidation.Attributes.Strings;
using DotValidation.Attributes.Utilities;
using DotValidation.Core;
using Xunit;

namespace DotValidation.Tests;

public class ValidatorTests
{
    private readonly Validator _validator = new();

    public class TestDto
    {
        [Required]
        public string Name { get; set; } = default!;

        [Email]
        public string Email { get; set; } = default!;

        [Range(1, 100)]
        public int Age { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string? Username { get; set; }
    }

    [Fact]
    public void Validate_ValidObject_ReturnsSuccess()
    {
        var dto = new TestDto
        {
            Name = "John Doe",
            Email = "john@example.com",
            Age = 25,
            Username = "johndoe"
        };

        var result = _validator.Validate(dto);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Validate_NullObject_ReturnsSuccess()
    {
        var result = _validator.Validate<TestDto>(null);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_MissingRequired_ReturnsError()
    {
        var dto = new TestDto
        {
            Name = "", // Empty string fails Required
            Email = "john@example.com",
            Age = 25
        };

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Name");
    }

    [Fact]
    public void Validate_InvalidEmail_ReturnsError()
    {
        var dto = new TestDto
        {
            Name = "John",
            Email = "not-an-email",
            Age = 25
        };

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Email");
    }

    [Fact]
    public void Validate_OutOfRange_ReturnsError()
    {
        var dto = new TestDto
        {
            Name = "John",
            Email = "john@example.com",
            Age = 150 // Out of range (1-100)
        };

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Age");
    }

    [Fact]
    public void Validate_StringTooShort_ReturnsError()
    {
        var dto = new TestDto
        {
            Name = "John",
            Email = "john@example.com",
            Age = 25,
            Username = "ab" // Too short (min 3)
        };

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Username");
    }

    [Fact]
    public void Validate_MultipleErrors_ReturnsAllErrors()
    {
        var dto = new TestDto
        {
            Name = "",
            Email = "invalid",
            Age = 0,
            Username = "ab"
        };

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.True(result.Errors.Count >= 3);
    }
}

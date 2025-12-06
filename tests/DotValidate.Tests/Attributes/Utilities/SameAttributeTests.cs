using DotValidate.Attributes.Utilities;
using DotValidate.Core;
using Xunit;

namespace DotValidate.Tests.Attributes.Utilities;

public class SameAttributeTests
{
    private readonly Validator _validator = new();

    private class RegistrationDto
    {
        public string Password { get; set; } = default!;

        [Same(nameof(Password))]
        public string ConfirmPassword { get; set; } = default!;
    }

    private class EmailDto
    {
        public string Email { get; set; } = default!;

        [Same(nameof(Email))]
        public string? ConfirmEmail { get; set; }
    }

    private class NumberDto
    {
        public int Value1 { get; set; }

        [Same(nameof(Value1))]
        public int Value2 { get; set; }
    }

    [Fact]
    public void IsValid_SameValues_ReturnsTrue()
    {
        var dto = new RegistrationDto { Password = "secret123", ConfirmPassword = "secret123" };
        var result = _validator.Validate(dto);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void IsValid_DifferentValues_ReturnsFalse()
    {
        var dto = new RegistrationDto { Password = "secret123", ConfirmPassword = "different" };
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "ConfirmPassword");
    }

    [Fact]
    public void IsValid_BothNull_ReturnsTrue()
    {
        var dto = new EmailDto { Email = null!, ConfirmEmail = null };
        var result = _validator.Validate(dto);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void IsValid_SameEmails_ReturnsTrue()
    {
        var dto = new EmailDto { Email = "test@example.com", ConfirmEmail = "test@example.com" };
        var result = _validator.Validate(dto);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void IsValid_DifferentEmails_ReturnsFalse()
    {
        var dto = new EmailDto { Email = "test@example.com", ConfirmEmail = "other@example.com" };
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
    }

    [Fact]
    public void IsValid_SameNumbers_ReturnsTrue()
    {
        var dto = new NumberDto { Value1 = 42, Value2 = 42 };
        var result = _validator.Validate(dto);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void IsValid_DifferentNumbers_ReturnsFalse()
    {
        var dto = new NumberDto { Value1 = 42, Value2 = 99 };
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
    }

    [Fact]
    public void IsValid_CaseSensitiveStrings_DifferentCase_ReturnsFalse()
    {
        var dto = new RegistrationDto { Password = "Password", ConfirmPassword = "password" };
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid); // Case-sensitive comparison
    }

    [Fact]
    public void FormatErrorMessage_IncludesOtherPropertyName()
    {
        var attribute = new SameAttribute("Password");
        var message = attribute.FormatErrorMessage("ConfirmPassword");
        Assert.Equal("ConfirmPassword must match Password.", message);
    }
}

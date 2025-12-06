using DotValidate.Attributes.Utilities;
using DotValidate.Core;
using Xunit;

namespace DotValidate.Tests.Attributes.Utilities;

public class DifferentAttributeTests
{
    private readonly Validator _validator = new();

    private class PasswordDto
    {
        public string CurrentPassword { get; set; } = default!;

        [Different(nameof(CurrentPassword))]
        public string NewPassword { get; set; } = default!;
    }

    private class EmailDto
    {
        public string PrimaryEmail { get; set; } = default!;

        [Different(nameof(PrimaryEmail))]
        public string? BackupEmail { get; set; }
    }

    private class NumberDto
    {
        public int Value1 { get; set; }

        [Different(nameof(Value1))]
        public int Value2 { get; set; }
    }

    [Fact]
    public void IsValid_DifferentValues_ReturnsTrue()
    {
        var dto = new PasswordDto { CurrentPassword = "oldpass", NewPassword = "newpass" };
        var result = _validator.Validate(dto);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void IsValid_SameValues_ReturnsFalse()
    {
        var dto = new PasswordDto { CurrentPassword = "samepass", NewPassword = "samepass" };
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "NewPassword");
    }

    [Fact]
    public void IsValid_NullValue_ReturnsTrue()
    {
        var dto = new EmailDto { PrimaryEmail = "primary@test.com", BackupEmail = null };
        var result = _validator.Validate(dto);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void IsValid_DifferentEmails_ReturnsTrue()
    {
        var dto = new EmailDto { PrimaryEmail = "primary@test.com", BackupEmail = "backup@test.com" };
        var result = _validator.Validate(dto);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void IsValid_SameEmails_ReturnsFalse()
    {
        var dto = new EmailDto { PrimaryEmail = "same@test.com", BackupEmail = "same@test.com" };
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
    }

    [Fact]
    public void IsValid_DifferentNumbers_ReturnsTrue()
    {
        var dto = new NumberDto { Value1 = 10, Value2 = 20 };
        var result = _validator.Validate(dto);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void IsValid_SameNumbers_ReturnsFalse()
    {
        var dto = new NumberDto { Value1 = 10, Value2 = 10 };
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
    }

    [Fact]
    public void IsValid_CaseSensitiveStrings_DifferentCase_ReturnsTrue()
    {
        var dto = new PasswordDto { CurrentPassword = "Password", NewPassword = "password" };
        var result = _validator.Validate(dto);
        Assert.True(result.IsValid); // Different because case-sensitive
    }

    [Fact]
    public void FormatErrorMessage_IncludesOtherPropertyName()
    {
        var attribute = new DifferentAttribute("OtherField");
        var message = attribute.FormatErrorMessage("MyField");
        Assert.Equal("MyField must be different from OtherField.", message);
    }
}

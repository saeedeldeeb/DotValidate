using DotValidate.Attributes.Booleans;
using DotValidate.Core;
using Xunit;

namespace DotValidate.Tests.Attributes.Booleans;

public class DeclinedIfAttributeTests
{
    private readonly Validator _validator = new();

    private class TestDto
    {
        public string UserRole { get; set; } = default!;

        [DeclinedIf(nameof(UserRole), "Guest", "Anonymous")]
        public bool CanAccessAdmin { get; set; }
    }

    [Fact]
    public void IsValid_ConditionMet_False_ReturnsTrue()
    {
        var dto = new TestDto { UserRole = "Guest", CanAccessAdmin = false };
        var result = _validator.Validate(dto);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void IsValid_ConditionMet_True_ReturnsFalse()
    {
        var dto = new TestDto { UserRole = "Guest", CanAccessAdmin = true };
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "CanAccessAdmin");
    }

    [Fact]
    public void IsValid_ConditionNotMet_True_ReturnsTrue()
    {
        var dto = new TestDto { UserRole = "Admin", CanAccessAdmin = true };
        var result = _validator.Validate(dto);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void IsValid_ConditionNotMet_False_ReturnsTrue()
    {
        var dto = new TestDto { UserRole = "User", CanAccessAdmin = false };
        var result = _validator.Validate(dto);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void IsValid_MultipleValues_AnyMatches_MustBeFalse()
    {
        var dto1 = new TestDto { UserRole = "Guest", CanAccessAdmin = true };
        var dto2 = new TestDto { UserRole = "Anonymous", CanAccessAdmin = true };

        Assert.False(_validator.Validate(dto1).IsValid);
        Assert.False(_validator.Validate(dto2).IsValid);
    }
}

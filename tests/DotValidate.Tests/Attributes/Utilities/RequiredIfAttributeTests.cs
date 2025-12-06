using DotValidate.Attributes.Utilities;
using DotValidate.Core;
using Xunit;

namespace DotValidate.Tests.Attributes.Utilities;

public class RequiredIfAttributeTests
{
    private readonly Validator _validator = new();

    private class TestDto
    {
        public string PaymentMethod { get; set; } = default!;

        [RequiredIf(nameof(PaymentMethod), "CreditCard", "DebitCard")]
        public string? CardNumber { get; set; }
    }

    [Fact]
    public void IsValid_ConditionMet_ValuePresent_ReturnsTrue()
    {
        var dto = new TestDto { PaymentMethod = "CreditCard", CardNumber = "1234567890" };
        var result = _validator.Validate(dto);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void IsValid_ConditionMet_ValueNull_ReturnsFalse()
    {
        var dto = new TestDto { PaymentMethod = "CreditCard", CardNumber = null };
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "CardNumber");
    }

    [Fact]
    public void IsValid_ConditionMet_ValueEmpty_ReturnsFalse()
    {
        var dto = new TestDto { PaymentMethod = "DebitCard", CardNumber = "" };
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
    }

    [Fact]
    public void IsValid_ConditionMet_ValueWhitespace_ReturnsFalse()
    {
        var dto = new TestDto { PaymentMethod = "CreditCard", CardNumber = "   " };
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
    }

    [Fact]
    public void IsValid_ConditionNotMet_ValueNull_ReturnsTrue()
    {
        var dto = new TestDto { PaymentMethod = "Cash", CardNumber = null };
        var result = _validator.Validate(dto);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void IsValid_ConditionNotMet_ValueEmpty_ReturnsTrue()
    {
        var dto = new TestDto { PaymentMethod = "PayPal", CardNumber = "" };
        var result = _validator.Validate(dto);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void IsValid_MultipleValues_AnyMatches_Required()
    {
        var dto1 = new TestDto { PaymentMethod = "CreditCard", CardNumber = null };
        var dto2 = new TestDto { PaymentMethod = "DebitCard", CardNumber = null };

        Assert.False(_validator.Validate(dto1).IsValid);
        Assert.False(_validator.Validate(dto2).IsValid);
    }
}

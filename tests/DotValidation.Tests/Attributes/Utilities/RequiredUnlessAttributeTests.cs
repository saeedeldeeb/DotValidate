using DotValidation.Attributes.Utilities;
using DotValidation.Core;
using Xunit;

namespace DotValidation.Tests.Attributes.Utilities;

public class RequiredUnlessAttributeTests
{
    private readonly Validator _validator = new();

    private class TestDto
    {
        public string PaymentMethod { get; set; } = default!;

        [RequiredUnless(nameof(PaymentMethod), "Cash")]
        public string? BillingAddress { get; set; }
    }

    [Fact]
    public void IsValid_ConditionMet_ValueNull_ReturnsTrue()
    {
        var dto = new TestDto { PaymentMethod = "Cash", BillingAddress = null };
        var result = _validator.Validate(dto);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void IsValid_ConditionMet_ValueEmpty_ReturnsTrue()
    {
        var dto = new TestDto { PaymentMethod = "Cash", BillingAddress = "" };
        var result = _validator.Validate(dto);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void IsValid_ConditionNotMet_ValuePresent_ReturnsTrue()
    {
        var dto = new TestDto { PaymentMethod = "CreditCard", BillingAddress = "123 Main St" };
        var result = _validator.Validate(dto);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void IsValid_ConditionNotMet_ValueNull_ReturnsFalse()
    {
        var dto = new TestDto { PaymentMethod = "CreditCard", BillingAddress = null };
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "BillingAddress");
    }

    [Fact]
    public void IsValid_ConditionNotMet_ValueEmpty_ReturnsFalse()
    {
        var dto = new TestDto { PaymentMethod = "DebitCard", BillingAddress = "" };
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
    }

    [Fact]
    public void IsValid_ConditionNotMet_ValueWhitespace_ReturnsFalse()
    {
        var dto = new TestDto { PaymentMethod = "PayPal", BillingAddress = "   " };
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
    }
}

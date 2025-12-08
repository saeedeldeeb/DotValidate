using DotValidation.Attributes.Booleans;
using DotValidation.Core;
using Xunit;

namespace DotValidation.Tests.Attributes.Booleans;

public class AcceptedIfAttributeTests
{
    private readonly Validator _validator = new();

    private class TestDto
    {
        public string PaymentMethod { get; set; } = default!;

        [AcceptedIf(nameof(PaymentMethod), "CreditCard", "DebitCard")]
        public bool AcceptCardTerms { get; set; }
    }

    [Fact]
    public void IsValid_ConditionMet_True_ReturnsTrue()
    {
        var dto = new TestDto { PaymentMethod = "CreditCard", AcceptCardTerms = true };
        var result = _validator.Validate(dto);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void IsValid_ConditionMet_False_ReturnsFalse()
    {
        var dto = new TestDto { PaymentMethod = "CreditCard", AcceptCardTerms = false };
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "AcceptCardTerms");
    }

    [Fact]
    public void IsValid_ConditionNotMet_False_ReturnsTrue()
    {
        var dto = new TestDto { PaymentMethod = "Cash", AcceptCardTerms = false };
        var result = _validator.Validate(dto);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void IsValid_ConditionNotMet_True_ReturnsTrue()
    {
        var dto = new TestDto { PaymentMethod = "PayPal", AcceptCardTerms = true };
        var result = _validator.Validate(dto);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void IsValid_MultipleValues_AnyMatches_MustBeTrue()
    {
        var dto1 = new TestDto { PaymentMethod = "CreditCard", AcceptCardTerms = false };
        var dto2 = new TestDto { PaymentMethod = "DebitCard", AcceptCardTerms = false };

        Assert.False(_validator.Validate(dto1).IsValid);
        Assert.False(_validator.Validate(dto2).IsValid);
    }
}

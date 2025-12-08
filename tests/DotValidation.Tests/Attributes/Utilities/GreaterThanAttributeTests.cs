using DotValidation.Attributes.Utilities;
using DotValidation.Core;
using Xunit;

namespace DotValidation.Tests.Attributes.Utilities;

public class GreaterThanAttributeTests
{
    private readonly Validator _validator = new();

    private class IntDto
    {
        public int Min { get; set; }

        [GreaterThan(nameof(Min))]
        public int Max { get; set; }
    }

    private class DecimalDto
    {
        public decimal MinPrice { get; set; }

        [GreaterThan(nameof(MinPrice))]
        public decimal MaxPrice { get; set; }
    }

    private class DateDto
    {
        public DateTime StartDate { get; set; }

        [GreaterThan(nameof(StartDate))]
        public DateTime EndDate { get; set; }
    }

    private class NullableDto
    {
        public int? Min { get; set; }

        [GreaterThan(nameof(Min))]
        public int? Max { get; set; }
    }

    [Fact]
    public void IsValid_IntGreater_ReturnsTrue()
    {
        var dto = new IntDto { Min = 10, Max = 20 };
        var result = _validator.Validate(dto);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void IsValid_IntEqual_ReturnsFalse()
    {
        var dto = new IntDto { Min = 10, Max = 10 };
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Max");
    }

    [Fact]
    public void IsValid_IntLess_ReturnsFalse()
    {
        var dto = new IntDto { Min = 20, Max = 10 };
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
    }

    [Fact]
    public void IsValid_DecimalGreater_ReturnsTrue()
    {
        var dto = new DecimalDto { MinPrice = 9.99m, MaxPrice = 19.99m };
        var result = _validator.Validate(dto);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void IsValid_DecimalLess_ReturnsFalse()
    {
        var dto = new DecimalDto { MinPrice = 19.99m, MaxPrice = 9.99m };
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
    }

    [Fact]
    public void IsValid_DateGreater_ReturnsTrue()
    {
        var dto = new DateDto
        {
            StartDate = new DateTime(2024, 1, 1),
            EndDate = new DateTime(2024, 12, 31)
        };
        var result = _validator.Validate(dto);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void IsValid_DateLess_ReturnsFalse()
    {
        var dto = new DateDto
        {
            StartDate = new DateTime(2024, 12, 31),
            EndDate = new DateTime(2024, 1, 1)
        };
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
    }

    [Fact]
    public void IsValid_NullValue_ReturnsTrue()
    {
        var dto = new NullableDto { Min = 10, Max = null };
        var result = _validator.Validate(dto);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void IsValid_NullOtherValue_ReturnsTrue()
    {
        var dto = new NullableDto { Min = null, Max = 10 };
        var result = _validator.Validate(dto);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void FormatErrorMessage_IncludesOtherPropertyName()
    {
        var attribute = new GreaterThanAttribute("MinValue");
        var message = attribute.FormatErrorMessage("MaxValue");
        Assert.Equal("MaxValue must be greater than MinValue.", message);
    }
}

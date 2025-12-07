using DotValidate.Attributes.Utilities;
using DotValidate.Core;
using Xunit;

namespace DotValidate.Tests.Attributes.Numbers;

public class LessThanOrEqualAttributeTests
{
    private readonly Validator _validator = new();

    private class IntDto
    {
        public int Max { get; set; }

        [LessThanOrEqual(nameof(Max))]
        public int Min { get; set; }
    }

    private class DecimalDto
    {
        public decimal MaxPrice { get; set; }

        [LessThanOrEqual(nameof(MaxPrice))]
        public decimal MinPrice { get; set; }
    }

    private class DateDto
    {
        public DateTime EndDate { get; set; }

        [LessThanOrEqual(nameof(EndDate))]
        public DateTime StartDate { get; set; }
    }

    private class NullableDto
    {
        public int? Max { get; set; }

        [LessThanOrEqual(nameof(Max))]
        public int? Min { get; set; }
    }

    [Fact]
    public void IsValid_IntLess_ReturnsTrue()
    {
        var dto = new IntDto { Max = 20, Min = 10 };
        var result = _validator.Validate(dto);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void IsValid_IntEqual_ReturnsTrue()
    {
        var dto = new IntDto { Max = 10, Min = 10 };
        var result = _validator.Validate(dto);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void IsValid_IntGreater_ReturnsFalse()
    {
        var dto = new IntDto { Max = 10, Min = 20 };
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Min");
    }

    [Fact]
    public void IsValid_DecimalLess_ReturnsTrue()
    {
        var dto = new DecimalDto { MaxPrice = 19.99m, MinPrice = 9.99m };
        var result = _validator.Validate(dto);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void IsValid_DecimalEqual_ReturnsTrue()
    {
        var dto = new DecimalDto { MaxPrice = 9.99m, MinPrice = 9.99m };
        var result = _validator.Validate(dto);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void IsValid_DecimalGreater_ReturnsFalse()
    {
        var dto = new DecimalDto { MaxPrice = 9.99m, MinPrice = 19.99m };
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
    }

    [Fact]
    public void IsValid_DateLess_ReturnsTrue()
    {
        var dto = new DateDto
        {
            EndDate = new DateTime(2024, 12, 31),
            StartDate = new DateTime(2024, 1, 1)
        };
        var result = _validator.Validate(dto);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void IsValid_DateEqual_ReturnsTrue()
    {
        var dto = new DateDto
        {
            EndDate = new DateTime(2024, 6, 15),
            StartDate = new DateTime(2024, 6, 15)
        };
        var result = _validator.Validate(dto);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void IsValid_DateGreater_ReturnsFalse()
    {
        var dto = new DateDto
        {
            EndDate = new DateTime(2024, 1, 1),
            StartDate = new DateTime(2024, 12, 31)
        };
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
    }

    [Fact]
    public void IsValid_NullValue_ReturnsTrue()
    {
        var dto = new NullableDto { Max = 10, Min = null };
        var result = _validator.Validate(dto);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void IsValid_NullOtherValue_ReturnsTrue()
    {
        var dto = new NullableDto { Max = null, Min = 10 };
        var result = _validator.Validate(dto);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void FormatErrorMessage_IncludesOtherPropertyName()
    {
        var attribute = new LessThanOrEqualAttribute("MaxValue");
        var message = attribute.FormatErrorMessage("MinValue");
        Assert.Equal("MinValue must be less than or equal to MaxValue.", message);
    }
}

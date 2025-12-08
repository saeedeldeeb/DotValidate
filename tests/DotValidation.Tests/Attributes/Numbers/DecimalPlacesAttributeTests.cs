using DotValidation.Attributes.Numbers;
using Xunit;

namespace DotValidation.Tests.Attributes.Numbers;

public class DecimalPlacesAttributeTests
{
    [Fact]
    public void IsValid_ExactDecimalPlaces_ReturnsTrue()
    {
        var attribute = new DecimalPlacesAttribute(2);
        Assert.True(attribute.IsValid(123.45m));
    }

    [Fact]
    public void IsValid_FewerDecimalPlaces_ReturnsFalse()
    {
        var attribute = new DecimalPlacesAttribute(2);
        Assert.False(attribute.IsValid(123.4m));
    }

    [Fact]
    public void IsValid_MoreDecimalPlaces_ReturnsFalse()
    {
        var attribute = new DecimalPlacesAttribute(2);
        Assert.False(attribute.IsValid(123.456m));
    }

    [Fact]
    public void IsValid_NoDecimalPlaces_WhenZeroRequired_ReturnsTrue()
    {
        var attribute = new DecimalPlacesAttribute(0);
        Assert.True(attribute.IsValid(123m));
    }

    [Fact]
    public void IsValid_WithinRange_ReturnsTrue()
    {
        var attribute = new DecimalPlacesAttribute(1, 3);
        Assert.True(attribute.IsValid(123.4m));
        Assert.True(attribute.IsValid(123.45m));
        Assert.True(attribute.IsValid(123.456m));
    }

    [Fact]
    public void IsValid_BelowRange_ReturnsFalse()
    {
        var attribute = new DecimalPlacesAttribute(2, 4);
        Assert.False(attribute.IsValid(123m));
    }

    [Fact]
    public void IsValid_AboveRange_ReturnsFalse()
    {
        var attribute = new DecimalPlacesAttribute(1, 2);
        Assert.False(attribute.IsValid(123.456m));
    }

    [Fact]
    public void IsValid_Double_ReturnsTrue()
    {
        var attribute = new DecimalPlacesAttribute(2);
        Assert.True(attribute.IsValid(123.45));
    }

    [Fact]
    public void IsValid_Float_ReturnsTrue()
    {
        var attribute = new DecimalPlacesAttribute(1);
        Assert.True(attribute.IsValid(123.4f));
    }

    [Fact]
    public void IsValid_Integer_ZeroDecimalPlaces_ReturnsTrue()
    {
        var attribute = new DecimalPlacesAttribute(0);
        Assert.True(attribute.IsValid(123));
    }

    [Fact]
    public void IsValid_Integer_RequiresDecimalPlaces_ReturnsFalse()
    {
        var attribute = new DecimalPlacesAttribute(2);
        Assert.False(attribute.IsValid(123));
    }

    [Fact]
    public void IsValid_StringNumber_ReturnsTrue()
    {
        var attribute = new DecimalPlacesAttribute(2);
        Assert.True(attribute.IsValid("123.45"));
    }

    [Fact]
    public void IsValid_Null_ReturnsTrue()
    {
        var attribute = new DecimalPlacesAttribute(2);
        Assert.True(attribute.IsValid(null));
    }

    [Fact]
    public void IsValid_NonNumeric_ReturnsFalse()
    {
        var attribute = new DecimalPlacesAttribute(2);
        Assert.False(attribute.IsValid("not a number"));
    }

    [Fact]
    public void IsValid_ZeroWithDecimalPlaces_ReturnsTrue()
    {
        var attribute = new DecimalPlacesAttribute(2);
        Assert.True(attribute.IsValid(0.00m));
    }

    [Fact]
    public void IsValid_NegativeNumber_ReturnsTrue()
    {
        var attribute = new DecimalPlacesAttribute(2);
        Assert.True(attribute.IsValid(-123.45m));
    }

    [Fact]
    public void Constructor_NegativeMinimum_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new DecimalPlacesAttribute(-1));
    }

    [Fact]
    public void Constructor_MaximumLessThanMinimum_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new DecimalPlacesAttribute(3, 1));
    }

    [Fact]
    public void FormatErrorMessage_ExactPlaces_ReturnsCorrectMessage()
    {
        var attribute = new DecimalPlacesAttribute(2);
        var message = attribute.FormatErrorMessage("Price");
        Assert.Equal("Price must have exactly 2 decimal places.", message);
    }

    [Fact]
    public void FormatErrorMessage_Range_ReturnsCorrectMessage()
    {
        var attribute = new DecimalPlacesAttribute(1, 3);
        var message = attribute.FormatErrorMessage("Amount");
        Assert.Equal("Amount must have between 1 and 3 decimal places.", message);
    }

    [Fact]
    public void FormatErrorMessage_CustomMessage_ReturnsCustomMessage()
    {
        var attribute = new DecimalPlacesAttribute(2) { ErrorMessage = "{0} requires {1} decimals" };
        var message = attribute.FormatErrorMessage("Value");
        Assert.Equal("Value requires 2 decimals", message);
    }
}

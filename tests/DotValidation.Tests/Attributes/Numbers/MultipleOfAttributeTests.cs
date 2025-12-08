using DotValidation.Attributes.Numbers;
using Xunit;

namespace DotValidation.Tests.Attributes.Numbers;

public class MultipleOfAttributeTests
{
    [Fact]
    public void IsValid_IsMultiple_ReturnsTrue()
    {
        var attribute = new MultipleOfAttribute(5);
        Assert.True(attribute.IsValid(10));
    }

    [Fact]
    public void IsValid_NotMultiple_ReturnsFalse()
    {
        var attribute = new MultipleOfAttribute(5);
        Assert.False(attribute.IsValid(7));
    }

    [Fact]
    public void IsValid_Zero_IsMultipleOfAny_ReturnsTrue()
    {
        var attribute = new MultipleOfAttribute(5);
        Assert.True(attribute.IsValid(0));
    }

    [Fact]
    public void IsValid_NegativeMultiple_ReturnsTrue()
    {
        var attribute = new MultipleOfAttribute(5);
        Assert.True(attribute.IsValid(-15));
    }

    [Fact]
    public void IsValid_DecimalMultiple_ReturnsTrue()
    {
        var attribute = new MultipleOfAttribute(0.5);
        Assert.True(attribute.IsValid(2.5));
    }

    [Fact]
    public void IsValid_DecimalNotMultiple_ReturnsFalse()
    {
        var attribute = new MultipleOfAttribute(0.5);
        Assert.False(attribute.IsValid(2.3));
    }

    [Fact]
    public void IsValid_LargeNumber_ReturnsTrue()
    {
        var attribute = new MultipleOfAttribute(100);
        Assert.True(attribute.IsValid(1000000));
    }

    [Fact]
    public void IsValid_Long_ReturnsTrue()
    {
        var attribute = new MultipleOfAttribute(7);
        Assert.True(attribute.IsValid(21L));
    }

    [Fact]
    public void IsValid_Decimal_ReturnsTrue()
    {
        var attribute = new MultipleOfAttribute(0.25);
        Assert.True(attribute.IsValid(1.25m));
    }

    [Fact]
    public void IsValid_Float_ReturnsTrue()
    {
        var attribute = new MultipleOfAttribute(2);
        Assert.True(attribute.IsValid(6.0f));
    }

    [Fact]
    public void IsValid_Double_ReturnsTrue()
    {
        var attribute = new MultipleOfAttribute(3);
        Assert.True(attribute.IsValid(9.0));
    }

    [Fact]
    public void IsValid_StringNumber_ReturnsTrue()
    {
        var attribute = new MultipleOfAttribute(4);
        Assert.True(attribute.IsValid("12"));
    }

    [Fact]
    public void IsValid_StringNumber_NotMultiple_ReturnsFalse()
    {
        var attribute = new MultipleOfAttribute(4);
        Assert.False(attribute.IsValid("13"));
    }

    [Fact]
    public void IsValid_Null_ReturnsTrue()
    {
        var attribute = new MultipleOfAttribute(5);
        Assert.True(attribute.IsValid(null));
    }

    [Fact]
    public void IsValid_NonNumeric_ReturnsFalse()
    {
        var attribute = new MultipleOfAttribute(5);
        Assert.False(attribute.IsValid("hello"));
    }

    [Fact]
    public void IsValid_NegativeMultiplier_ReturnsTrue()
    {
        var attribute = new MultipleOfAttribute(-3);
        Assert.True(attribute.IsValid(9));
    }

    [Fact]
    public void IsValid_ExactValue_ReturnsTrue()
    {
        var attribute = new MultipleOfAttribute(7);
        Assert.True(attribute.IsValid(7));
    }

    [Fact]
    public void Constructor_ZeroValue_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new MultipleOfAttribute(0));
    }

    [Fact]
    public void FormatErrorMessage_ReturnsCorrectMessage()
    {
        var attribute = new MultipleOfAttribute(5);
        var message = attribute.FormatErrorMessage("Quantity");
        Assert.Equal("Quantity must be a multiple of 5.", message);
    }

    [Fact]
    public void FormatErrorMessage_DecimalValue_ReturnsCorrectMessage()
    {
        var attribute = new MultipleOfAttribute(0.25);
        var message = attribute.FormatErrorMessage("Price");
        Assert.Equal("Price must be a multiple of 0.25.", message);
    }

    [Fact]
    public void FormatErrorMessage_CustomMessage_ReturnsCustomMessage()
    {
        var attribute = new MultipleOfAttribute(10) { ErrorMessage = "{0} should be divisible by {1}" };
        var message = attribute.FormatErrorMessage("Amount");
        Assert.Equal("Amount should be divisible by 10", message);
    }
}

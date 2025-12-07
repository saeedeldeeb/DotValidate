using DotValidate.Attributes.Numbers;
using Xunit;

namespace DotValidate.Tests.Attributes.Numbers;

public class DigitsAttributeTests
{
    [Fact]
    public void IsValid_ExactDigits_ReturnsTrue()
    {
        var attribute = new DigitsAttribute(5);
        Assert.True(attribute.IsValid(12345));
    }

    [Fact]
    public void IsValid_FewerDigits_ReturnsFalse()
    {
        var attribute = new DigitsAttribute(5);
        Assert.False(attribute.IsValid(1234));
    }

    [Fact]
    public void IsValid_MoreDigits_ReturnsFalse()
    {
        var attribute = new DigitsAttribute(5);
        Assert.False(attribute.IsValid(123456));
    }

    [Fact]
    public void IsValid_SingleDigit_ReturnsTrue()
    {
        var attribute = new DigitsAttribute(1);
        Assert.True(attribute.IsValid(5));
    }

    [Fact]
    public void IsValid_Zero_SingleDigit_ReturnsTrue()
    {
        var attribute = new DigitsAttribute(1);
        Assert.True(attribute.IsValid(0));
    }

    [Fact]
    public void IsValid_WithinRange_ReturnsTrue()
    {
        var attribute = new DigitsAttribute(3, 5);
        Assert.True(attribute.IsValid(123));
        Assert.True(attribute.IsValid(1234));
        Assert.True(attribute.IsValid(12345));
    }

    [Fact]
    public void IsValid_BelowRange_ReturnsFalse()
    {
        var attribute = new DigitsAttribute(3, 5);
        Assert.False(attribute.IsValid(12));
    }

    [Fact]
    public void IsValid_AboveRange_ReturnsFalse()
    {
        var attribute = new DigitsAttribute(3, 5);
        Assert.False(attribute.IsValid(123456));
    }

    [Fact]
    public void IsValid_NegativeNumber_IgnoresSign_ReturnsTrue()
    {
        var attribute = new DigitsAttribute(3);
        Assert.True(attribute.IsValid(-123));
    }

    [Fact]
    public void IsValid_Long_ReturnsTrue()
    {
        var attribute = new DigitsAttribute(10);
        Assert.True(attribute.IsValid(1234567890L));
    }

    [Fact]
    public void IsValid_Short_ReturnsTrue()
    {
        var attribute = new DigitsAttribute(4);
        Assert.True(attribute.IsValid((short)1234));
    }

    [Fact]
    public void IsValid_Byte_ReturnsTrue()
    {
        var attribute = new DigitsAttribute(3);
        Assert.True(attribute.IsValid((byte)255));
    }

    [Fact]
    public void IsValid_StringNumber_ReturnsTrue()
    {
        var attribute = new DigitsAttribute(5);
        Assert.True(attribute.IsValid("12345"));
    }

    [Fact]
    public void IsValid_StringNegativeNumber_ReturnsTrue()
    {
        var attribute = new DigitsAttribute(3);
        Assert.True(attribute.IsValid("-123"));
    }

    [Fact]
    public void IsValid_Null_ReturnsTrue()
    {
        var attribute = new DigitsAttribute(5);
        Assert.True(attribute.IsValid(null));
    }

    [Fact]
    public void IsValid_NonInteger_ReturnsFalse()
    {
        var attribute = new DigitsAttribute(5);
        Assert.False(attribute.IsValid(123.45));
    }

    [Fact]
    public void IsValid_NonNumericString_ReturnsFalse()
    {
        var attribute = new DigitsAttribute(5);
        Assert.False(attribute.IsValid("hello"));
    }

    [Fact]
    public void Constructor_ZeroMinimum_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new DigitsAttribute(0));
    }

    [Fact]
    public void Constructor_NegativeMinimum_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new DigitsAttribute(-1));
    }

    [Fact]
    public void Constructor_MaximumLessThanMinimum_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new DigitsAttribute(5, 3));
    }

    [Fact]
    public void FormatErrorMessage_ExactDigits_ReturnsCorrectMessage()
    {
        var attribute = new DigitsAttribute(5);
        var message = attribute.FormatErrorMessage("ZipCode");
        Assert.Equal("ZipCode must have exactly 5 digits.", message);
    }

    [Fact]
    public void FormatErrorMessage_Range_ReturnsCorrectMessage()
    {
        var attribute = new DigitsAttribute(3, 5);
        var message = attribute.FormatErrorMessage("Code");
        Assert.Equal("Code must have between 3 and 5 digits.", message);
    }

    [Fact]
    public void FormatErrorMessage_CustomMessage_ReturnsCustomMessage()
    {
        var attribute = new DigitsAttribute(5) { ErrorMessage = "{0} needs {1} digits" };
        var message = attribute.FormatErrorMessage("PIN");
        Assert.Equal("PIN needs 5 digits", message);
    }
}

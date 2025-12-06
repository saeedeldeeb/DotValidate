using DotValidate.Attributes.Booleans;
using Xunit;

namespace DotValidate.Tests.Attributes.Booleans;

public class BooleanAttributeTests
{
    private readonly BooleanAttribute _attribute = new();

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void IsValid_BooleanValue_ReturnsTrue(bool value)
    {
        Assert.True(_attribute.IsValid(value));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(0)]
    public void IsValid_IntegerOneOrZero_ReturnsTrue(int value)
    {
        Assert.True(_attribute.IsValid(value));
    }

    [Theory]
    [InlineData("1")]
    [InlineData("0")]
    public void IsValid_StringOneOrZero_ReturnsTrue(string value)
    {
        Assert.True(_attribute.IsValid(value));
    }

    [Theory]
    [InlineData("true")]
    [InlineData("TRUE")]
    [InlineData("True")]
    [InlineData("false")]
    [InlineData("FALSE")]
    [InlineData("False")]
    public void IsValid_StringTrueOrFalse_ReturnsTrue(string value)
    {
        Assert.True(_attribute.IsValid(value));
    }

    [Fact]
    public void IsValid_Null_ReturnsTrue()
    {
        Assert.True(_attribute.IsValid(null));
    }

    [Theory]
    [InlineData("yes")]
    [InlineData("no")]
    [InlineData("on")]
    [InlineData("off")]
    public void IsValid_NonBooleanStrings_ReturnsFalse(string value)
    {
        Assert.False(_attribute.IsValid(value));
    }

    [Fact]
    public void IsValid_RandomString_ReturnsFalse()
    {
        Assert.False(_attribute.IsValid("random"));
    }

    [Fact]
    public void IsValid_EmptyString_ReturnsFalse()
    {
        Assert.False(_attribute.IsValid(""));
    }

    [Fact]
    public void IsValid_OtherInteger_ReturnsFalse()
    {
        Assert.False(_attribute.IsValid(2));
    }

    // Strict mode tests
    [Fact]
    public void IsValid_Strict_BooleanTrue_ReturnsTrue()
    {
        var attribute = new BooleanAttribute { Strict = true };
        Assert.True(attribute.IsValid(true));
    }

    [Fact]
    public void IsValid_Strict_BooleanFalse_ReturnsTrue()
    {
        var attribute = new BooleanAttribute { Strict = true };
        Assert.True(attribute.IsValid(false));
    }

    [Fact]
    public void IsValid_Strict_IntegerOne_ReturnsFalse()
    {
        var attribute = new BooleanAttribute { Strict = true };
        Assert.False(attribute.IsValid(1));
    }

    [Fact]
    public void IsValid_Strict_IntegerZero_ReturnsFalse()
    {
        var attribute = new BooleanAttribute { Strict = true };
        Assert.False(attribute.IsValid(0));
    }

    [Fact]
    public void IsValid_Strict_StringTrue_ReturnsFalse()
    {
        var attribute = new BooleanAttribute { Strict = true };
        Assert.False(attribute.IsValid("true"));
    }

    [Fact]
    public void IsValid_Strict_StringFalse_ReturnsFalse()
    {
        var attribute = new BooleanAttribute { Strict = true };
        Assert.False(attribute.IsValid("false"));
    }

    [Fact]
    public void IsValid_Strict_Null_ReturnsTrue()
    {
        var attribute = new BooleanAttribute { Strict = true };
        Assert.True(attribute.IsValid(null));
    }

    [Fact]
    public void FormatErrorMessage_NonStrict_FormatsCorrectly()
    {
        var message = _attribute.FormatErrorMessage("IsActive");
        Assert.Contains("IsActive", message);
        Assert.Contains("boolean", message.ToLower());
    }

    [Fact]
    public void FormatErrorMessage_Strict_FormatsCorrectly()
    {
        var attribute = new BooleanAttribute { Strict = true };
        var message = attribute.FormatErrorMessage("IsEnabled");
        Assert.Contains("IsEnabled", message);
        Assert.Contains("true or false", message.ToLower());
    }
}

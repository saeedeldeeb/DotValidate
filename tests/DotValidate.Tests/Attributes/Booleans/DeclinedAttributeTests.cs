using DotValidate.Attributes.Booleans;
using Xunit;

namespace DotValidate.Tests.Attributes.Booleans;

public class DeclinedAttributeTests
{
    private readonly DeclinedAttribute _attribute = new();

    [Fact]
    public void IsValid_BooleanFalse_ReturnsTrue()
    {
        Assert.True(_attribute.IsValid(false));
    }

    [Theory]
    [InlineData("false")]
    [InlineData("FALSE")]
    [InlineData("False")]
    public void IsValid_StringFalse_ReturnsTrue(string value)
    {
        Assert.True(_attribute.IsValid(value));
    }

    [Theory]
    [InlineData("no")]
    [InlineData("NO")]
    [InlineData("No")]
    public void IsValid_StringNo_ReturnsTrue(string value)
    {
        Assert.True(_attribute.IsValid(value));
    }

    [Theory]
    [InlineData("off")]
    [InlineData("OFF")]
    [InlineData("Off")]
    public void IsValid_StringOff_ReturnsTrue(string value)
    {
        Assert.True(_attribute.IsValid(value));
    }

    [Fact]
    public void IsValid_IntegerZero_ReturnsTrue()
    {
        Assert.True(_attribute.IsValid(0));
    }

    [Fact]
    public void IsValid_StringZero_ReturnsTrue()
    {
        Assert.True(_attribute.IsValid("0"));
    }

    [Fact]
    public void IsValid_BooleanTrue_ReturnsFalse()
    {
        Assert.False(_attribute.IsValid(true));
    }

    [Fact]
    public void IsValid_StringYes_ReturnsFalse()
    {
        Assert.False(_attribute.IsValid("yes"));
    }

    [Fact]
    public void IsValid_IntegerOne_ReturnsFalse()
    {
        Assert.False(_attribute.IsValid(1));
    }

    [Fact]
    public void IsValid_Null_ReturnsTrue()
    {
        Assert.True(_attribute.IsValid(null));
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
    public void FormatErrorMessage_FormatsCorrectly()
    {
        var message = _attribute.FormatErrorMessage("OptOut");
        Assert.Contains("OptOut", message);
        Assert.Contains("declined", message.ToLower());
    }
}

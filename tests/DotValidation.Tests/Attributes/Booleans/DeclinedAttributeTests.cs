using DotValidation.Attributes.Booleans;
using Xunit;

namespace DotValidation.Tests.Attributes.Booleans;

public class DeclinedAttributeTests
{
    private readonly DeclinedAttribute _attribute = new();

    [Fact]
    public void IsValid_False_ReturnsTrue()
    {
        Assert.True(_attribute.IsValid(false));
    }

    [Fact]
    public void IsValid_True_ReturnsFalse()
    {
        Assert.False(_attribute.IsValid(true));
    }

    [Fact]
    public void IsValid_Null_ReturnsTrue()
    {
        Assert.True(_attribute.IsValid(null));
    }

    [Fact]
    public void IsValid_StringFalse_ReturnsFalse()
    {
        Assert.False(_attribute.IsValid("false"));
    }

    [Fact]
    public void IsValid_IntegerZero_ReturnsFalse()
    {
        Assert.False(_attribute.IsValid(0));
    }

    [Fact]
    public void IsValid_StringNo_ReturnsFalse()
    {
        Assert.False(_attribute.IsValid("no"));
    }

    [Fact]
    public void IsValid_StringOff_ReturnsFalse()
    {
        Assert.False(_attribute.IsValid("off"));
    }

    [Fact]
    public void FormatErrorMessage_FormatsCorrectly()
    {
        var message = _attribute.FormatErrorMessage("OptOut");
        Assert.Contains("OptOut", message);
        Assert.Contains("declined", message.ToLower());
    }
}

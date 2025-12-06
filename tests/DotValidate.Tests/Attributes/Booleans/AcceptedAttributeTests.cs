using DotValidate.Attributes.Booleans;
using Xunit;

namespace DotValidate.Tests.Attributes.Booleans;

public class AcceptedAttributeTests
{
    private readonly AcceptedAttribute _attribute = new();

    [Fact]
    public void IsValid_BooleanTrue_ReturnsTrue()
    {
        Assert.True(_attribute.IsValid(true));
    }

    [Theory]
    [InlineData("true")]
    [InlineData("TRUE")]
    [InlineData("True")]
    public void IsValid_StringTrue_ReturnsTrue(string value)
    {
        Assert.True(_attribute.IsValid(value));
    }

    [Theory]
    [InlineData("yes")]
    [InlineData("YES")]
    [InlineData("Yes")]
    public void IsValid_StringYes_ReturnsTrue(string value)
    {
        Assert.True(_attribute.IsValid(value));
    }

    [Theory]
    [InlineData("on")]
    [InlineData("ON")]
    [InlineData("On")]
    public void IsValid_StringOn_ReturnsTrue(string value)
    {
        Assert.True(_attribute.IsValid(value));
    }

    [Fact]
    public void IsValid_IntegerOne_ReturnsTrue()
    {
        Assert.True(_attribute.IsValid(1));
    }

    [Fact]
    public void IsValid_StringOne_ReturnsTrue()
    {
        Assert.True(_attribute.IsValid("1"));
    }

    [Fact]
    public void IsValid_BooleanFalse_ReturnsFalse()
    {
        Assert.False(_attribute.IsValid(false));
    }

    [Fact]
    public void IsValid_StringNo_ReturnsFalse()
    {
        Assert.False(_attribute.IsValid("no"));
    }

    [Fact]
    public void IsValid_IntegerZero_ReturnsFalse()
    {
        Assert.False(_attribute.IsValid(0));
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
        var message = _attribute.FormatErrorMessage("TermsAccepted");
        Assert.Contains("TermsAccepted", message);
        Assert.Contains("accepted", message.ToLower());
    }
}

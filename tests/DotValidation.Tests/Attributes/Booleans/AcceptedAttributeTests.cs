using DotValidation.Attributes.Booleans;
using Xunit;

namespace DotValidation.Tests.Attributes.Booleans;

public class AcceptedAttributeTests
{
    private readonly AcceptedAttribute _attribute = new();

    [Fact]
    public void IsValid_True_ReturnsTrue()
    {
        Assert.True(_attribute.IsValid(true));
    }

    [Fact]
    public void IsValid_False_ReturnsFalse()
    {
        Assert.False(_attribute.IsValid(false));
    }

    [Fact]
    public void IsValid_Null_ReturnsTrue()
    {
        Assert.True(_attribute.IsValid(null));
    }

    [Fact]
    public void IsValid_StringTrue_ReturnsFalse()
    {
        Assert.False(_attribute.IsValid("true"));
    }

    [Fact]
    public void IsValid_IntegerOne_ReturnsFalse()
    {
        Assert.False(_attribute.IsValid(1));
    }

    [Fact]
    public void IsValid_StringYes_ReturnsFalse()
    {
        Assert.False(_attribute.IsValid("yes"));
    }

    [Fact]
    public void IsValid_StringOn_ReturnsFalse()
    {
        Assert.False(_attribute.IsValid("on"));
    }

    [Fact]
    public void FormatErrorMessage_FormatsCorrectly()
    {
        var message = _attribute.FormatErrorMessage("TermsAccepted");
        Assert.Contains("TermsAccepted", message);
        Assert.Contains("accepted", message.ToLower());
    }
}

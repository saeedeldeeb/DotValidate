using DotValidate.Attributes.Strings;
using Xunit;

namespace DotValidate.Tests.Attributes.Strings;

public class AsciiAttributeTests
{
    private readonly AsciiAttribute _attribute = new();

    // Valid ASCII strings
    [Theory]
    [InlineData("Hello")]
    [InlineData("Hello World")]
    [InlineData("Hello123")]
    [InlineData("Hello-World_123")]
    [InlineData("!@#$%^&*()")]
    [InlineData("user@email.com")]
    [InlineData("https://example.com")]
    [InlineData("Line1\nLine2")]
    [InlineData("Tab\there")]
    [InlineData("123456789")]
    [InlineData(" ")]
    [InlineData("~")]  // ASCII 126
    public void IsValid_ValidAscii_ReturnsTrue(string value)
    {
        Assert.True(_attribute.IsValid(value));
    }

    // Invalid - contains non-ASCII characters
    [Theory]
    [InlineData("Café")]
    [InlineData("naïve")]
    [InlineData("日本語")]
    [InlineData("中文")]
    [InlineData("Привет")]
    [InlineData("Hello™")]
    [InlineData("Price: €100")]
    [InlineData("Temperature: 20°C")]
    [InlineData("Em—dash")]
    public void IsValid_InvalidAscii_ReturnsFalse(string value)
    {
        Assert.False(_attribute.IsValid(value));
    }

    [Fact]
    public void IsValid_Null_ReturnsTrue()
    {
        Assert.True(_attribute.IsValid(null));
    }

    [Fact]
    public void IsValid_EmptyString_ReturnsTrue()
    {
        Assert.True(_attribute.IsValid(string.Empty));
    }

    [Fact]
    public void IsValid_NonString_ReturnsFalse()
    {
        Assert.False(_attribute.IsValid(123));
        Assert.False(_attribute.IsValid(true));
    }

    [Fact]
    public void FormatErrorMessage_ReturnsCorrectMessage()
    {
        var message = _attribute.FormatErrorMessage("Data");
        Assert.Equal("Data must contain only ASCII characters.", message);
    }
}

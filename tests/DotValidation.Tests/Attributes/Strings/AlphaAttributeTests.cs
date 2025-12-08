using DotValidation.Attributes.Strings;
using Xunit;

namespace DotValidation.Tests.Attributes.Strings;

public class AlphaAttributeTests
{
    private readonly AlphaAttribute _attribute = new();
    private readonly AlphaAttribute _asciiAttribute = new() { Ascii = true };

    // Unicode mode (default) - valid cases
    [Theory]
    [InlineData("Hello")]
    [InlineData("hello")]
    [InlineData("HELLO")]
    [InlineData("Café")]
    [InlineData("naïve")]
    [InlineData("Ñoño")]
    [InlineData("日本語")]
    [InlineData("中文")]
    [InlineData("العربية")]
    [InlineData("Привет")]
    public void IsValid_UnicodeMode_ValidAlpha_ReturnsTrue(string value)
    {
        Assert.True(_attribute.IsValid(value));
    }

    // Unicode mode (default) - invalid cases
    [Theory]
    [InlineData("Hello123")]
    [InlineData("Hello World")]
    [InlineData("Hello!")]
    [InlineData("Hello-World")]
    [InlineData("Hello_World")]
    [InlineData("123")]
    [InlineData("!@#")]
    public void IsValid_UnicodeMode_InvalidAlpha_ReturnsFalse(string value)
    {
        Assert.False(_attribute.IsValid(value));
    }

    // ASCII mode - valid cases
    [Theory]
    [InlineData("Hello")]
    [InlineData("hello")]
    [InlineData("HELLO")]
    [InlineData("ABC")]
    [InlineData("xyz")]
    public void IsValid_AsciiMode_ValidAlpha_ReturnsTrue(string value)
    {
        Assert.True(_asciiAttribute.IsValid(value));
    }

    // ASCII mode - invalid cases (including non-ASCII letters)
    [Theory]
    [InlineData("Café")]
    [InlineData("naïve")]
    [InlineData("Ñoño")]
    [InlineData("日本語")]
    [InlineData("Hello123")]
    [InlineData("Hello World")]
    [InlineData("Hello!")]
    public void IsValid_AsciiMode_InvalidAlpha_ReturnsFalse(string value)
    {
        Assert.False(_asciiAttribute.IsValid(value));
    }

    [Fact]
    public void IsValid_Null_ReturnsTrue()
    {
        Assert.True(_attribute.IsValid(null));
        Assert.True(_asciiAttribute.IsValid(null));
    }

    [Fact]
    public void IsValid_EmptyString_ReturnsTrue()
    {
        Assert.True(_attribute.IsValid(string.Empty));
        Assert.True(_asciiAttribute.IsValid(string.Empty));
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
        var message = _attribute.FormatErrorMessage("FirstName");
        Assert.Equal("FirstName must contain only alphabetic characters.", message);
    }
}

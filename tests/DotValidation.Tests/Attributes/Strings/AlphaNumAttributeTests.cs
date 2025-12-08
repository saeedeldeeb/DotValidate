using DotValidation.Attributes.Strings;
using Xunit;

namespace DotValidation.Tests.Attributes.Strings;

public class AlphaNumAttributeTests
{
    private readonly AlphaNumAttribute _attribute = new();
    private readonly AlphaNumAttribute _asciiAttribute = new() { Ascii = true };

    // Unicode mode (default) - valid cases
    [Theory]
    [InlineData("Hello")]
    [InlineData("Hello123")]
    [InlineData("123")]
    [InlineData("ABC123")]
    [InlineData("Café123")]
    [InlineData("日本語123")]
    [InlineData("中文測試")]
    [InlineData("Привет123")]
    [InlineData("العربية")]
    public void IsValid_UnicodeMode_ValidAlphaNum_ReturnsTrue(string value)
    {
        Assert.True(_attribute.IsValid(value));
    }

    // Unicode mode (default) - invalid cases
    [Theory]
    [InlineData("Hello World")]
    [InlineData("Hello-World")]
    [InlineData("Hello_World")]
    [InlineData("Hello!")]
    [InlineData("user@email.com")]
    [InlineData("!@#")]
    [InlineData("hello.world")]
    public void IsValid_UnicodeMode_InvalidAlphaNum_ReturnsFalse(string value)
    {
        Assert.False(_attribute.IsValid(value));
    }

    // ASCII mode - valid cases
    [Theory]
    [InlineData("Hello")]
    [InlineData("Hello123")]
    [InlineData("123")]
    [InlineData("ABC")]
    [InlineData("abc123ABC")]
    public void IsValid_AsciiMode_ValidAlphaNum_ReturnsTrue(string value)
    {
        Assert.True(_asciiAttribute.IsValid(value));
    }

    // ASCII mode - invalid cases (including non-ASCII and special chars)
    [Theory]
    [InlineData("Café")]
    [InlineData("日本語")]
    [InlineData("Hello World")]
    [InlineData("Hello-World")]
    [InlineData("Hello_World")]
    [InlineData("Hello!")]
    public void IsValid_AsciiMode_InvalidAlphaNum_ReturnsFalse(string value)
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
        var message = _attribute.FormatErrorMessage("Code");
        Assert.Equal("Code must contain only letters and numbers.", message);
    }
}

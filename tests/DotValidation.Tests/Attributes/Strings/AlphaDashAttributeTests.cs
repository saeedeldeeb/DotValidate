using DotValidation.Attributes.Strings;
using Xunit;

namespace DotValidation.Tests.Attributes.Strings;

public class AlphaDashAttributeTests
{
    private readonly AlphaDashAttribute _attribute = new();
    private readonly AlphaDashAttribute _asciiAttribute = new() { Ascii = true };

    // Unicode mode (default) - valid cases
    [Theory]
    [InlineData("Hello")]
    [InlineData("Hello123")]
    [InlineData("Hello-World")]
    [InlineData("Hello_World")]
    [InlineData("Hello-World_123")]
    [InlineData("user-name")]
    [InlineData("user_name")]
    [InlineData("Café123")]
    [InlineData("日本語123")]
    [InlineData("中文-測試")]
    [InlineData("Привет_123")]
    [InlineData("123")]
    [InlineData("---")]
    [InlineData("___")]
    public void IsValid_UnicodeMode_ValidAlphaDash_ReturnsTrue(string value)
    {
        Assert.True(_attribute.IsValid(value));
    }

    // Unicode mode (default) - invalid cases
    [Theory]
    [InlineData("Hello World")]
    [InlineData("Hello!")]
    [InlineData("Hello@World")]
    [InlineData("Hello.World")]
    [InlineData("user@email.com")]
    [InlineData("!@#")]
    public void IsValid_UnicodeMode_InvalidAlphaDash_ReturnsFalse(string value)
    {
        Assert.False(_attribute.IsValid(value));
    }

    // ASCII mode - valid cases
    [Theory]
    [InlineData("Hello")]
    [InlineData("Hello123")]
    [InlineData("Hello-World")]
    [InlineData("Hello_World")]
    [InlineData("user-name_123")]
    [InlineData("ABC")]
    [InlineData("123")]
    [InlineData("a-b-c")]
    [InlineData("a_b_c")]
    public void IsValid_AsciiMode_ValidAlphaDash_ReturnsTrue(string value)
    {
        Assert.True(_asciiAttribute.IsValid(value));
    }

    // ASCII mode - invalid cases (including non-ASCII)
    [Theory]
    [InlineData("Café")]
    [InlineData("日本語")]
    [InlineData("Hello World")]
    [InlineData("Hello!")]
    [InlineData("user@name")]
    public void IsValid_AsciiMode_InvalidAlphaDash_ReturnsFalse(string value)
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
        var message = _attribute.FormatErrorMessage("Username");
        Assert.Equal("Username must contain only letters, numbers, dashes, and underscores.", message);
    }
}

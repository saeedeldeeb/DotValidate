using DotValidation.Attributes.Strings;
using Xunit;

namespace DotValidation.Tests.Attributes.Strings;

public class UppercaseAttributeTests
{
    private readonly UppercaseAttribute _attribute = new();

    [Theory]
    [InlineData("HELLO")]
    [InlineData("HELLO WORLD")]
    [InlineData("TEST123")]
    [InlineData("HELLO-WORLD")]
    [InlineData("HELLO_WORLD")]
    [InlineData("123")]
    [InlineData("!@#$%")]
    [InlineData("CAFÉ")]
    public void IsValid_Uppercase_ReturnsTrue(string value)
    {
        Assert.True(_attribute.IsValid(value));
    }

    [Theory]
    [InlineData("Hello")]
    [InlineData("hello")]
    [InlineData("HelloWorld")]
    [InlineData("Hello World")]
    [InlineData("test")]
    [InlineData("Café")]
    public void IsValid_NotUppercase_ReturnsFalse(string value)
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
    }

    [Fact]
    public void FormatErrorMessage_ReturnsCorrectMessage()
    {
        var message = _attribute.FormatErrorMessage("Code");
        Assert.Equal("Code must be uppercase.", message);
    }
}

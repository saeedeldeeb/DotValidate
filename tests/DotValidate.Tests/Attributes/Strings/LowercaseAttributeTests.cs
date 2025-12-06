using DotValidate.Attributes.Strings;
using Xunit;

namespace DotValidate.Tests.Attributes.Strings;

public class LowercaseAttributeTests
{
    private readonly LowercaseAttribute _attribute = new();

    [Theory]
    [InlineData("hello")]
    [InlineData("hello world")]
    [InlineData("test123")]
    [InlineData("hello-world")]
    [InlineData("hello_world")]
    [InlineData("123")]
    [InlineData("!@#$%")]
    [InlineData("café")]
    public void IsValid_Lowercase_ReturnsTrue(string value)
    {
        Assert.True(_attribute.IsValid(value));
    }

    [Theory]
    [InlineData("Hello")]
    [InlineData("HELLO")]
    [InlineData("helloWorld")]
    [InlineData("Hello World")]
    [InlineData("TEST")]
    [InlineData("Café")]
    public void IsValid_NotLowercase_ReturnsFalse(string value)
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
        var message = _attribute.FormatErrorMessage("Username");
        Assert.Equal("Username must be lowercase.", message);
    }
}

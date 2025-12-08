using DotValidation.Attributes.Strings;
using Xunit;

namespace DotValidation.Tests.Attributes.Strings;

public class UrlAttributeTests
{
    private readonly UrlAttribute _attribute = new();

    [Theory]
    [InlineData("https://example.com")]
    [InlineData("http://example.com")]
    [InlineData("https://www.example.com")]
    [InlineData("https://sub.example.com")]
    [InlineData("https://example.com/path")]
    [InlineData("https://example.com/path?query=1")]
    [InlineData("https://example.com/path?query=1&other=2")]
    [InlineData("https://example.com:8080")]
    [InlineData("https://example.com:8080/path")]
    [InlineData("http://localhost")]
    [InlineData("http://localhost:3000")]
    [InlineData("https://example.com/path#fragment")]
    public void IsValid_ValidUrls_ReturnsTrue(string url)
    {
        Assert.True(_attribute.IsValid(url));
    }

    [Theory]
    [InlineData("not-a-url")]
    [InlineData("example.com")]
    [InlineData("www.example.com")]
    [InlineData("://missing-scheme")]
    [InlineData("ftp://example.com")]
    [InlineData("file://example.com")]
    [InlineData("mailto:test@example.com")]
    [InlineData("javascript:alert(1)")]
    public void IsValid_InvalidUrls_ReturnsFalse(string url)
    {
        Assert.False(_attribute.IsValid(url));
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
    public void IsValid_Whitespace_ReturnsTrue()
    {
        Assert.True(_attribute.IsValid("   "));
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
        var message = _attribute.FormatErrorMessage("Website");
        Assert.Equal("Website must be a valid URL.", message);
    }
}

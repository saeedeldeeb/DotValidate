using DotValidation.Attributes.Strings;
using Xunit;

namespace DotValidation.Tests.Attributes.Strings;

public class StartsWithAttributeTests
{
    [Theory]
    [InlineData("https://example.com", "https://", "http://")]
    [InlineData("http://example.com", "https://", "http://")]
    [InlineData("Mr. Smith", "Mr.", "Mrs.", "Ms.")]
    [InlineData("Mrs. Jones", "Mr.", "Mrs.", "Ms.")]
    public void IsValid_StartsWithValidValue_ReturnsTrue(string value, params string[] allowed)
    {
        var attribute = new StartsWithAttribute(allowed);
        Assert.True(attribute.IsValid(value));
    }

    [Theory]
    [InlineData("ftp://example.com", "https://", "http://")]
    [InlineData("Dr. Brown", "Mr.", "Mrs.", "Ms.")]
    [InlineData("hello", "foo", "bar")]
    public void IsValid_DoesntStartWithAllowedValue_ReturnsFalse(string value, params string[] allowed)
    {
        var attribute = new StartsWithAttribute(allowed);
        Assert.False(attribute.IsValid(value));
    }

    [Fact]
    public void IsValid_CaseSensitive_DifferentCase_ReturnsFalse()
    {
        var attribute = new StartsWithAttribute("https://");
        Assert.False(attribute.IsValid("HTTPS://example.com"));
    }

    [Fact]
    public void IsValid_IgnoreCase_DifferentCase_ReturnsTrue()
    {
        var attribute = new StartsWithAttribute("https://", "http://") { IgnoreCase = true };
        Assert.True(attribute.IsValid("HTTPS://example.com"));
        Assert.True(attribute.IsValid("HTTP://example.com"));
    }

    [Fact]
    public void IsValid_Null_ReturnsTrue()
    {
        var attribute = new StartsWithAttribute("foo");
        Assert.True(attribute.IsValid(null));
    }

    [Fact]
    public void IsValid_EmptyString_ReturnsTrue()
    {
        var attribute = new StartsWithAttribute("foo");
        Assert.True(attribute.IsValid(string.Empty));
    }

    [Fact]
    public void IsValid_NonString_ReturnsFalse()
    {
        var attribute = new StartsWithAttribute("foo");
        Assert.False(attribute.IsValid(123));
    }

    [Fact]
    public void Constructor_NoValues_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new StartsWithAttribute());
    }

    [Fact]
    public void FormatErrorMessage_IncludesAllowedValues()
    {
        var attribute = new StartsWithAttribute("https://", "http://");
        var message = attribute.FormatErrorMessage("Url");
        Assert.Equal("Url must start with one of the following: https://, http://.", message);
    }
}

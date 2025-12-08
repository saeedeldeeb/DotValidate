using System.Text.RegularExpressions;
using DotValidation.Attributes.Strings;
using Xunit;

namespace DotValidation.Tests.Attributes.Strings;

public class NotRegexAttributeTests
{
    [Fact]
    public void IsValid_NonMatchingPattern_ReturnsTrue()
    {
        var attribute = new NotRegexAttribute(@"\d+"); // No digits allowed
        Assert.True(attribute.IsValid("hello"));
        Assert.True(attribute.IsValid("abc"));
    }

    [Fact]
    public void IsValid_MatchingPattern_ReturnsFalse()
    {
        var attribute = new NotRegexAttribute(@"\d+"); // No digits allowed
        Assert.False(attribute.IsValid("hello123"));
        Assert.False(attribute.IsValid("123"));
    }

    [Fact]
    public void IsValid_BlockBannedWords_ReturnsCorrectly()
    {
        var attribute = new NotRegexAttribute(@"\b(spam|banned)\b") { Options = RegexOptions.IgnoreCase };

        Assert.True(attribute.IsValid("This is a valid message"));
        Assert.False(attribute.IsValid("This is spam"));
        Assert.False(attribute.IsValid("This is SPAM"));
        Assert.False(attribute.IsValid("banned content"));
    }

    [Fact]
    public void IsValid_BlockSpecialCharacters_ReturnsCorrectly()
    {
        var attribute = new NotRegexAttribute(@"[<>""'&]"); // No HTML special chars

        Assert.True(attribute.IsValid("Hello World"));
        Assert.False(attribute.IsValid("<script>"));
        Assert.False(attribute.IsValid("Hello & World"));
    }

    [Fact]
    public void IsValid_WithOptions_AppliesOptions()
    {
        var attribute = new NotRegexAttribute("TEST") { Options = RegexOptions.IgnoreCase };
        Assert.False(attribute.IsValid("test"));
        Assert.False(attribute.IsValid("TEST"));
        Assert.False(attribute.IsValid("Test"));
    }

    [Fact]
    public void IsValid_Null_ReturnsTrue()
    {
        var attribute = new NotRegexAttribute(@"\d+");
        Assert.True(attribute.IsValid(null));
    }

    [Fact]
    public void IsValid_EmptyString_ReturnsTrue()
    {
        var attribute = new NotRegexAttribute(@"\d+");
        Assert.True(attribute.IsValid(string.Empty));
    }

    [Fact]
    public void IsValid_NonString_ReturnsFalse()
    {
        var attribute = new NotRegexAttribute(@"\d+");
        Assert.False(attribute.IsValid(123));
    }

    [Fact]
    public void Constructor_NullPattern_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new NotRegexAttribute(null!));
    }

    [Fact]
    public void Constructor_EmptyPattern_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new NotRegexAttribute(string.Empty));
    }

    [Fact]
    public void FormatErrorMessage_ReturnsCorrectMessage()
    {
        var attribute = new NotRegexAttribute(@"\d+");
        var message = attribute.FormatErrorMessage("Username");
        Assert.Equal("Username has an invalid format.", message);
    }
}

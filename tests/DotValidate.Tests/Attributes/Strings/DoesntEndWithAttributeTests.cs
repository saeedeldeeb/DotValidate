using DotValidate.Attributes.Strings;
using Xunit;

namespace DotValidate.Tests.Attributes.Strings;

public class DoesntEndWithAttributeTests
{
    [Theory]
    [InlineData("hello world", "foo", "bar")]
    [InlineData("test", "foo", "bar")]
    [InlineData("document", ".exe", ".bat")]
    public void IsValid_DoesntEndWithValues_ReturnsTrue(string value, params string[] forbidden)
    {
        var attribute = new DoesntEndWithAttribute(forbidden);
        Assert.True(attribute.IsValid(value));
    }

    [Theory]
    [InlineData("barfoo", "foo", "bar")]
    [InlineData("foobar", "foo", "bar")]
    [InlineData("virus.exe", ".exe", ".bat")]
    [InlineData("script.bat", ".exe", ".bat")]
    public void IsValid_EndsWithForbiddenValue_ReturnsFalse(string value, params string[] forbidden)
    {
        var attribute = new DoesntEndWithAttribute(forbidden);
        Assert.False(attribute.IsValid(value));
    }

    [Fact]
    public void IsValid_CaseSensitive_DifferentCase_ReturnsTrue()
    {
        var attribute = new DoesntEndWithAttribute(".exe", ".bat");
        Assert.True(attribute.IsValid("file.EXE")); // Different case
    }

    [Fact]
    public void IsValid_IgnoreCase_SameValueDifferentCase_ReturnsFalse()
    {
        var attribute = new DoesntEndWithAttribute(".exe", ".bat") { IgnoreCase = true };
        Assert.False(attribute.IsValid("file.EXE"));
        Assert.False(attribute.IsValid("file.Exe"));
        Assert.False(attribute.IsValid("script.BAT"));
    }

    [Fact]
    public void IsValid_Null_ReturnsTrue()
    {
        var attribute = new DoesntEndWithAttribute("foo");
        Assert.True(attribute.IsValid(null));
    }

    [Fact]
    public void IsValid_EmptyString_ReturnsTrue()
    {
        var attribute = new DoesntEndWithAttribute("foo");
        Assert.True(attribute.IsValid(string.Empty));
    }

    [Fact]
    public void IsValid_NonString_ReturnsFalse()
    {
        var attribute = new DoesntEndWithAttribute("foo");
        Assert.False(attribute.IsValid(123));
    }

    [Fact]
    public void Constructor_NoValues_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new DoesntEndWithAttribute());
    }

    [Fact]
    public void FormatErrorMessage_IncludesForbiddenValues()
    {
        var attribute = new DoesntEndWithAttribute(".exe", ".bat");
        var message = attribute.FormatErrorMessage("Filename");
        Assert.Equal("Filename must not end with any of the following: .exe, .bat.", message);
    }
}

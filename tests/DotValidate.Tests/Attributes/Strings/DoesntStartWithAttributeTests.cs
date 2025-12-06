using DotValidate.Attributes.Strings;
using Xunit;

namespace DotValidate.Tests.Attributes.Strings;

public class DoesntStartWithAttributeTests
{
    [Theory]
    [InlineData("hello world", "foo", "bar")]
    [InlineData("test", "foo", "bar")]
    [InlineData("something", "admin", "root")]
    public void IsValid_DoesntStartWithValues_ReturnsTrue(string value, params string[] forbidden)
    {
        var attribute = new DoesntStartWithAttribute(forbidden);
        Assert.True(attribute.IsValid(value));
    }

    [Theory]
    [InlineData("foobar", "foo", "bar")]
    [InlineData("barfoo", "foo", "bar")]
    [InlineData("admin_user", "admin", "root")]
    [InlineData("root_access", "admin", "root")]
    public void IsValid_StartsWithForbiddenValue_ReturnsFalse(string value, params string[] forbidden)
    {
        var attribute = new DoesntStartWithAttribute(forbidden);
        Assert.False(attribute.IsValid(value));
    }

    [Fact]
    public void IsValid_CaseSensitive_DifferentCase_ReturnsTrue()
    {
        var attribute = new DoesntStartWithAttribute("foo", "bar");
        Assert.True(attribute.IsValid("Foobar")); // Different case
    }

    [Fact]
    public void IsValid_IgnoreCase_SameValueDifferentCase_ReturnsFalse()
    {
        var attribute = new DoesntStartWithAttribute("foo", "bar") { IgnoreCase = true };
        Assert.False(attribute.IsValid("Foobar"));
        Assert.False(attribute.IsValid("FOOBAR"));
        Assert.False(attribute.IsValid("BARfoo"));
    }

    [Fact]
    public void IsValid_Null_ReturnsTrue()
    {
        var attribute = new DoesntStartWithAttribute("foo");
        Assert.True(attribute.IsValid(null));
    }

    [Fact]
    public void IsValid_EmptyString_ReturnsTrue()
    {
        var attribute = new DoesntStartWithAttribute("foo");
        Assert.True(attribute.IsValid(string.Empty));
    }

    [Fact]
    public void IsValid_NonString_ReturnsFalse()
    {
        var attribute = new DoesntStartWithAttribute("foo");
        Assert.False(attribute.IsValid(123));
    }

    [Fact]
    public void Constructor_NoValues_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new DoesntStartWithAttribute());
    }

    [Fact]
    public void FormatErrorMessage_IncludesForbiddenValues()
    {
        var attribute = new DoesntStartWithAttribute("foo", "bar");
        var message = attribute.FormatErrorMessage("Username");
        Assert.Equal("Username must not start with any of the following: foo, bar.", message);
    }
}

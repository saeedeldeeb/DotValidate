using DotValidation.Attributes.Strings;
using Xunit;

namespace DotValidation.Tests.Attributes.Strings;

public class RegexAttributeTests
{
    [Fact]
    public void IsValid_MatchingPattern_ReturnsTrue()
    {
        var attribute = new RegexAttribute(@"^\d{3}-\d{4}$");
        Assert.True(attribute.IsValid("123-4567"));
    }

    [Fact]
    public void IsValid_NonMatchingPattern_ReturnsFalse()
    {
        var attribute = new RegexAttribute(@"^\d{3}-\d{4}$");
        Assert.False(attribute.IsValid("abc-defg"));
    }

    [Fact]
    public void IsValid_Null_ReturnsTrue()
    {
        var attribute = new RegexAttribute(@"^\d+$");
        Assert.True(attribute.IsValid(null));
    }

    [Fact]
    public void IsValid_EmptyString_ReturnsTrue()
    {
        var attribute = new RegexAttribute(@"^\d+$");
        Assert.True(attribute.IsValid(""));
    }
}

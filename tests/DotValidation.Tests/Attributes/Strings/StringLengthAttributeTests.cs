using DotValidation.Attributes.Strings;
using Xunit;

namespace DotValidation.Tests.Attributes.Strings;

public class StringLengthAttributeTests
{
    [Fact]
    public void IsValid_WithinLength_ReturnsTrue()
    {
        var attribute = new StringLengthAttribute(10);
        Assert.True(attribute.IsValid("hello"));
    }

    [Fact]
    public void IsValid_ExceedsLength_ReturnsFalse()
    {
        var attribute = new StringLengthAttribute(5);
        Assert.False(attribute.IsValid("hello world"));
    }

    [Fact]
    public void IsValid_WithMinimum_TooShort_ReturnsFalse()
    {
        var attribute = new StringLengthAttribute(10) { MinimumLength = 3 };
        Assert.False(attribute.IsValid("ab"));
    }

    [Fact]
    public void IsValid_WithMinimum_Valid_ReturnsTrue()
    {
        var attribute = new StringLengthAttribute(10) { MinimumLength = 3 };
        Assert.True(attribute.IsValid("abc"));
    }

    [Fact]
    public void IsValid_Null_ReturnsTrue()
    {
        var attribute = new StringLengthAttribute(10);
        Assert.True(attribute.IsValid(null));
    }
}

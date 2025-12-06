using DotValidate.Attributes.Utilities;
using Xunit;

namespace DotValidate.Tests.Attributes.Utilities;

public class RequiredAttributeTests
{
    private readonly RequiredAttribute _attribute = new();

    [Fact]
    public void IsValid_Null_ReturnsFalse()
    {
        Assert.False(_attribute.IsValid(null));
    }

    [Fact]
    public void IsValid_EmptyString_ReturnsFalse()
    {
        Assert.False(_attribute.IsValid(""));
    }

    [Fact]
    public void IsValid_WhitespaceString_ReturnsFalse()
    {
        Assert.False(_attribute.IsValid("   "));
    }

    [Fact]
    public void IsValid_ValidString_ReturnsTrue()
    {
        Assert.True(_attribute.IsValid("hello"));
    }

    [Fact]
    public void IsValid_AllowEmptyStrings_EmptyString_ReturnsTrue()
    {
        var attribute = new RequiredAttribute { AllowEmptyStrings = true };
        Assert.True(attribute.IsValid(""));
    }
}

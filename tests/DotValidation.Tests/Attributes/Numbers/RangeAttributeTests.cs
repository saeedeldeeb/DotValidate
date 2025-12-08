using DotValidation.Attributes.Numbers;
using Xunit;

namespace DotValidation.Tests.Attributes.Numbers;

public class RangeAttributeTests
{
    [Fact]
    public void IsValid_InRange_ReturnsTrue()
    {
        var attribute = new RangeAttribute(1, 10);
        Assert.True(attribute.IsValid(5));
    }

    [Fact]
    public void IsValid_AtMinimum_ReturnsTrue()
    {
        var attribute = new RangeAttribute(1, 10);
        Assert.True(attribute.IsValid(1));
    }

    [Fact]
    public void IsValid_AtMaximum_ReturnsTrue()
    {
        var attribute = new RangeAttribute(1, 10);
        Assert.True(attribute.IsValid(10));
    }

    [Fact]
    public void IsValid_BelowMinimum_ReturnsFalse()
    {
        var attribute = new RangeAttribute(1, 10);
        Assert.False(attribute.IsValid(0));
    }

    [Fact]
    public void IsValid_AboveMaximum_ReturnsFalse()
    {
        var attribute = new RangeAttribute(1, 10);
        Assert.False(attribute.IsValid(11));
    }

    [Fact]
    public void IsValid_Null_ReturnsTrue()
    {
        var attribute = new RangeAttribute(1, 10);
        Assert.True(attribute.IsValid(null));
    }

    [Fact]
    public void IsValid_DoubleRange_Works()
    {
        var attribute = new RangeAttribute(1.5, 10.5);
        Assert.True(attribute.IsValid(5.0));
        Assert.False(attribute.IsValid(1.0));
    }
}

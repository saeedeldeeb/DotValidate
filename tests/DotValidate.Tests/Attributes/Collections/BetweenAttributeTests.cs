using DotValidate.Attributes.Collections;
using Xunit;

namespace DotValidate.Tests.Attributes.Collections;

public class BetweenAttributeTests
{
    [Fact]
    public void IsValid_CountWithinRange_ReturnsTrue()
    {
        var attribute = new BetweenAttribute(2, 5);
        Assert.True(attribute.IsValid(new[] { 1, 2, 3 }));
    }

    [Fact]
    public void IsValid_CountAtMinimum_ReturnsTrue()
    {
        var attribute = new BetweenAttribute(2, 5);
        Assert.True(attribute.IsValid(new[] { 1, 2 }));
    }

    [Fact]
    public void IsValid_CountAtMaximum_ReturnsTrue()
    {
        var attribute = new BetweenAttribute(2, 5);
        Assert.True(attribute.IsValid(new[] { 1, 2, 3, 4, 5 }));
    }

    [Fact]
    public void IsValid_CountBelowMinimum_ReturnsFalse()
    {
        var attribute = new BetweenAttribute(2, 5);
        Assert.False(attribute.IsValid(new[] { 1 }));
    }

    [Fact]
    public void IsValid_CountAboveMaximum_ReturnsFalse()
    {
        var attribute = new BetweenAttribute(2, 5);
        Assert.False(attribute.IsValid(new[] { 1, 2, 3, 4, 5, 6 }));
    }

    [Fact]
    public void IsValid_EmptyCollection_WhenMinimumIsZero_ReturnsTrue()
    {
        var attribute = new BetweenAttribute(0, 5);
        Assert.True(attribute.IsValid(Array.Empty<int>()));
    }

    [Fact]
    public void IsValid_EmptyCollection_WhenMinimumIsOne_ReturnsFalse()
    {
        var attribute = new BetweenAttribute(1, 5);
        Assert.False(attribute.IsValid(Array.Empty<int>()));
    }

    [Fact]
    public void IsValid_List_ReturnsTrue()
    {
        var attribute = new BetweenAttribute(1, 3);
        Assert.True(attribute.IsValid(new List<string> { "a", "b" }));
    }

    [Fact]
    public void IsValid_HashSet_ReturnsTrue()
    {
        var attribute = new BetweenAttribute(2, 4);
        Assert.True(attribute.IsValid(new HashSet<int> { 1, 2, 3 }));
    }

    [Fact]
    public void IsValid_Dictionary_ReturnsTrue()
    {
        var attribute = new BetweenAttribute(1, 3);
        var dict = new Dictionary<string, int> { { "a", 1 }, { "b", 2 } };
        Assert.True(attribute.IsValid(dict));
    }

    [Fact]
    public void IsValid_Null_ReturnsTrue()
    {
        var attribute = new BetweenAttribute(1, 5);
        Assert.True(attribute.IsValid(null));
    }

    [Fact]
    public void IsValid_NonCollection_ReturnsFalse()
    {
        var attribute = new BetweenAttribute(1, 5);
        Assert.False(attribute.IsValid(123));
    }

    [Fact]
    public void IsValid_String_TreatedAsCharCollection_ReturnsTrue()
    {
        var attribute = new BetweenAttribute(3, 10);
        Assert.True(attribute.IsValid("hello")); // 5 chars
    }

    [Fact]
    public void IsValid_ExactRange_ReturnsTrue()
    {
        var attribute = new BetweenAttribute(3, 3);
        Assert.True(attribute.IsValid(new[] { 1, 2, 3 }));
    }

    [Fact]
    public void IsValid_ExactRange_WrongCount_ReturnsFalse()
    {
        var attribute = new BetweenAttribute(3, 3);
        Assert.False(attribute.IsValid(new[] { 1, 2 }));
    }

    [Fact]
    public void Constructor_NegativeMinimum_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new BetweenAttribute(-1, 5));
    }

    [Fact]
    public void Constructor_MaximumLessThanMinimum_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new BetweenAttribute(5, 2));
    }

    [Fact]
    public void FormatErrorMessage_ReturnsCorrectMessage()
    {
        var attribute = new BetweenAttribute(2, 5);
        var message = attribute.FormatErrorMessage("Tags");
        Assert.Equal("Tags must have between 2 and 5 items.", message);
    }

    [Fact]
    public void FormatErrorMessage_CustomMessage_ReturnsCustomMessage()
    {
        var attribute = new BetweenAttribute(1, 3) { ErrorMessage = "{0} needs {1}-{2} items" };
        var message = attribute.FormatErrorMessage("Items");
        Assert.Equal("Items needs 1-3 items", message);
    }
}

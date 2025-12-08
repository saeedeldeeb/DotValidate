using DotValidation.Attributes.Collections;
using Xunit;

namespace DotValidation.Tests.Attributes.Collections;

public class DistinctAttributeTests
{
    private readonly DistinctAttribute _attribute = new();

    [Fact]
    public void IsValid_UniqueValues_ReturnsTrue()
    {
        Assert.True(_attribute.IsValid(new[] { 1, 2, 3, 4, 5 }));
    }

    [Fact]
    public void IsValid_DuplicateValues_ReturnsFalse()
    {
        Assert.False(_attribute.IsValid(new[] { 1, 2, 3, 2, 5 }));
    }

    [Fact]
    public void IsValid_AllSameValues_ReturnsFalse()
    {
        Assert.False(_attribute.IsValid(new[] { 1, 1, 1, 1 }));
    }

    [Fact]
    public void IsValid_EmptyCollection_ReturnsTrue()
    {
        Assert.True(_attribute.IsValid(Array.Empty<int>()));
    }

    [Fact]
    public void IsValid_SingleItem_ReturnsTrue()
    {
        Assert.True(_attribute.IsValid(new[] { 1 }));
    }

    [Fact]
    public void IsValid_UniqueStrings_ReturnsTrue()
    {
        Assert.True(_attribute.IsValid(new[] { "a", "b", "c" }));
    }

    [Fact]
    public void IsValid_DuplicateStrings_ReturnsFalse()
    {
        Assert.False(_attribute.IsValid(new[] { "a", "b", "a" }));
    }

    [Fact]
    public void IsValid_CaseSensitiveStrings_ReturnsTrue()
    {
        // "A" and "a" are different
        Assert.True(_attribute.IsValid(new[] { "A", "a", "B", "b" }));
    }

    [Fact]
    public void IsValid_List_UniqueValues_ReturnsTrue()
    {
        Assert.True(_attribute.IsValid(new List<int> { 1, 2, 3 }));
    }

    [Fact]
    public void IsValid_List_DuplicateValues_ReturnsFalse()
    {
        Assert.False(_attribute.IsValid(new List<int> { 1, 2, 1 }));
    }

    [Fact]
    public void IsValid_WithNullItems_UniqueNulls_ReturnsFalse()
    {
        // Two null values are duplicates
        Assert.False(_attribute.IsValid(new string?[] { "a", null, "b", null }));
    }

    [Fact]
    public void IsValid_WithSingleNull_ReturnsTrue()
    {
        Assert.True(_attribute.IsValid(new string?[] { "a", null, "b" }));
    }

    [Fact]
    public void IsValid_Null_ReturnsTrue()
    {
        Assert.True(_attribute.IsValid(null));
    }

    [Fact]
    public void IsValid_NonCollection_ReturnsFalse()
    {
        Assert.False(_attribute.IsValid(123));
    }

    [Fact]
    public void IsValid_UniqueObjects_ReturnsTrue()
    {
        var obj1 = new { Id = 1 };
        var obj2 = new { Id = 2 };
        Assert.True(_attribute.IsValid(new[] { obj1, obj2 }));
    }

    [Fact]
    public void IsValid_SameObjectReference_ReturnsFalse()
    {
        var obj = new { Id = 1 };
        Assert.False(_attribute.IsValid(new[] { obj, obj }));
    }

    [Fact]
    public void FormatErrorMessage_ReturnsCorrectMessage()
    {
        var message = _attribute.FormatErrorMessage("Tags");
        Assert.Equal("Tags must contain unique values.", message);
    }

    [Fact]
    public void FormatErrorMessage_CustomMessage_ReturnsCustomMessage()
    {
        var attribute = new DistinctAttribute { ErrorMessage = "{0} has duplicates" };
        var message = attribute.FormatErrorMessage("Items");
        Assert.Equal("Items has duplicates", message);
    }
}

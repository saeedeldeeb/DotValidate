using DotValidation.Attributes.Collections;
using Xunit;

namespace DotValidation.Tests.Attributes.Collections;

public class ContainsAttributeTests
{
    [Fact]
    public void IsValid_SingleValue_Present_ReturnsTrue()
    {
        var attribute = new ContainsAttribute("admin");
        Assert.True(attribute.IsValid(new[] { "admin", "user" }));
    }

    [Fact]
    public void IsValid_SingleValue_Missing_ReturnsFalse()
    {
        var attribute = new ContainsAttribute("admin");
        Assert.False(attribute.IsValid(new[] { "user", "guest" }));
    }

    [Fact]
    public void IsValid_MultipleValues_AllPresent_ReturnsTrue()
    {
        var attribute = new ContainsAttribute("a", "b");
        Assert.True(attribute.IsValid(new[] { "a", "b", "c" }));
    }

    [Fact]
    public void IsValid_MultipleValues_PartialPresent_ReturnsFalse()
    {
        var attribute = new ContainsAttribute("a", "b");
        Assert.False(attribute.IsValid(new[] { "a", "c" }));
    }

    [Fact]
    public void IsValid_Null_ReturnsTrue()
    {
        var attribute = new ContainsAttribute("admin");
        Assert.True(attribute.IsValid(null));
    }

    [Fact]
    public void IsValid_EmptyCollection_ReturnsFalse()
    {
        var attribute = new ContainsAttribute("admin");
        Assert.False(attribute.IsValid(Array.Empty<string>()));
    }

    [Fact]
    public void IsValid_NonCollection_ReturnsFalse()
    {
        var attribute = new ContainsAttribute("admin");
        Assert.False(attribute.IsValid("not a collection"));
    }

    [Fact]
    public void IsValid_IntegerArray_AllPresent_ReturnsTrue()
    {
        var attribute = new ContainsAttribute(1, 2);
        Assert.True(attribute.IsValid(new[] { 1, 2, 3 }));
    }

    [Fact]
    public void IsValid_List_Works()
    {
        var attribute = new ContainsAttribute("x", "y");
        Assert.True(attribute.IsValid(new List<string> { "x", "y", "z" }));
    }

    [Fact]
    public void Constructor_NoValues_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new ContainsAttribute());
    }

    [Fact]
    public void FormatErrorMessage_SingleValue_FormatsCorrectly()
    {
        var attribute = new ContainsAttribute("admin");
        var message = attribute.FormatErrorMessage("Roles");
        Assert.Contains("admin", message);
        Assert.Contains("Roles", message);
    }

    [Fact]
    public void FormatErrorMessage_MultipleValues_FormatsCorrectly()
    {
        var attribute = new ContainsAttribute("a", "b", "c");
        var message = attribute.FormatErrorMessage("Items");
        Assert.Contains("Items", message);
        Assert.Contains("'a'", message);
        Assert.Contains("'b'", message);
        Assert.Contains("'c'", message);
    }
}

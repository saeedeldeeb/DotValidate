using DotValidation.Attributes.Strings;
using Xunit;

namespace DotValidation.Tests.Attributes.Strings;

public class HexColorAttributeTests
{
    private readonly HexColorAttribute _attribute = new();

    // Valid hex colors with # prefix
    [Theory]
    [InlineData("#fff")]
    [InlineData("#FFF")]
    [InlineData("#000")]
    [InlineData("#abc")]
    [InlineData("#ABC")]
    [InlineData("#ffffff")]
    [InlineData("#FFFFFF")]
    [InlineData("#000000")]
    [InlineData("#ff0000")]
    [InlineData("#00ff00")]
    [InlineData("#0000ff")]
    [InlineData("#FF5733")]
    [InlineData("#ffffffff")]
    [InlineData("#FFFFFFFF")]
    [InlineData("#ff000080")]
    public void IsValid_ValidHexColorWithHash_ReturnsTrue(string value)
    {
        Assert.True(_attribute.IsValid(value));
    }

    // Valid hex colors without # prefix
    [Theory]
    [InlineData("fff")]
    [InlineData("FFF")]
    [InlineData("ffffff")]
    [InlineData("FFFFFF")]
    [InlineData("FF5733")]
    [InlineData("ffffffff")]
    [InlineData("ff000080")]
    public void IsValid_ValidHexColorWithoutHash_ReturnsTrue(string value)
    {
        Assert.True(_attribute.IsValid(value));
    }

    // Invalid hex colors
    [Theory]
    [InlineData("#ff")]           // Too short
    [InlineData("#ffff")]         // 4 chars (invalid)
    [InlineData("#fffff")]        // 5 chars (invalid)
    [InlineData("#fffffff")]      // 7 chars (invalid)
    [InlineData("#fffffffff")]    // 9 chars (too long)
    [InlineData("#gggggg")]       // Invalid characters
    [InlineData("#xyz")]          // Invalid characters
    [InlineData("red")]           // Color name
    [InlineData("rgb(255,0,0)")]  // RGB format
    [InlineData("##ffffff")]      // Double hash
    public void IsValid_InvalidHexColor_ReturnsFalse(string value)
    {
        Assert.False(_attribute.IsValid(value));
    }

    [Fact]
    public void IsValid_Null_ReturnsTrue()
    {
        Assert.True(_attribute.IsValid(null));
    }

    [Fact]
    public void IsValid_EmptyString_ReturnsTrue()
    {
        Assert.True(_attribute.IsValid(string.Empty));
    }

    [Fact]
    public void IsValid_NonString_ReturnsFalse()
    {
        Assert.False(_attribute.IsValid(123));
    }

    [Fact]
    public void FormatErrorMessage_ReturnsCorrectMessage()
    {
        var message = _attribute.FormatErrorMessage("BackgroundColor");
        Assert.Equal("BackgroundColor must be a valid hexadecimal color.", message);
    }
}

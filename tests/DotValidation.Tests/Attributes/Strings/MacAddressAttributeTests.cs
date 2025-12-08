using DotValidation.Attributes.Strings;
using Xunit;

namespace DotValidation.Tests.Attributes.Strings;

public class MacAddressAttributeTests
{
    private readonly MacAddressAttribute _attribute = new();

    // Valid MAC addresses with colons
    [Theory]
    [InlineData("00:1A:2B:3C:4D:5E")]
    [InlineData("00:1a:2b:3c:4d:5e")]
    [InlineData("FF:FF:FF:FF:FF:FF")]
    [InlineData("00:00:00:00:00:00")]
    [InlineData("A1:B2:C3:D4:E5:F6")]
    public void IsValid_ValidMacWithColons_ReturnsTrue(string value)
    {
        Assert.True(_attribute.IsValid(value));
    }

    // Valid MAC addresses with hyphens
    [Theory]
    [InlineData("00-1A-2B-3C-4D-5E")]
    [InlineData("00-1a-2b-3c-4d-5e")]
    [InlineData("FF-FF-FF-FF-FF-FF")]
    [InlineData("00-00-00-00-00-00")]
    [InlineData("A1-B2-C3-D4-E5-F6")]
    public void IsValid_ValidMacWithHyphens_ReturnsTrue(string value)
    {
        Assert.True(_attribute.IsValid(value));
    }

    // Valid MAC addresses without separators
    [Theory]
    [InlineData("001A2B3C4D5E")]
    [InlineData("001a2b3c4d5e")]
    [InlineData("FFFFFFFFFFFF")]
    [InlineData("000000000000")]
    [InlineData("A1B2C3D4E5F6")]
    public void IsValid_ValidMacWithoutSeparators_ReturnsTrue(string value)
    {
        Assert.True(_attribute.IsValid(value));
    }

    // Invalid MAC addresses
    [Theory]
    [InlineData("00:1A:2B:3C:4D")]        // Too short
    [InlineData("00:1A:2B:3C:4D:5E:FF")]  // Too long
    [InlineData("00:1A:2B:3C:4D:GG")]     // Invalid character
    [InlineData("00:1A:2B:3C:4D:5")]      // Incomplete octet
    [InlineData("001A2B3C4D5")]           // Too short (no separator)
    [InlineData("001A2B3C4D5E7")]         // Too long (no separator)
    [InlineData("00.1A.2B.3C.4D.5E")]     // Dots not supported
    [InlineData("not-a-mac-address")]     // Invalid format
    [InlineData("00:1A-2B:3C-4D:5E")]     // Mixed separators
    public void IsValid_InvalidMac_ReturnsFalse(string value)
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
        var message = _attribute.FormatErrorMessage("DeviceMac");
        Assert.Equal("DeviceMac must be a valid MAC address.", message);
    }
}

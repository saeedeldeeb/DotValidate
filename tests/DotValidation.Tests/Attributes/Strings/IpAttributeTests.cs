using DotValidation.Attributes.Strings;
using Xunit;

namespace DotValidation.Tests.Attributes.Strings;

public class IpAttributeTests
{
    private readonly IpAttribute _attribute = new();
    private readonly IpAttribute _ipv4Attribute = new() { Version = IpVersion.V4 };
    private readonly IpAttribute _ipv6Attribute = new() { Version = IpVersion.V6 };

    // Valid IPv4 addresses (default mode)
    [Theory]
    [InlineData("192.168.1.1")]
    [InlineData("10.0.0.1")]
    [InlineData("172.16.0.1")]
    [InlineData("255.255.255.255")]
    [InlineData("0.0.0.0")]
    [InlineData("127.0.0.1")]
    public void IsValid_ValidIPv4_ReturnsTrue(string value)
    {
        Assert.True(_attribute.IsValid(value));
    }

    // Valid IPv6 addresses (default mode)
    [Theory]
    [InlineData("::1")]
    [InlineData("::")]
    [InlineData("2001:db8::1")]
    [InlineData("fe80::1")]
    [InlineData("2001:0db8:85a3:0000:0000:8a2e:0370:7334")]
    [InlineData("2001:db8:85a3::8a2e:370:7334")]
    public void IsValid_ValidIPv6_ReturnsTrue(string value)
    {
        Assert.True(_attribute.IsValid(value));
    }

    // Invalid IP addresses
    [Theory]
    [InlineData("256.256.256.256")]
    [InlineData("192.168.1.1.1")]
    [InlineData("not-an-ip")]
    [InlineData("192.168.1.a")]
    [InlineData("abc::xyz")]
    public void IsValid_InvalidIP_ReturnsFalse(string value)
    {
        Assert.False(_attribute.IsValid(value));
    }

    // IPv4 only mode
    [Theory]
    [InlineData("192.168.1.1")]
    [InlineData("10.0.0.1")]
    [InlineData("127.0.0.1")]
    public void IsValid_IPv4Mode_ValidIPv4_ReturnsTrue(string value)
    {
        Assert.True(_ipv4Attribute.IsValid(value));
    }

    [Theory]
    [InlineData("::1")]
    [InlineData("2001:db8::1")]
    [InlineData("fe80::1")]
    public void IsValid_IPv4Mode_IPv6Address_ReturnsFalse(string value)
    {
        Assert.False(_ipv4Attribute.IsValid(value));
    }

    // IPv6 only mode
    [Theory]
    [InlineData("::1")]
    [InlineData("2001:db8::1")]
    [InlineData("fe80::1")]
    public void IsValid_IPv6Mode_ValidIPv6_ReturnsTrue(string value)
    {
        Assert.True(_ipv6Attribute.IsValid(value));
    }

    [Theory]
    [InlineData("192.168.1.1")]
    [InlineData("10.0.0.1")]
    [InlineData("127.0.0.1")]
    public void IsValid_IPv6Mode_IPv4Address_ReturnsFalse(string value)
    {
        Assert.False(_ipv6Attribute.IsValid(value));
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
    public void FormatErrorMessage_Default_ReturnsCorrectMessage()
    {
        var message = _attribute.FormatErrorMessage("ServerAddress");
        Assert.Equal("ServerAddress must be a valid IP address.", message);
    }

    [Fact]
    public void FormatErrorMessage_IPv4_ReturnsCorrectMessage()
    {
        var message = _ipv4Attribute.FormatErrorMessage("ServerAddress");
        Assert.Equal("ServerAddress must be a valid IPv4 address.", message);
    }

    [Fact]
    public void FormatErrorMessage_IPv6_ReturnsCorrectMessage()
    {
        var message = _ipv6Attribute.FormatErrorMessage("ServerAddress");
        Assert.Equal("ServerAddress must be a valid IPv6 address.", message);
    }
}

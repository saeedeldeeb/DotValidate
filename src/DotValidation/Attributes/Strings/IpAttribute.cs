using System.Net;
using System.Net.Sockets;

namespace DotValidation.Attributes.Strings;

/// <summary>
/// Specifies that a string property must be a valid IP address.
/// Supports both IPv4 and IPv6 addresses by default.
/// </summary>
public sealed class IpAttribute : ValidationAttribute
{
    /// <summary>
    /// When set to V4, only IPv4 addresses are valid.
    /// When set to V6, only IPv6 addresses are valid.
    /// When Any (default), both IPv4 and IPv6 are valid.
    /// </summary>
    public IpVersion Version { get; set; } = IpVersion.Any;

    protected override string DefaultErrorMessage => Version switch
    {
        IpVersion.V4 => "{0} must be a valid IPv4 address.",
        IpVersion.V6 => "{0} must be a valid IPv6 address.",
        IpVersion.Any or _ => "{0} must be a valid IP address."
    };

    public override bool IsValid(object? value)
    {
        if (value is null)
        {
            return true; // Use [Required] to enforce non-null
        }

        if (value is not string str)
        {
            return false;
        }

        if (string.IsNullOrEmpty(str))
        {
            return true; // Use [Required] to enforce non-empty
        }

        if (!IPAddress.TryParse(str, out var ipAddress))
        {
            return false;
        }

        return Version switch
        {
            IpVersion.V4 => ipAddress.AddressFamily == AddressFamily.InterNetwork,
            IpVersion.V6 => ipAddress.AddressFamily == AddressFamily.InterNetworkV6,
            IpVersion.Any or _ => true
        };
    }
}

/// <summary>
/// Specifies the IP address version.
/// </summary>
public enum IpVersion
{
    /// <summary>
    /// Both IPv4 and IPv6 addresses are valid (default).
    /// </summary>
    Any,

    /// <summary>
    /// IPv4 address (e.g., 192.168.1.1)
    /// </summary>
    V4,

    /// <summary>
    /// IPv6 address (e.g., ::1, 2001:db8::1)
    /// </summary>
    V6
}

using System.Text.RegularExpressions;

namespace DotValidation.Attributes.Strings;

/// <summary>
/// Specifies that a string property must be a valid MAC address.
/// Supports formats: 00:1A:2B:3C:4D:5E, 00-1A-2B-3C-4D-5E, or 001A2B3C4D5E.
/// </summary>
public sealed partial class MacAddressAttribute : ValidationAttribute
{
    protected override string DefaultErrorMessage => "{0} must be a valid MAC address.";

    // Matches MAC addresses in various formats:
    // - Colon-separated: 00:1A:2B:3C:4D:5E
    // - Hyphen-separated: 00-1A-2B-3C-4D-5E
    // - No separator: 001A2B3C4D5E
    [GeneratedRegex(@"^([0-9A-Fa-f]{2}:){5}[0-9A-Fa-f]{2}$|^([0-9A-Fa-f]{2}-){5}[0-9A-Fa-f]{2}$|^[0-9A-Fa-f]{12}$")]
    private static partial Regex MacAddressRegex();

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

        return MacAddressRegex().IsMatch(str);
    }
}

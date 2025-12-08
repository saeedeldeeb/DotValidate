using System.Text.RegularExpressions;

namespace DotValidation.Attributes.Strings;

/// <summary>
/// Specifies that a string property must be a valid hexadecimal color value.
/// Supports formats: #RGB, #RRGGBB, #RRGGBBAA (with or without the # prefix).
/// </summary>
public sealed partial class HexColorAttribute : ValidationAttribute
{
    protected override string DefaultErrorMessage => "{0} must be a valid hexadecimal color.";

    // Matches #RGB, #RRGGBB, #RRGGBBAA (with optional # prefix)
    [GeneratedRegex(@"^#?([0-9A-Fa-f]{3}|[0-9A-Fa-f]{6}|[0-9A-Fa-f]{8})$")]
    private static partial Regex HexColorRegex();

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

        return HexColorRegex().IsMatch(str);
    }
}

using System.Text.RegularExpressions;

namespace DotValidate.Attributes.Strings;

/// <summary>
/// Specifies that a string property must contain only alpha-numeric characters.
/// By default, allows Unicode letters and numbers. Use Ascii = true to restrict to ASCII characters.
/// </summary>
public sealed partial class AlphaNumAttribute : ValidationAttribute
{
    /// <summary>
    /// When true, restricts validation to ASCII characters (a-z, A-Z, 0-9) only.
    /// When false (default), allows all Unicode letters and numbers.
    /// </summary>
    public bool Ascii { get; set; }

    protected override string DefaultErrorMessage => "{0} must contain only letters and numbers.";

    // Unicode: \p{L} (letters), \p{M} (marks), \p{N} (numbers)
    [GeneratedRegex(@"^[\p{L}\p{M}\p{N}]+$")]
    private static partial Regex UnicodeAlphaNumRegex();

    // ASCII only: a-z, A-Z, 0-9
    [GeneratedRegex(@"^[a-zA-Z0-9]+$")]
    private static partial Regex AsciiAlphaNumRegex();

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

        return Ascii
            ? AsciiAlphaNumRegex().IsMatch(str)
            : UnicodeAlphaNumRegex().IsMatch(str);
    }
}

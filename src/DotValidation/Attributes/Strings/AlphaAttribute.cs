using System.Text.RegularExpressions;

namespace DotValidation.Attributes.Strings;

/// <summary>
/// Specifies that a string property must contain only alphabetic characters.
/// By default, allows Unicode letters. Use Ascii = true to restrict to a-z and A-Z.
/// </summary>
public sealed partial class AlphaAttribute : ValidationAttribute
{
    /// <summary>
    /// When true, restricts validation to ASCII letters (a-z, A-Z) only.
    /// When false (default), allows all Unicode alphabetic characters.
    /// </summary>
    public bool Ascii { get; set; }

    protected override string DefaultErrorMessage => "{0} must contain only alphabetic characters.";

    // Unicode: \p{L} (letters) and \p{M} (marks for combining characters)
    [GeneratedRegex(@"^[\p{L}\p{M}]+$")]
    private static partial Regex UnicodeAlphaRegex();

    // ASCII only: a-z, A-Z
    [GeneratedRegex(@"^[a-zA-Z]+$")]
    private static partial Regex AsciiAlphaRegex();

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
            ? AsciiAlphaRegex().IsMatch(str)
            : UnicodeAlphaRegex().IsMatch(str);
    }
}

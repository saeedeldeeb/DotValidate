using System.Text.RegularExpressions;

namespace DotValidate.Attributes.Strings;

/// <summary>
/// Specifies that a string property must contain only alpha-numeric characters, dashes, and underscores.
/// By default, allows Unicode letters and numbers. Use Ascii = true to restrict to ASCII characters.
/// </summary>
public sealed partial class AlphaDashAttribute : ValidationAttribute
{
    /// <summary>
    /// When true, restricts validation to ASCII characters (a-z, A-Z, 0-9, -, _) only.
    /// When false (default), allows all Unicode letters and numbers plus dashes and underscores.
    /// </summary>
    public bool Ascii { get; set; }

    protected override string DefaultErrorMessage => "{0} must contain only letters, numbers, dashes, and underscores.";

    // Unicode: \p{L} (letters), \p{M} (marks), \p{N} (numbers), plus dash and underscore
    [GeneratedRegex(@"^[\p{L}\p{M}\p{N}_-]+$")]
    private static partial Regex UnicodeAlphaDashRegex();

    // ASCII only: a-z, A-Z, 0-9, dash, underscore
    [GeneratedRegex(@"^[a-zA-Z0-9_-]+$")]
    private static partial Regex AsciiAlphaDashRegex();

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
            ? AsciiAlphaDashRegex().IsMatch(str)
            : UnicodeAlphaDashRegex().IsMatch(str);
    }
}

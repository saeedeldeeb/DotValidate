using System.Text.RegularExpressions;

namespace DotValidate.Attributes;

/// <summary>
/// Specifies that a property value must match the specified regular expression pattern.
/// </summary>
public sealed class RegexAttribute : ValidationAttribute
{
    /// <summary>
    /// Gets the regular expression pattern.
    /// </summary>
    public string Pattern { get; }

    /// <summary>
    /// Gets or sets the regex options.
    /// </summary>
    public RegexOptions Options { get; set; } = RegexOptions.None;

    /// <summary>
    /// Gets or sets the timeout for pattern matching.
    /// </summary>
    public int MatchTimeoutInMilliseconds { get; set; } = 2000;

    private Regex? _regex;

    protected override string DefaultErrorMessage => "{0} is not in the correct format.";

    /// <summary>
    /// Initializes a new instance with the specified pattern.
    /// </summary>
    public RegexAttribute(string pattern)
    {
        if (string.IsNullOrEmpty(pattern))
        {
            throw new ArgumentNullException(nameof(pattern));
        }

        Pattern = pattern;
    }

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

        _regex ??= new Regex(
            Pattern,
            Options,
            TimeSpan.FromMilliseconds(MatchTimeoutInMilliseconds)
        );

        return _regex.IsMatch(str);
    }
}

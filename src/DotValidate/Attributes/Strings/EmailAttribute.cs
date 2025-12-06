using System.Text.RegularExpressions;

namespace DotValidate.Attributes.Strings;

/// <summary>
/// Specifies that a property value must be a valid email address.
/// </summary>
public sealed partial class EmailAttribute : ValidationAttribute
{
    // RFC 5322 compliant email regex pattern
    private const string EmailPattern = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$";

    protected override string DefaultErrorMessage => "{0} must be a valid email address.";

    [GeneratedRegex(EmailPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled)]
    private static partial Regex EmailRegex();

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

        if (string.IsNullOrWhiteSpace(str))
        {
            return true; // Use [Required] to enforce non-empty
        }

        return EmailRegex().IsMatch(str);
    }
}

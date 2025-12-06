namespace DotValidate.Attributes.Strings;

/// <summary>
/// Specifies that a string property must be entirely uppercase.
/// </summary>
public sealed class UppercaseAttribute : ValidationAttribute
{
    protected override string DefaultErrorMessage => "{0} must be uppercase.";

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

        return str == str.ToUpperInvariant();
    }
}

namespace DotValidation.Attributes.Strings;

/// <summary>
/// Specifies that a string property must contain only 7-bit ASCII characters (0-127).
/// </summary>
public sealed class AsciiAttribute : ValidationAttribute
{
    protected override string DefaultErrorMessage => "{0} must contain only ASCII characters.";

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

        foreach (var c in str)
        {
            if (c > 127)
            {
                return false;
            }
        }

        return true;
    }
}

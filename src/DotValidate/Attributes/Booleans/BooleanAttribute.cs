namespace DotValidate.Attributes.Booleans;

/// <summary>
/// Specifies that a field must be a valid boolean value.
/// Accepted values: true, false, 1, 0, "1", "0", "true", "false".
/// Use the Strict property to only accept actual true or false values.
/// </summary>
public sealed class BooleanAttribute : ValidationAttribute
{
    /// <summary>
    /// Gets or sets whether to use strict mode.
    /// When true, only actual boolean true or false values are accepted.
    /// </summary>
    public bool Strict { get; set; }

    protected override string DefaultErrorMessage =>
        Strict ? "{0} must be true or false." : "{0} must be a valid boolean value.";

    public override bool IsValid(object? value)
    {
        if (value is null)
        {
            return true; // Use [Required] to enforce non-null
        }

        if (Strict)
        {
            return value is true or false;
        }

        return value switch
        {
            bool => true,
            1 or 0 => true,
            "1" or "0" => true,
            string s => s.Equals("true", StringComparison.OrdinalIgnoreCase) ||
                        s.Equals("false", StringComparison.OrdinalIgnoreCase),
            _ => false
        };
    }
}

namespace DotValidate.Attributes.Booleans;

/// <summary>
/// Specifies that a field must be declined (false, "false", "no", "off", 0, or "0").
/// </summary>
public sealed class DeclinedAttribute : ValidationAttribute
{
    private static readonly HashSet<string> DeclinedStrings = new(StringComparer.OrdinalIgnoreCase)
    {
        "false", "no", "off", "0"
    };

    protected override string DefaultErrorMessage => "{0} must be declined.";

    public override bool IsValid(object? value)
    {
        if (value is null)
        {
            return true; // Use [Required] to enforce non-null
        }

        return value switch
        {
            false => true,
            0 => true,
            string s => DeclinedStrings.Contains(s),
            _ => false
        };
    }
}

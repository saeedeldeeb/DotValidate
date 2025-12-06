namespace DotValidate.Attributes.Booleans;

/// <summary>
/// Specifies that a field must be accepted (true, "true", "yes", "on", 1, or "1").
/// Useful for validating "Terms of Service" acceptance or similar fields.
/// </summary>
public sealed class AcceptedAttribute : ValidationAttribute
{
    private static readonly HashSet<string> AcceptedStrings = new(StringComparer.OrdinalIgnoreCase)
    {
        "true", "yes", "on", "1"
    };

    protected override string DefaultErrorMessage => "{0} must be accepted.";

    public override bool IsValid(object? value)
    {
        if (value is null)
        {
            return true; // Use [Required] to enforce non-null
        }

        return value switch
        {
            true => true,
            1 => true,
            string s => AcceptedStrings.Contains(s),
            _ => false
        };
    }
}

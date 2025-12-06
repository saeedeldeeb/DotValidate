namespace DotValidate.Attributes.Booleans;

/// <summary>
/// Specifies that a boolean field must be false (declined).
/// </summary>
public sealed class DeclinedAttribute : ValidationAttribute
{
    protected override string DefaultErrorMessage => "{0} must be declined.";

    public override bool IsValid(object? value)
    {
        if (value is null)
        {
            return true; // Use [Required] to enforce non-null
        }

        return value is false;
    }
}

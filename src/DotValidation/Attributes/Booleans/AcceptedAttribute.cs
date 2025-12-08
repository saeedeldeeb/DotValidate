namespace DotValidation.Attributes.Booleans;

/// <summary>
/// Specifies that a boolean field must be true (accepted).
/// Useful for validating "Terms of Service" acceptance or similar fields.
/// </summary>
public sealed class AcceptedAttribute : ValidationAttribute
{
    protected override string DefaultErrorMessage => "{0} must be accepted.";

    public override bool IsValid(object? value)
    {
        if (value is null)
        {
            return true; // Use [Required] to enforce non-null
        }

        return value is true;
    }
}

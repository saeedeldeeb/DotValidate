namespace DotValidate.Attributes.Utilities;

/// <summary>
/// Specifies that a property value is required.
/// </summary>
public sealed class RequiredAttribute : ValidationAttribute
{
    /// <summary>
    /// Gets or sets a value indicating whether empty strings are allowed.
    /// </summary>
    public bool AllowEmptyStrings { get; set; }

    protected override string DefaultErrorMessage => "{0} is required.";

    public override bool IsValid(object? value)
    {
        if (value is null)
        {
            return false;
        }

        if (value is string str && !AllowEmptyStrings)
        {
            return !string.IsNullOrWhiteSpace(str);
        }

        return true;
    }
}

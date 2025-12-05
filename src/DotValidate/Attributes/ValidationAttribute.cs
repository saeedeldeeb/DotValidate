namespace DotValidate.Attributes;

/// <summary>
/// Base class for all validation attributes.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true)]
public abstract class ValidationAttribute : Attribute
{
    /// <summary>
    /// Gets or sets the error message to display when validation fails.
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Gets the default error message for this validation attribute.
    /// </summary>
    protected abstract string DefaultErrorMessage { get; }

    /// <summary>
    /// Validates the specified value.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <param name="propertyName">The name of the property being validated.</param>
    /// <returns>True if the value is valid; otherwise, false.</returns>
    public abstract bool IsValid(object? value);

    /// <summary>
    /// Gets the formatted error message for display.
    /// </summary>
    /// <param name="propertyName">The name of the property being validated.</param>
    /// <returns>The error message.</returns>
    public virtual string FormatErrorMessage(string propertyName)
    {
        return string.Format(ErrorMessage ?? DefaultErrorMessage, propertyName);
    }
}

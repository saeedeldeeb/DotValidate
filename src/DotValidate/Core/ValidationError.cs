namespace DotValidate.Core;

/// <summary>
/// Represents a single validation error.
/// </summary>
public sealed class ValidationError
{
    /// <summary>
    /// The name of the property that failed validation.
    /// </summary>
    public string PropertyName { get; }

    /// <summary>
    /// The error message describing the validation failure.
    /// </summary>
    public string ErrorMessage { get; }

    /// <summary>
    /// The value that failed validation.
    /// </summary>
    public object? AttemptedValue { get; }

    public ValidationError(string propertyName, string errorMessage, object? attemptedValue = null)
    {
        PropertyName = propertyName;
        ErrorMessage = errorMessage;
        AttemptedValue = attemptedValue;
    }
}

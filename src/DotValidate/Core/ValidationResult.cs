namespace DotValidate.Core;

/// <summary>
/// Represents the result of validating an object.
/// </summary>
public sealed class ValidationResult
{
    private readonly List<ValidationError> _errors;

    /// <summary>
    /// Gets whether the validation passed (no errors).
    /// </summary>
    public bool IsValid => _errors.Count == 0;

    /// <summary>
    /// Gets the collection of validation errors.
    /// </summary>
    public IReadOnlyList<ValidationError> Errors => _errors;

    public ValidationResult()
    {
        _errors = [];
    }

    public ValidationResult(IEnumerable<ValidationError> errors)
    {
        _errors = errors.ToList();
    }

    /// <summary>
    /// Adds an error to the result.
    /// </summary>
    public void AddError(ValidationError error)
    {
        _errors.Add(error);
    }

    /// <summary>
    /// Adds an error to the result.
    /// </summary>
    public void AddError(string propertyName, string errorMessage, object? attemptedValue = null)
    {
        _errors.Add(new ValidationError(propertyName, errorMessage, attemptedValue));
    }

    /// <summary>
    /// Converts errors to a dictionary format suitable for Problem Details.
    /// </summary>
    public IDictionary<string, string[]> ToDictionary()
    {
        return _errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray()
            );
    }

    /// <summary>
    /// Creates a successful validation result.
    /// </summary>
    public static ValidationResult Success() => new();

    /// <summary>
    /// Creates a failed validation result with the specified errors.
    /// </summary>
    public static ValidationResult Failure(params ValidationError[] errors) => new(errors);
}

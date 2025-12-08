namespace DotValidation.Core;

/// <summary>
/// Provides validation capabilities for objects.
/// </summary>
public interface IValidator
{
    /// <summary>
    /// Validates the specified object.
    /// </summary>
    /// <param name="instance">The object to validate.</param>
    /// <returns>The validation result.</returns>
    ValidationResult Validate(object? instance);

    /// <summary>
    /// Validates the specified object.
    /// </summary>
    /// <typeparam name="T">The type of object to validate.</typeparam>
    /// <param name="instance">The object to validate.</param>
    /// <returns>The validation result.</returns>
    ValidationResult Validate<T>(T? instance);
}

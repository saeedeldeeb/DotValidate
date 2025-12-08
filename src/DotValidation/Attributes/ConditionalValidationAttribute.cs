namespace DotValidation.Attributes;

/// <summary>
/// Base class for validation attributes that depend on the value of another property.
/// </summary>
public abstract class ConditionalValidationAttribute : ValidationAttribute
{
    /// <summary>
    /// Gets the name of the property this validation depends on.
    /// </summary>
    public string DependentProperty { get; }

    /// <summary>
    /// Gets the values that trigger the condition.
    /// </summary>
    public object[] DependentValues { get; }

    /// <summary>
    /// Initializes a new instance with the dependent property and values.
    /// </summary>
    /// <param name="dependentProperty">The name of the property to check.</param>
    /// <param name="dependentValues">The values that trigger the condition.</param>
    protected ConditionalValidationAttribute(string dependentProperty, params object[] dependentValues)
    {
        if (string.IsNullOrWhiteSpace(dependentProperty))
        {
            throw new ArgumentException("Dependent property cannot be null or empty.", nameof(dependentProperty));
        }

        DependentProperty = dependentProperty;
        DependentValues = dependentValues;
    }

    /// <summary>
    /// Checks if the condition is met (dependent property has one of the specified values).
    /// </summary>
    protected bool IsConditionMet(object instance)
    {
        var property = instance.GetType().GetProperty(DependentProperty);
        if (property is null)
        {
            return false;
        }

        var value = property.GetValue(instance);
        return DependentValues.Any(v => Equals(v, value));
    }

    /// <summary>
    /// Validates the specified value against the instance context.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <param name="instance">The object instance containing the property.</param>
    /// <returns>True if the value is valid; otherwise, false.</returns>
    public abstract bool IsValid(object? value, object instance);

    /// <summary>
    /// This overload always returns true. Use IsValid(object?, object) for conditional validation.
    /// </summary>
    public sealed override bool IsValid(object? value) => true;
}

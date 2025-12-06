using System.Reflection;
using DotValidate.Attributes;

namespace DotValidate.Core;

/// <summary>
/// Default implementation of <see cref="IValidator"/>.
/// </summary>
public sealed class Validator : IValidator
{
    /// <inheritdoc />
    public ValidationResult Validate(object? instance)
    {
        if (instance is null)
        {
            return ValidationResult.Success();
        }

        var result = new ValidationResult();
        var type = instance.GetType();
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            var attributes = property.GetCustomAttributes<ValidationAttribute>(inherit: true);
            var value = property.GetValue(instance);

            foreach (var attribute in attributes)
            {
                bool isValid;

                if (attribute is ConditionalValidationAttribute conditional)
                {
                    isValid = conditional.IsValid(value, instance);
                }
                else
                {
                    isValid = attribute.IsValid(value);
                }

                if (!isValid)
                {
                    result.AddError(
                        property.Name,
                        attribute.FormatErrorMessage(property.Name),
                        value
                    );
                }
            }
        }

        return result;
    }

    /// <inheritdoc />
    public ValidationResult Validate<T>(T? instance)
    {
        if (instance is null)
        {
            return ValidationResult.Success();
        }

        return Validate((object)instance);
    }
}

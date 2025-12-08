using DotValidation.Core;
using Microsoft.AspNetCore.Http;

namespace DotValidation.AspNetCore;

/// <summary>
/// Endpoint filter that automatically validates minimal API arguments.
/// </summary>
public sealed class DotValidationEndpointFilter : IEndpointFilter
{
    private readonly IValidator _validator;

    public DotValidationEndpointFilter(IValidator validator)
    {
        _validator = validator;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var allErrors = new Dictionary<string, string[]>();

        foreach (var argument in context.Arguments)
        {
            if (argument is null)
            {
                continue;
            }

            // Skip primitive types and common non-DTO types
            var type = argument.GetType();
            if (type.IsPrimitive || type == typeof(string) || type == typeof(decimal) ||
                type == typeof(DateTime) || type == typeof(DateTimeOffset) ||
                type == typeof(Guid) || type == typeof(CancellationToken) ||
                type.IsEnum)
            {
                continue;
            }

            var result = _validator.Validate(argument);

            if (!result.IsValid)
            {
                foreach (var kvp in result.ToDictionary())
                {
                    if (allErrors.TryGetValue(kvp.Key, out var existing))
                    {
                        allErrors[kvp.Key] = [.. existing, .. kvp.Value];
                    }
                    else
                    {
                        allErrors[kvp.Key] = kvp.Value;
                    }
                }
            }
        }

        if (allErrors.Count > 0)
        {
            return Results.ValidationProblem(allErrors);
        }

        return await next(context);
    }
}

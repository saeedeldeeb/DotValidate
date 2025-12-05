using DotValidate.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DotValidate.AspNetCore;

/// <summary>
/// Action filter that automatically validates action arguments.
/// </summary>
public sealed class DotValidateFilter : IAsyncActionFilter
{
    private readonly IValidator _validator;

    public DotValidateFilter(IValidator validator)
    {
        _validator = validator;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var allErrors = new Dictionary<string, string[]>();

        foreach (var argument in context.ActionArguments.Values)
        {
            if (argument is null)
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
            context.Result = new BadRequestObjectResult(
                new ValidationProblemDetails(allErrors)
            );
            return;
        }

        await next();
    }
}

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace DotValidation.AspNetCore;

/// <summary>
/// Extension methods for configuring DotValidation middleware.
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Adds DotValidation validation to all MVC controllers globally.
    /// </summary>
    /// <param name="builder">The MVC builder.</param>
    /// <returns>The MVC builder so that additional calls can be chained.</returns>
    public static IMvcBuilder AddDotValidationFilter(this IMvcBuilder builder)
    {
        builder.AddMvcOptions(options =>
        {
            options.Filters.AddService<DotValidationFilter>();
        });

        return builder;
    }

    /// <summary>
    /// Adds DotValidation validation to all endpoints in a route group.
    /// </summary>
    /// <param name="builder">The route group builder.</param>
    /// <returns>The route group builder so that additional calls can be chained.</returns>
    public static RouteGroupBuilder AddDotValidationFilter(this RouteGroupBuilder builder)
    {
        builder.AddEndpointFilter<DotValidationEndpointFilter>();
        return builder;
    }

    /// <summary>
    /// Adds DotValidation validation to a specific endpoint.
    /// </summary>
    /// <param name="builder">The route handler builder.</param>
    /// <returns>The route handler builder so that additional calls can be chained.</returns>
    public static RouteHandlerBuilder AddDotValidationFilter(this RouteHandlerBuilder builder)
    {
        builder.AddEndpointFilter<DotValidationEndpointFilter>();
        return builder;
    }

    /// <summary>
    /// Adds DotValidation validation to all minimal API endpoints.
    /// This creates a route group with the validation filter applied.
    /// </summary>
    /// <param name="app">The endpoint route builder.</param>
    /// <returns>A route group with validation enabled.</returns>
    public static RouteGroupBuilder MapDotValidationGroup(this IEndpointRouteBuilder app)
    {
        return app.MapGroup("").AddEndpointFilter<DotValidationEndpointFilter>();
    }
}

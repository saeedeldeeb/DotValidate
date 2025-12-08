using DotValidation.Core;
using Microsoft.Extensions.DependencyInjection;

namespace DotValidation.AspNetCore;

/// <summary>
/// Extension methods for adding DotValidation services to the dependency injection container.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds DotValidation services to the specified IServiceCollection.
    /// </summary>
    /// <param name="services">The IServiceCollection to add services to.</param>
    /// <returns>The IServiceCollection so that additional calls can be chained.</returns>
    public static IServiceCollection AddDotValidation(this IServiceCollection services)
    {
        services.AddSingleton<IValidator, Validator>();
        services.AddScoped<DotValidationFilter>();
        services.AddScoped<DotValidationEndpointFilter>();

        return services;
    }

    /// <summary>
    /// Adds DotValidation services with a custom validator to the specified IServiceCollection.
    /// </summary>
    /// <typeparam name="TValidator">The type of validator to use.</typeparam>
    /// <param name="services">The IServiceCollection to add services to.</param>
    /// <returns>The IServiceCollection so that additional calls can be chained.</returns>
    public static IServiceCollection AddDotValidation<TValidator>(this IServiceCollection services)
        where TValidator : class, IValidator
    {
        services.AddSingleton<IValidator, TValidator>();
        services.AddScoped<DotValidationFilter>();
        services.AddScoped<DotValidationEndpointFilter>();

        return services;
    }
}

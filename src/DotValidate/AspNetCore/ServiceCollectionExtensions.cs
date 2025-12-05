using DotValidate.Core;
using Microsoft.Extensions.DependencyInjection;

namespace DotValidate.AspNetCore;

/// <summary>
/// Extension methods for adding DotValidate services to the dependency injection container.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds DotValidate services to the specified IServiceCollection.
    /// </summary>
    /// <param name="services">The IServiceCollection to add services to.</param>
    /// <returns>The IServiceCollection so that additional calls can be chained.</returns>
    public static IServiceCollection AddDotValidate(this IServiceCollection services)
    {
        services.AddSingleton<IValidator, Validator>();
        services.AddScoped<DotValidateFilter>();
        services.AddScoped<DotValidateEndpointFilter>();

        return services;
    }

    /// <summary>
    /// Adds DotValidate services with a custom validator to the specified IServiceCollection.
    /// </summary>
    /// <typeparam name="TValidator">The type of validator to use.</typeparam>
    /// <param name="services">The IServiceCollection to add services to.</param>
    /// <returns>The IServiceCollection so that additional calls can be chained.</returns>
    public static IServiceCollection AddDotValidate<TValidator>(this IServiceCollection services)
        where TValidator : class, IValidator
    {
        services.AddSingleton<IValidator, TValidator>();
        services.AddScoped<DotValidateFilter>();
        services.AddScoped<DotValidateEndpointFilter>();

        return services;
    }
}

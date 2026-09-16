using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FixedAssets.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFixedAssetsModule(this IServiceCollection services)
    {
        var applicationAssembly = typeof(FixedAssets.Application.AssemblyMarker).Assembly;
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly));
        services.AddValidatorsFromAssembly(applicationAssembly);
        return services;
    }
}

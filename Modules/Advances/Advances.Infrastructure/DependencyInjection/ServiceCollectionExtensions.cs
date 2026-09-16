using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Advances.Infrastructure.DependencyInjection;

/// <summary>
/// Composition root entry point for the Advances module (brief §20/§76 Phase 11 — skeleton only).
/// Host/API calls this once; nothing outside this module reaches into its Application/Domain/
/// Infrastructure projects directly — see docs/modular-monolith-target-architecture.md §6 and
/// Treasury.Infrastructure.DependencyInjection.ServiceCollectionExtensions for the identical shape.
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAdvancesModule(this IServiceCollection services)
    {
        var applicationAssembly = typeof(Advances.Application.AssemblyMarker).Assembly;

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly));
        services.AddValidatorsFromAssembly(applicationAssembly);

        return services;
    }
}

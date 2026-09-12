using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Parties.Infrastructure.DependencyInjection;

/// <summary>
/// Composition root entry point for the Parties module. Host/API calls this once; nothing outside
/// this module reaches into its Application/Domain/Infrastructure projects directly — see
/// docs/modular-monolith-target-architecture.md §13 for why Parties exists and what it owns
/// (Dealer/DealerGroup/DealerType, relocated from Sales).
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPartiesModule(this IServiceCollection services)
    {
        var applicationAssembly = typeof(Parties.Application.AssemblyMarker).Assembly;

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly));
        services.AddValidatorsFromAssembly(applicationAssembly);
        services.AddAutoMapper(cfg => cfg.AddProfile<Parties.Application.MappingProfile>());

        return services;
    }
}

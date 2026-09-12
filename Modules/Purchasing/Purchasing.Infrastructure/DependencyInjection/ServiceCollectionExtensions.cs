using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Purchasing.Infrastructure.DependencyInjection;

/// <summary>
/// Composition root entry point for the Purchasing module. Host/OrgSys.Api calls this once;
/// nothing outside this module reaches into its Application/Domain/Infrastructure projects
/// directly — see docs/modular-monolith-target-architecture.md §6 for the enforced dependency
/// rules. Now owns PurchaseRequisition/PurchaseOrder (see
/// docs/modular-monolith-target-architecture.md §13) — no longer the empty Phase 1 shell.
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPurchasingModule(this IServiceCollection services)
    {
        var applicationAssembly = typeof(Purchasing.Application.MappingProfile).Assembly;

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly));
        services.AddAutoMapper(cfg => cfg.AddProfile<Purchasing.Application.MappingProfile>());
        services.AddValidatorsFromAssembly(applicationAssembly);

        return services;
    }
}

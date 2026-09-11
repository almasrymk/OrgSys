using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Purchasing.Infrastructure.DependencyInjection;

/// <summary>
/// Composition root entry point for the Purchasing module (brief §20). Host/OrgSys.Api calls this
/// once; nothing outside this module reaches into its Application/Domain/Infrastructure
/// projects directly — see docs/modular-monolith-target-architecture.md §6 for the enforced
/// dependency rules.
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPurchasingModule(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly));

        // AutoMapper profile(s), FluentValidation validators, and module-specific services are
        // registered here as each is extracted into this module (see migration-order phases in
        // docs/modular-monolith-analysis.md §19). Empty during Phase 1 — no business code has
        // moved into this module yet.

        return services;
    }
}

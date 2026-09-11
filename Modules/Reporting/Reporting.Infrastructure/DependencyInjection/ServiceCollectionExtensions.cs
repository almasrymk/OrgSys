using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Reporting.Infrastructure.DependencyInjection;

/// <summary>
/// Composition root entry point for the read-only Reporting module (brief §11/§20) — it owns
/// no domain state, only queries other modules' Contracts. See
/// docs/modular-monolith-target-architecture.md §3 (Reporting) and §5.
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddReportingModule(this IServiceCollection services)
    {
        var applicationAssembly = typeof(Reporting.Application.DealerBalance).Assembly;
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly));
        return services;
    }
}

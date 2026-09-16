using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SaaS.Application.Features;
using SaaS.Application.Tenancy;
using SaaS.Contracts.Features;
using SaaS.Contracts.Tenancy;

namespace SaaS.Infrastructure.DependencyInjection;

/// <summary>
/// Composition root entry point for the SaaS module (brief §20). See
/// docs/architecture/adr/tenant-vs-company.md and docs/modular-monolith-target-architecture.md §6
/// for the enforced dependency rules.
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSaaSModule(this IServiceCollection services)
    {
        var applicationAssembly = typeof(SaaS.Application.MappingProfile).Assembly;

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly));
        services.AddAutoMapper(cfg => cfg.AddProfile<SaaS.Application.MappingProfile>());
        services.AddValidatorsFromAssembly(applicationAssembly);

        services.AddScoped<ITenantFeatureService, TenantFeatureService>();
        services.AddScoped<ICurrentTenant, UnresolvedCurrentTenant>();

        return services;
    }
}

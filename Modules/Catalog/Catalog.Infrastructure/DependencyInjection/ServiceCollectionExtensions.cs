using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Infrastructure.DependencyInjection;

/// <summary>
/// Composition root entry point for the Catalog module (mirrors every other module's
/// Add&lt;Module&gt;Module() pattern). Product/Classification/Unit/Property/Brand/PriceList are all
/// simple master data with no explicit invariants beyond validation, so — matching the exact
/// convention these entities already used before relocation (see
/// docs/catalog/catalog-target-architecture.md §4) — there are no bespoke per-aggregate
/// repositories to register here; handlers use the generic OrgSys.SharedKernel.IRepository&lt;T&gt;
/// (registered once, globally, in API/Program.cs) directly.
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCatalogModule(this IServiceCollection services)
    {
        var applicationAssembly = typeof(Catalog.Application.MappingProfile).Assembly;

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly));
        services.AddAutoMapper(cfg => cfg.AddProfile<Catalog.Application.MappingProfile>());
        services.AddValidatorsFromAssembly(applicationAssembly);

        return services;
    }
}

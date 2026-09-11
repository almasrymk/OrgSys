using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Treasury.Infrastructure.DependencyInjection;

/// <summary>
/// Composition root entry point for the Treasury module (brief §20). See
/// docs/modular-monolith-target-architecture.md §6 for the enforced dependency rules.
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTreasuryModule(this IServiceCollection services)
    {
        var applicationAssembly = typeof(Treasury.Application.MappingProfile).Assembly;

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly));
        services.AddAutoMapper(cfg => cfg.AddProfile<Treasury.Application.MappingProfile>());
        services.AddValidatorsFromAssembly(applicationAssembly);

        return services;
    }
}

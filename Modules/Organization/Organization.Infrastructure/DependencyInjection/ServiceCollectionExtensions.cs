using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Organization.Infrastructure.DependencyInjection;

/// <summary>
/// Composition root entry point for the Organization module (brief §20). See
/// docs/modular-monolith-target-architecture.md §6 for the enforced dependency rules.
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOrganizationModule(this IServiceCollection services)
    {
        var applicationAssembly = typeof(Organization.Application.MappingProfile).Assembly;

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly));
        services.AddAutoMapper(cfg => cfg.AddProfile<Organization.Application.MappingProfile>());
        services.AddValidatorsFromAssembly(applicationAssembly);

        return services;
    }
}

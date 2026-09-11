using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Administration.Infrastructure.DependencyInjection;

/// <summary>
/// Composition root entry point for the Administration module (brief §20). See
/// docs/modular-monolith-target-architecture.md §6 for the enforced dependency rules.
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAdministrationModule(this IServiceCollection services)
    {
        var applicationAssembly = typeof(Administration.Application.MappingProfile).Assembly;

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly));
        services.AddAutoMapper(cfg => cfg.AddProfile<Administration.Application.MappingProfile>());
        services.AddValidatorsFromAssembly(applicationAssembly);

        return services;
    }
}

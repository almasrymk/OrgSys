using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Sales.Infrastructure.DependencyInjection;

/// <summary>
/// Composition root entry point for the Sales module (brief §20). See
/// docs/modular-monolith-target-architecture.md §6 for the enforced dependency rules.
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSalesModule(this IServiceCollection services)
    {
        var applicationAssembly = typeof(Sales.Application.MappingProfile).Assembly;

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly));
        services.AddAutoMapper(cfg => cfg.AddProfile<Sales.Application.MappingProfile>());
        services.AddValidatorsFromAssembly(applicationAssembly);

        return services;
    }
}

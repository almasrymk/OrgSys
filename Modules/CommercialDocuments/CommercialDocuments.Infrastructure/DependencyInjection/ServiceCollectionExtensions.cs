using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CommercialDocuments.Infrastructure.DependencyInjection;

/// <summary>
/// Composition root entry point for the CommercialDocuments module. See
/// docs/commercial-documents-module.md and docs/modular-monolith-target-architecture.md.
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommercialDocumentsModule(this IServiceCollection services)
    {
        var applicationAssembly = typeof(CommercialDocuments.Application.MappingProfile).Assembly;

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly));
        services.AddAutoMapper(cfg => cfg.AddProfile<CommercialDocuments.Application.MappingProfile>());
        services.AddValidatorsFromAssembly(applicationAssembly);

        return services;
    }
}

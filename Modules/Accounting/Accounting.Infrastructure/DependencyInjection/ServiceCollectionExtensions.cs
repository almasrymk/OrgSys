using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Accounting.Infrastructure.DependencyInjection;

/// <summary>
/// Composition root entry point for the Accounting module (brief §20). See
/// docs/modular-monolith-target-architecture.md §6 for the enforced dependency rules.
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAccountingModule(this IServiceCollection services)
    {
        var applicationAssembly = typeof(Accounting.Application.MappingProfile).Assembly;

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly));
        services.AddAutoMapper(cfg => cfg.AddProfile<Accounting.Application.MappingProfile>());
        services.AddValidatorsFromAssembly(applicationAssembly);
        services.AddScoped<Accounting.Application.IAccountingPeriodService, Accounting.Application.AccountingPeriodService>();
        // Temporary home for the AR/AP account validators — see Accounting.Application.csproj's
        // own comment for why, and docs/modular-monolith-analysis.md §19 for when they move.
        services.AddScoped<Accounting.Application.IReceivableAccountValidator, Accounting.Application.ReceivableAccountValidator>();
        services.AddScoped<Accounting.Application.IPayableAccountValidator, Accounting.Application.PayableAccountValidator>();

        return services;
    }
}

using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Payables.Domain.Repositories;
using Payables.Infrastructure.Persistence;

namespace Payables.Infrastructure.DependencyInjection;

/// <summary>
/// Composition root entry point for the Payables module (brief §20). Host/OrgSys.Api calls this
/// once; nothing outside this module reaches into its Application/Domain/Infrastructure
/// projects directly — see docs/modular-monolith-target-architecture.md §6 for the enforced
/// dependency rules.
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPayablesModule(this IServiceCollection services)
    {
        var applicationAssembly = typeof(Payables.Application.OpeningBalance.Commands.SetSupplierOpeningBalanceCommand).Assembly;

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly));
        services.AddValidatorsFromAssembly(applicationAssembly);

        services.AddScoped<IPayableRepository, PayableRepository>();
        services.AddScoped<ISupplierPaymentApplicationRepository, SupplierPaymentApplicationRepository>();

        return services;
    }
}

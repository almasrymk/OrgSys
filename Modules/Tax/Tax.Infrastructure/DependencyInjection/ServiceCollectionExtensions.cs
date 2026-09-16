using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Tax.Application.ElectronicInvoicing;
using Tax.Infrastructure.ElectronicInvoicing;

namespace Tax.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTaxModule(this IServiceCollection services)
    {
        var applicationAssembly = typeof(Tax.Application.AssemblyMarker).Assembly;
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly));
        services.AddValidatorsFromAssembly(applicationAssembly);
        services.AddSingleton<IElectronicInvoiceAdapter, DisabledEtaElectronicInvoiceAdapter>();
        services.AddSingleton<IElectronicInvoiceAdapter, DisabledZatcaElectronicInvoiceAdapter>();
        services.AddSingleton<IElectronicInvoiceSubmitter, ElectronicInvoiceSubmitter>();
        return services;
    }
}

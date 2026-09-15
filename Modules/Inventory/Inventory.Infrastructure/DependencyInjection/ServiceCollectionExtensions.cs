using FluentValidation;
using Inventory.Application.Postings;
using Inventory.Domain.Repositories;
using Inventory.Infrastructure.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory.Infrastructure.DependencyInjection;

/// <summary>
/// Composition root entry point for the Inventory module (brief §20). See
/// docs/modular-monolith-target-architecture.md §6 for the enforced dependency rules.
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInventoryModule(this IServiceCollection services)
    {
        var applicationAssembly = typeof(Inventory.Application.MappingProfile).Assembly;

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly));
        services.AddAutoMapper(cfg => cfg.AddProfile<Inventory.Application.MappingProfile>());
        services.AddValidatorsFromAssembly(applicationAssembly);

        services.AddScoped<IInventoryBalanceRepository, InventoryBalanceRepository>();
        services.AddScoped<InventoryLedgerPoster>();
        services.AddScoped<StockReservationService>();

        return services;
    }
}

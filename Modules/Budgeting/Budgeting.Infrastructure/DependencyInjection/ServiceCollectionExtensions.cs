using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Budgeting.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBudgetingModule(this IServiceCollection services)
    {
        var applicationAssembly = typeof(Budgeting.Application.AssemblyMarker).Assembly;
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly));
        services.AddValidatorsFromAssembly(applicationAssembly);
        return services;
    }
}

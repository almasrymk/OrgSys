using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace MasterData.Infrastructure.DependencyInjection;

/// <summary>
/// Composition root entry point for the MasterData module (brief §20). Host/OrgSys.Api calls this
/// once; nothing outside this module reaches into its Application/Domain/Infrastructure
/// projects directly — see docs/modular-monolith-target-architecture.md §6 for the enforced
/// dependency rules.
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMasterDataModule(this IServiceCollection services)
    {
        // Anchor on MappingProfile (MasterData.Application), not this Infrastructure assembly —
        // the CQRS handlers/validators live in .Application, not here.
        var applicationAssembly = typeof(MasterData.Application.MappingProfile).Assembly;

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly));
        services.AddAutoMapper(cfg => cfg.AddProfile<MasterData.Application.MappingProfile>());
        services.AddValidatorsFromAssembly(applicationAssembly);

        return services;
    }
}

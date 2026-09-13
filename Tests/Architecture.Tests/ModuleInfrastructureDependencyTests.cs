using System.Reflection;
using NetArchTest.Rules;
using Xunit;

namespace Architecture.Tests;

/// <summary>
/// Enforces docs/modular-monolith-target-architecture.md's Infrastructure-layer isolation rule
/// (Infrastructure-elimination brief, Step 16 #5): one module's Infrastructure project must never
/// reference another module's Infrastructure project directly — each module owns its own
/// persistence/seeding, and cross-module needs go through Contracts (or, for the historical
/// shared OrgContext, the narrowly-scoped BuildingBlocks/OrgSys.DatabaseMigrator composition
/// project, which is not itself a module). No exceptions are accepted here by design.
/// </summary>
public class ModuleInfrastructureDependencyTests
{
    private static readonly (string Module, Assembly Infrastructure)[] ModuleInfrastructures =
    [
        ("Sales", typeof(Sales.Infrastructure.DependencyInjection.ServiceCollectionExtensions).Assembly),
        ("Parties", typeof(Parties.Infrastructure.DependencyInjection.ServiceCollectionExtensions).Assembly),
        ("CommercialDocuments", typeof(CommercialDocuments.Infrastructure.DependencyInjection.ServiceCollectionExtensions).Assembly),
        ("Purchasing", typeof(Purchasing.Infrastructure.DependencyInjection.ServiceCollectionExtensions).Assembly),
        ("Inventory", typeof(Inventory.Infrastructure.DependencyInjection.ServiceCollectionExtensions).Assembly),
        ("Treasury", typeof(Treasury.Infrastructure.DependencyInjection.ServiceCollectionExtensions).Assembly),
        ("Accounting", typeof(Accounting.Infrastructure.DependencyInjection.ServiceCollectionExtensions).Assembly),
        ("Receivables", typeof(Receivables.Infrastructure.DependencyInjection.ServiceCollectionExtensions).Assembly),
        ("Payables", typeof(Payables.Infrastructure.DependencyInjection.ServiceCollectionExtensions).Assembly),
        ("Administration", typeof(Administration.Infrastructure.DependencyInjection.ServiceCollectionExtensions).Assembly),
        ("Organization", typeof(Organization.Infrastructure.DependencyInjection.ServiceCollectionExtensions).Assembly),
        ("MasterData", typeof(MasterData.Infrastructure.DependencyInjection.ServiceCollectionExtensions).Assembly),
        ("Reporting", typeof(Reporting.Infrastructure.DependencyInjection.ServiceCollectionExtensions).Assembly),
    ];

    public static IEnumerable<object[]> AllModuleInfrastructurePairs()
    {
        foreach (var (moduleA, assemblyA) in ModuleInfrastructures)
            foreach (var (moduleB, _) in ModuleInfrastructures)
                if (moduleA != moduleB)
                    yield return [moduleA, assemblyA, moduleB];
    }

    [Theory]
    [MemberData(nameof(AllModuleInfrastructurePairs))]
    public void ModuleInfrastructure_MustNotDependOn_AnyOtherModuleInfrastructure(string moduleName, Assembly infrastructureAssembly, string otherModule)
    {
        var result = Types.InAssembly(infrastructureAssembly)
            .Should()
            .NotHaveDependencyOn($"{otherModule}.Infrastructure")
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"{moduleName}.Infrastructure must not depend on {otherModule}.Infrastructure — cross-module needs go through Contracts. " +
            $"Offending types: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }
}

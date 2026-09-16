using System.Reflection;
using NetArchTest.Rules;
using Xunit;

namespace Architecture.Tests;

/// <summary>
/// Enforces docs/dependency-rules.md — the Application-layer half of module isolation that
/// ModuleDependencyTests (Domain-to-Domain) didn't cover. See docs/architecture-review.md for
/// the audit that motivated these, and docs/module-ownership.md for what each module owns.
/// A module's Application project may depend on another module's Contracts freely (that's the
/// whole point of Contracts), but never on another module's Domain or Infrastructure directly.
/// </summary>
public class ModuleLayerDependencyTests
{
    private static readonly (string Module, Assembly Application)[] ModuleApplications =
    [
        ("Sales", typeof(Sales.Application.MappingProfile).Assembly),
        ("Parties", typeof(Parties.Application.AssemblyMarker).Assembly),
        ("CommercialDocuments", typeof(CommercialDocuments.Application.MappingProfile).Assembly),
        ("Purchasing", typeof(Purchasing.Application.AssemblyMarker).Assembly),
        ("Catalog", typeof(Catalog.Application.MappingProfile).Assembly),
        ("Inventory", typeof(Inventory.Application.MappingProfile).Assembly),
        ("Treasury", typeof(Treasury.Application.MappingProfile).Assembly),
        ("Accounting", typeof(Accounting.Application.MappingProfile).Assembly),
        ("Receivables", typeof(Receivables.Application.OpeningBalance.Commands.SetCustomerOpeningBalanceCommand).Assembly),
        ("Payables", typeof(Payables.Application.OpeningBalance.Commands.SetSupplierOpeningBalanceCommand).Assembly),
        ("Advances", typeof(Advances.Application.AssemblyMarker).Assembly),
        ("Workflow", typeof(Workflow.Application.AssemblyMarker).Assembly),
        ("Budgeting", typeof(Budgeting.Application.AssemblyMarker).Assembly),
        ("Tax", typeof(Tax.Application.AssemblyMarker).Assembly),
        ("FixedAssets", typeof(FixedAssets.Application.AssemblyMarker).Assembly),
        ("Administration", typeof(Administration.Application.MappingProfile).Assembly),
        ("Organization", typeof(Organization.Application.MappingProfile).Assembly),
        ("MasterData", typeof(MasterData.Application.MappingProfile).Assembly),
        ("Reporting", typeof(Reporting.Application.DealerBalance).Assembly),
        ("SaaS", typeof(SaaS.Application.MappingProfile).Assembly),
    ];

    private static readonly (string Module, Assembly Domain)[] ModuleDomains =
    [
        ("Sales", typeof(Sales.Domain.AssemblyMarker).Assembly),
        ("Parties", typeof(Parties.Domain.AssemblyMarker).Assembly),
        ("CommercialDocuments", typeof(CommercialDocuments.Domain.AssemblyMarker).Assembly),
        ("Purchasing", typeof(Purchasing.Domain.AssemblyMarker).Assembly),
        ("Catalog", typeof(Catalog.Domain.AssemblyMarker).Assembly),
        ("Inventory", typeof(Inventory.Domain.AssemblyMarker).Assembly),
        ("Treasury", typeof(Treasury.Domain.AssemblyMarker).Assembly),
        ("Accounting", typeof(Accounting.Domain.AssemblyMarker).Assembly),
        ("Receivables", typeof(Receivables.Domain.AssemblyMarker).Assembly),
        ("Payables", typeof(Payables.Domain.AssemblyMarker).Assembly),
        ("Advances", typeof(Advances.Domain.AssemblyMarker).Assembly),
        ("Workflow", typeof(Workflow.Domain.AssemblyMarker).Assembly),
        ("Budgeting", typeof(Budgeting.Domain.AssemblyMarker).Assembly),
        ("Tax", typeof(Tax.Domain.AssemblyMarker).Assembly),
        ("FixedAssets", typeof(FixedAssets.Domain.AssemblyMarker).Assembly),
        ("Administration", typeof(Administration.Domain.AssemblyMarker).Assembly),
        ("Organization", typeof(Organization.Domain.AssemblyMarker).Assembly),
        ("MasterData", typeof(MasterData.Domain.AssemblyMarker).Assembly),
        ("SaaS", typeof(SaaS.Domain.AssemblyMarker).Assembly),
    ];

    /// <summary>
    /// Documented, deliberate exceptions to "Application must not depend on another module's
    /// Domain" — see docs/dependency-rules.md §3. Grows only when a genuinely shared/foundational
    /// dependency can't yet go through Contracts; shrinks as later phases introduce the missing
    /// Contracts (e.g. Accounting.Contracts for the AR/AP validators).
    /// </summary>
    private static readonly (string Module, string DependsOnModule, string Reason)[] AcceptedApplicationDomainExceptions =
    [
    ];

    /// <summary>
    /// Documented exceptions to "Application must not depend on another module's Application" —
    /// see docs/dependency-rules.md §3. MasterData is treated as a safe shared dependency
    /// (reference data) rather than routed through Contracts yet. Accounting.Application is
    /// deliberately NOT in this list: every module now reaches Accounting only through
    /// Accounting.Contracts (IReceivableAccountValidator/IPayableAccountValidator/
    /// IAccountingPeriodService-equivalent queries all live in Accounting.Contracts) — see the
    /// Accounting DDD cleanup report.
    /// </summary>
    private static readonly (string Module, string DependsOnModule, string Reason)[] AcceptedApplicationApplicationExceptions =
    [
    ];

    public static IEnumerable<object[]> AllApplicationToDomainPairs()
    {
        foreach (var (moduleA, assemblyA) in ModuleApplications)
            foreach (var (moduleB, _) in ModuleDomains)
                if (moduleA != moduleB)
                    yield return [moduleA, assemblyA, moduleB];
    }

    public static IEnumerable<object[]> AllApplicationToApplicationPairs()
    {
        foreach (var (moduleA, assemblyA) in ModuleApplications)
            foreach (var (moduleB, _) in ModuleApplications)
                if (moduleA != moduleB)
                    yield return [moduleA, assemblyA, moduleB];
    }

    public static IEnumerable<object[]> AllApplications() => ModuleApplications.Select(m => new object[] { m.Module, m.Application });

    [Theory]
    [MemberData(nameof(AllApplicationToDomainPairs))]
    public void ModuleApplication_MustNotDependOn_AnyOtherModuleDomain(string moduleName, Assembly applicationAssembly, string otherModule)
    {
        if (AcceptedApplicationDomainExceptions.Any(e => e.Module == moduleName && e.DependsOnModule == otherModule))
            return; // documented exception — see AcceptedApplicationDomainExceptions / docs/dependency-rules.md

        var result = Types.InAssembly(applicationAssembly)
            .Should()
            .NotHaveDependencyOn($"{otherModule}.Domain")
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"{moduleName}.Application must not depend on {otherModule}.Domain — use {otherModule}.Contracts instead. " +
            $"Offending types: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Theory]
    [MemberData(nameof(AllApplicationToDomainPairs))]
    public void ModuleApplication_MustNotDependOn_AnyOtherModuleInfrastructure(string moduleName, Assembly applicationAssembly, string otherModule)
    {
        var result = Types.InAssembly(applicationAssembly)
            .Should()
            .NotHaveDependencyOn($"{otherModule}.Infrastructure")
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"{moduleName}.Application must not depend on {otherModule}.Infrastructure. " +
            $"Offending types: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Theory]
    [MemberData(nameof(AllApplicationToApplicationPairs))]
    public void ModuleApplication_MustNotDependOn_AnyOtherModuleApplication_UnlessAccepted(string moduleName, Assembly applicationAssembly, string otherModule)
    {
        if (AcceptedApplicationApplicationExceptions.Any(e => e.Module == moduleName && e.DependsOnModule == otherModule))
            return; // documented exception — see AcceptedApplicationApplicationExceptions / docs/dependency-rules.md

        var result = Types.InAssembly(applicationAssembly)
            .Should()
            .NotHaveDependencyOn($"{otherModule}.Application")
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"{moduleName}.Application must not depend on {otherModule}.Application — use {otherModule}.Contracts instead. " +
            $"Offending types: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Theory]
    [MemberData(nameof(AllApplications))]
    public void ModuleApplication_MustNotDependOn_OwnInfrastructure(string moduleName, Assembly applicationAssembly)
    {
        // Application must not reach "downward" into Infrastructure even within its own module —
        // Infrastructure depends on Application, never the reverse.
        var result = Types.InAssembly(applicationAssembly)
            .Should()
            .NotHaveDependencyOn($"{moduleName}.Infrastructure")
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"{moduleName}.Application must not depend on its own {moduleName}.Infrastructure. " +
            $"Offending types: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    public static IEnumerable<object[]> DomainToOtherApplicationPairs()
    {
        var applicationModuleNames = ModuleApplications.Select(a => a.Module).ToArray();
        foreach (var (domainModule, domainAssembly) in ModuleDomains)
            foreach (var applicationModule in applicationModuleNames)
                if (domainModule != applicationModule)
                    yield return [domainModule, domainAssembly, applicationModule];
    }

    [Theory]
    [MemberData(nameof(DomainToOtherApplicationPairs))]
    public void ModuleDomain_MustNotDependOn_AnyOtherModuleApplication(string moduleName, Assembly domainAssembly, string otherModule)
    {
        var result = Types.InAssembly(domainAssembly)
            .Should()
            .NotHaveDependencyOn($"{otherModule}.Application")
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"{moduleName}.Domain must not depend on {otherModule}.Application. " +
            $"Offending types: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Theory]
    [MemberData(nameof(AllApplications))]
    public void ModuleDomain_MustNotDependOn_OwnApplicationOrInfrastructure(string moduleName, Assembly _)
    {
        var domainAssembly = ModuleDomains.FirstOrDefault(d => d.Module == moduleName).Domain;
        if (domainAssembly == null)
            return; // module has no Domain of its own (e.g. Reporting) — nothing to check

        var result = Types.InAssembly(domainAssembly)
            .Should()
            .NotHaveDependencyOnAny($"{moduleName}.Application", $"{moduleName}.Infrastructure")
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"{moduleName}.Domain must not depend on its own Application/Infrastructure. " +
            $"Offending types: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }
}

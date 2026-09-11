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
        ("Purchasing", typeof(Purchasing.Application.AssemblyMarker).Assembly),
        ("Inventory", typeof(Inventory.Application.MappingProfile).Assembly),
        ("Treasury", typeof(Treasury.Application.MappingProfile).Assembly),
        ("Accounting", typeof(Accounting.Application.MappingProfile).Assembly),
        ("Receivables", typeof(Receivables.Application.OpeningBalance.Commands.SetCustomerOpeningBalanceCommand).Assembly),
        ("Payables", typeof(Payables.Application.OpeningBalance.Commands.SetSupplierOpeningBalanceCommand).Assembly),
        ("Administration", typeof(Administration.Application.MappingProfile).Assembly),
        ("Organization", typeof(Organization.Application.MappingProfile).Assembly),
        ("MasterData", typeof(MasterData.Application.MappingProfile).Assembly),
        ("Reporting", typeof(Reporting.Application.DealerBalance).Assembly),
    ];

    private static readonly (string Module, Assembly Domain)[] ModuleDomains =
    [
        ("Sales", typeof(Sales.Domain.AssemblyMarker).Assembly),
        ("Purchasing", typeof(Purchasing.Domain.AssemblyMarker).Assembly),
        ("Inventory", typeof(Inventory.Domain.AssemblyMarker).Assembly),
        ("Treasury", typeof(Treasury.Domain.AssemblyMarker).Assembly),
        ("Accounting", typeof(Accounting.Domain.AssemblyMarker).Assembly),
        ("Receivables", typeof(Receivables.Domain.AssemblyMarker).Assembly),
        ("Payables", typeof(Payables.Domain.AssemblyMarker).Assembly),
        ("Administration", typeof(Administration.Domain.AssemblyMarker).Assembly),
        ("Organization", typeof(Organization.Domain.AssemblyMarker).Assembly),
        ("MasterData", typeof(MasterData.Domain.AssemblyMarker).Assembly),
    ];

    /// <summary>
    /// Documented, deliberate exceptions to "Application must not depend on another module's
    /// Domain" — see docs/dependency-rules.md §3. Grows only when a genuinely shared/foundational
    /// dependency can't yet go through Contracts; shrinks as later phases introduce the missing
    /// Contracts (e.g. Accounting.Contracts for the AR/AP validators).
    /// </summary>
    private static readonly (string Module, string DependsOnModule, string Reason)[] AcceptedApplicationDomainExceptions =
    [
        ("Accounting", "Sales", "IReceivableAccountValidator/IPayableAccountValidator need Dealer/DealerType; see Accounting.Application.csproj comment."),
        ("Sales", "Accounting", "Invoice/Dealer Get/Search handlers populate JournalId/JournalCode and GL-account provisioning by reading Accounting.Domain.Journal/Account directly — the exact reference Phase 4 (\"Fix Sales -> Accounting integration\") replaces with Accounting.Contracts."),
        ("Sales", "MasterData", "MappingProfile Unit/UnitDto mapping support."),
        ("Inventory", "Sales", "Transaction Get/Search/Create/Delete/Update handlers and MappingProfile read Dealer/Order (the resolved Sales<->Inventory circular coupling — see Sales.Domain/Inventory.Domain accepted exception above, now also visible at the Application layer via the same navigations)."),
        ("Inventory", "Accounting", "Transaction Get/Search handlers populate JournalId/JournalCode the same way Sales' Invoice handlers do — same Phase 4-class debt, scoped to Inventory's own future accounting-integration cleanup."),
        ("Inventory", "Organization", "MappingProfile Branch/Shift mapping support."),
        ("Inventory", "MasterData", "MappingProfile Unit/Classification mapping support."),
        ("Treasury", "Sales", "Financial Create/Update/Delete/Cancel/Redo/PostTransaction handlers and MappingProfile read Dealer directly (Financial.Dealer — same navigation as the existing Treasury.Domain -> Sales.Domain exception, now also visible at the Application layer)."),
        ("Treasury", "Accounting", "FinancialTransfer/Financial Post/Reverse handlers and validators read Account/Journal directly — Phase-4-class debt for Treasury's own future accounting-integration cleanup."),
        ("Treasury", "MasterData", "MappingProfile and CreateFinancialPaidInvoiceCommandHandler read Currency directly."),
        ("Receivables", "Accounting", "SetCustomerOpeningBalanceCommandHandler reads Journal/JournalType/Currency directly (see Accounting.Application.csproj comment on why the validators stayed in Accounting)."),
        ("Receivables", "Sales", "Same handler reads DealerType."),
        ("Receivables", "MasterData", "Same handler reads Currency."),
        ("Payables", "Accounting", "SetSupplierOpeningBalanceCommandHandler reads Journal/JournalType/Currency directly."),
        ("Payables", "Sales", "Same handler reads DealerType."),
        ("Payables", "MasterData", "Same handler reads Currency."),
        ("Administration", "Organization", "MappingProfile reads Branch (User.BranchId) directly — mirrors the existing Administration.Domain -> Organization.Domain exception."),
        ("Reporting", "Sales", "Reporting is a read-only cross-module aggregator by design (no Reporting.Domain) — reads Invoice/InvoiceType directly rather than duplicating a read model."),
        ("Reporting", "Treasury", "Same reasoning — reads Financial/FinancialType directly."),
        ("Reporting", "Inventory", "Same reasoning — reads TransactionProduct/TransactionType directly."),
        ("Reporting", "MasterData", "Same reasoning — Warehouse/Financial report queries read Classification/Currency directly."),
    ];

    /// <summary>
    /// Documented exceptions to "Application must not depend on another module's Application" —
    /// see docs/dependency-rules.md §3. MasterData/Accounting are treated as safe shared
    /// dependencies (reference data, GL validators) rather than routed through Contracts yet.
    /// </summary>
    private static readonly (string Module, string DependsOnModule, string Reason)[] AcceptedApplicationApplicationExceptions =
    [
        ("Sales", "MasterData", "UnitDto mapping support."),
        ("Sales", "Accounting", "Dealer create/update GL-account provisioning (DealerPayableAccountProvisioning/DealerReceivableAccountProvisioning) uses IPayableAccountValidator/IReceivableAccountValidator — same Phase 4-class debt as the Domain-level Sales -> Accounting exception above."),
        ("Inventory", "MasterData", "UnitDto/ProductDto mapping support."),
        ("Payables", "Accounting", "IPayableAccountValidator/IAccountingPeriodService."),
        ("Payables", "Receivables", "Shared IReceivableAccountValidator clearing-account check."),
        ("Receivables", "Accounting", "IReceivableAccountValidator/IAccountingPeriodService."),
        ("Treasury", "Accounting", "IAccountingPeriodService for Financial Post/Reverse."),
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

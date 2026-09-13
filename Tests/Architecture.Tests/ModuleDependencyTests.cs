using System.Reflection;
using NetArchTest.Rules;
using Xunit;

namespace Architecture.Tests;

/// <summary>
/// Enforces docs/modular-monolith-target-architecture.md §6. Written in Phase 1, before any
/// business code has moved into the module projects (docs/modular-monolith-analysis.md §20) —
/// every assertion here passes trivially today because the Domain assemblies are still empty.
/// It starts catching real violations the moment a later phase adds the first cross-module
/// reference.
/// </summary>
public class ModuleDependencyTests
{
    private static readonly (string Module, Assembly Domain)[] ModuleDomains =
    [
        ("Sales", typeof(Sales.Domain.AssemblyMarker).Assembly),
        ("Parties", typeof(Parties.Domain.AssemblyMarker).Assembly),
        ("CommercialDocuments", typeof(CommercialDocuments.Domain.AssemblyMarker).Assembly),
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
    /// Known, deliberate exceptions to the "no cross-module Domain reference" rule, accepted as a
    /// pragmatic interim state during incremental migration (docs/modular-monolith-target-architecture.md
    /// §3 "Dealer/Invoice cross-cutting resolution", §7). Each entry must have a one-line reason.
    /// Grows only when a new entity move genuinely can't avoid a cross-module navigation property
    /// without a riskier redesign; shrinks as later phases replace these with Contracts calls.
    /// </summary>
    private static readonly (string Module, string DependsOnModule, string Reason)[] AcceptedDomainExceptions =
    [
        ("Administration", "Organization", "User.BranchId keeps its existing EF navigation to Branch rather than being redesigned to a Contracts lookup mid-migration — see analysis §13 Dealer/Country precedent."),
        ("Accounting", "MasterData", "Journal.CurrencyId keeps its existing EF navigation to Currency for the same reason."),
        ("Treasury", "MasterData", "Bank/BankBranch/FinancialAccount/Financial/FinancialTransfer keep their existing EF navigations to Country/City/District/Currency/PaymentType."),
        ("Treasury", "Organization", "CashBox/BankAccount keep their existing EF navigation to Branch."),
        ("Treasury", "Administration", "CashBox.KeeperUserId keeps its existing EF navigation to User."),
        ("Treasury", "Accounting", "CashBox/BankAccount/FinancialAccount keep their existing EF navigation to Account; Financial keeps its existing EF navigation to Journal."),
        ("Inventory", "Sales", "Transaction.Order keeps its existing EF navigation. Sales and Inventory had a genuine bidirectional coupling (Invoice.Stock/InvoiceProduct.Product on one side, Product.Dealer/Transaction.Dealer on the other); the Sales-side navigations were dropped in favor of manual batch lookups so only this one direction survives."),
        ("Inventory", "Parties", "Product.Dealer and Transaction.Dealer keep their existing EF navigations to Dealer, now owned by Parties (relocated from Sales.Domain — see docs/modular-monolith-target-architecture.md §13)."),
        ("Treasury", "CommercialDocuments", "FinancialInvoice.Invoice keeps its existing EF navigation to Invoice, now owned by CommercialDocuments (relocated from Sales.Domain)."),
        ("Treasury", "Parties", "Financial.Dealer keeps its existing EF navigation to Dealer, now owned by Parties (relocated from Sales.Domain)."),
        ("Sales", "Organization", "Order/Dealer keep their existing EF navigations to Branch/Shift/CompanyProfile."),
        ("Sales", "MasterData", "OrderProduct.Unit keeps its existing EF navigation to Unit."),
        ("Sales", "Parties", "Order.Dealer keeps its existing EF navigation to Dealer, now owned by Parties (relocated from this project)."),
        ("Sales", "CommercialDocuments", "Order.Invoice keeps its existing EF navigation to Invoice, now owned by CommercialDocuments (relocated from this project)."),
        ("CommercialDocuments", "MasterData", "Invoice keeps its existing EF navigations to Currency/PaymentType."),
        ("CommercialDocuments", "Parties", "Invoice.Dealer keeps its existing EF navigation to Dealer, now owned by Parties (relocated from Sales.Domain along with Invoice itself)."),
        ("Parties", "MasterData", "Dealer keeps its existing EF navigations to Country/City/District."),
        ("Parties", "Accounting", "Dealer.AccountId keeps its existing EF navigation to Account (GL account link)."),
        ("Inventory", "Accounting", "Product/Transaction keep their existing EF navigation to Account."),
        ("Inventory", "Organization", "Stock/Transaction/Inventory keep their existing EF navigations to Branch/Shift."),
        ("Inventory", "MasterData", "Product/ProductUnit/Stock/Transaction keep their existing EF navigations to Unit/Classification/Country/City/District/Currency."),
        ("Purchasing", "Parties", "PurchaseOrder.Dealer (the supplier) keeps an EF navigation to Parties.Domain.Dealer, same convention as every other module's Dealer reference."),
        ("Purchasing", "MasterData", "PurchaseRequisitionProduct.Unit / PurchaseOrderProduct.Unit keep an EF navigation to MasterData.Unit, same convention as CommercialDocuments.Domain.InvoiceProduct/Sales.Domain.OrderProduct."),
    ];

    public static IEnumerable<object[]> AllModulePairs()
    {
        foreach (var (moduleA, assemblyA) in ModuleDomains)
            foreach (var (moduleB, _) in ModuleDomains)
                if (moduleA != moduleB)
                    yield return [moduleA, assemblyA, moduleB];
    }

    [Theory]
    [MemberData(nameof(AllModulePairs))]
    public void ModuleDomain_MustNotDependOn_AnyOtherModuleDomain(string moduleName, Assembly domainAssembly, string otherModule)
    {
        if (AcceptedDomainExceptions.Any(e => e.Module == moduleName && e.DependsOnModule == otherModule))
            return; // documented exception — see AcceptedDomainExceptions

        // General rule behind brief §29's specific examples (Sales.Domain -> Inventory.Domain,
        // Sales.Domain -> Treasury.Domain, Inventory.Domain -> Accounting.Domain,
        // Treasury.Domain -> Accounting.Domain): no Module.Domain may reference any other
        // module's Domain namespace at all, not just the four the brief calls out by name.
        var result = Types.InAssembly(domainAssembly)
            .Should()
            .NotHaveDependencyOn($"{otherModule}.Domain")
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"{moduleName}.Domain must not depend on {otherModule}.Domain. " +
            $"Offending types: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Theory]
    [MemberData(nameof(AllModulePairs))]
    public void ModuleDomain_MustNotDependOn_AnyOtherModuleInfrastructure(string moduleName, Assembly domainAssembly, string otherModule)
    {
        var result = Types.InAssembly(domainAssembly)
            .Should()
            .NotHaveDependencyOn($"{otherModule}.Infrastructure")
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"{moduleName}.Domain must not depend on {otherModule}.Infrastructure. " +
            $"Offending types: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Fact]
    public void ModuleDomains_MustNotDependOn_MediatR_Or_EntityFrameworkCore()
    {
        // Domain stays framework-agnostic; CQRS/persistence concerns belong in Application/Infrastructure.
        foreach (var (moduleName, assembly) in ModuleDomains)
        {
            var result = Types.InAssembly(assembly)
                .Should()
                .NotHaveDependencyOnAny("MediatR", "Microsoft.EntityFrameworkCore")
                .GetResult();

            Assert.True(result.IsSuccessful,
                $"{moduleName}.Domain must stay framework-agnostic (no MediatR/EF Core references). " +
                $"Offending types: {string.Join(", ", result.FailingTypeNames ?? [])}");
        }
    }
}

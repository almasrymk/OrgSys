using NetArchTest.Rules;
using Xunit;

namespace Architecture.Tests;

/// <summary>
/// Named, explicit invariants for the Invoice ownership move out of Sales.Domain and into
/// CommercialDocuments.Domain (docs/commercial-documents-module.md). These overlap with the
/// generic parameterized checks in ModuleDependencyTests/ModuleLayerDependencyTests, but are kept
/// as standalone facts so a broken invariant here fails with an unambiguous, specific message
/// rather than one row out of a large parameterized matrix.
/// </summary>
public class CommercialDocumentsOwnershipTests
{
    [Fact]
    public void PurchasingApplication_MustNotDependOn_SalesDomain()
    {
        var result = Types.InAssembly(typeof(Purchasing.Application.AssemblyMarker).Assembly)
            .Should()
            .NotHaveDependencyOn("Sales.Domain")
            .GetResult();

        Assert.True(result.IsSuccessful,
            "Purchasing.Application must not depend on Sales.Domain — Invoice access must go through " +
            $"CommercialDocuments.Contracts. Offending types: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Fact]
    public void CommercialDocumentsDomain_MustNotDependOn_SalesDomain()
    {
        var result = Types.InAssembly(typeof(CommercialDocuments.Domain.AssemblyMarker).Assembly)
            .Should()
            .NotHaveDependencyOn("Sales.Domain")
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"CommercialDocuments.Domain must not depend on Sales.Domain. Offending types: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Fact]
    public void CommercialDocumentsDomain_MustNotDependOn_PurchasingDomain()
    {
        var result = Types.InAssembly(typeof(CommercialDocuments.Domain.AssemblyMarker).Assembly)
            .Should()
            .NotHaveDependencyOn("Purchasing.Domain")
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"CommercialDocuments.Domain must not depend on Purchasing.Domain. Offending types: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Fact]
    public void CommercialDocumentsDomain_MustNotDependOn_AccountingDomain()
    {
        var result = Types.InAssembly(typeof(CommercialDocuments.Domain.AssemblyMarker).Assembly)
            .Should()
            .NotHaveDependencyOn("Accounting.Domain")
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"CommercialDocuments.Domain must not depend on Accounting.Domain. Offending types: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Fact]
    public void CommercialDocumentsDomain_MustNotDependOn_InventoryDomain()
    {
        var result = Types.InAssembly(typeof(CommercialDocuments.Domain.AssemblyMarker).Assembly)
            .Should()
            .NotHaveDependencyOn("Inventory.Domain")
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"CommercialDocuments.Domain must not depend on Inventory.Domain. Offending types: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Fact]
    public void SalesDomain_MustNotOwnInvoice()
    {
        var hasInvoiceType = typeof(Sales.Domain.AssemblyMarker).Assembly.GetTypes()
            .Any(t => t.Name is "Invoice" or "InvoiceProduct" or "InvoiceType");

        Assert.False(hasInvoiceType, "Sales.Domain must not own Invoice/InvoiceProduct/InvoiceType — CommercialDocuments.Domain is the authoritative owner.");
    }

    [Fact]
    public void PurchasingDomain_MustNotOwnADuplicateInvoice()
    {
        var hasInvoiceType = typeof(Purchasing.Domain.AssemblyMarker).Assembly.GetTypes()
            .Any(t => t.Name is "Invoice" or "InvoiceProduct" or "InvoiceType");

        Assert.False(hasInvoiceType, "Purchasing.Domain must not own a duplicate Invoice/InvoiceProduct/InvoiceType — there is exactly one Invoice model, owned by CommercialDocuments.Domain.");
    }

    [Fact]
    public void CommercialDocumentsDomain_OwnsInvoice()
    {
        var assembly = typeof(CommercialDocuments.Domain.AssemblyMarker).Assembly;
        var hasInvoice = assembly.GetTypes().Any(t => t.Name == "Invoice");
        var hasInvoiceProduct = assembly.GetTypes().Any(t => t.Name == "InvoiceProduct");
        var hasInvoiceType = assembly.GetTypes().Any(t => t.Name == "InvoiceType");

        Assert.True(hasInvoice && hasInvoiceProduct && hasInvoiceType,
            "CommercialDocuments.Domain must own Invoice, InvoiceProduct and InvoiceType.");
    }
}

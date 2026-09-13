using CommercialDocuments.Application.Invoices.Queries;
using CommercialDocuments.Contracts.Invoices;
using CommercialDocuments.Domain;
using Moq;
using OrgSys.SharedKernel;
using System.Net;
using Xunit;

namespace Application.Tests;

public class GetInvoiceReferenceQueryHandlerTests
{
    [Fact]
    public async Task Handle_ExistingInvoice_ReturnsReference()
    {
        var invoice = new Invoice { Id = 10, Code = "INV-10", TypeId = 2, DealerId = 5, Status = Status.New };
        var repository = new Mock<IRepository<Invoice>>();
        repository.Setup(r => r.GetByFilterAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Invoice, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync(invoice);

        var handler = new GetInvoiceReferenceQueryHandler(repository.Object);

        var result = await handler.Handle(new GetInvoiceReferenceQuery(10), CancellationToken.None);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.NotNull(result.Response);
        Assert.Equal(InvoiceTypeId.Purchase, result.Response!.TypeId);
        Assert.False(result.Response.IsDeleted);
    }

    [Fact]
    public async Task Handle_MissingInvoice_ReturnsNullReference()
    {
        var repository = new Mock<IRepository<Invoice>>();
        repository.Setup(r => r.GetByFilterAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Invoice, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync((Invoice?)null);

        var handler = new GetInvoiceReferenceQueryHandler(repository.Object);

        var result = await handler.Handle(new GetInvoiceReferenceQuery(999), CancellationToken.None);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.Null(result.Response);
    }
}

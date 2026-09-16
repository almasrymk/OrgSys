using CommercialDocuments.Contracts.IntegrationEvents;
using Moq;
using OrgSys.SharedKernel;
using Receivables.Application.Invoices.Integration;
using Receivables.Domain;
using Receivables.Domain.Repositories;
using Xunit;

namespace Application.Tests;

public class SalesInvoicePostedIntegrationEventHandlerTests
{
    private static SalesInvoicePostedIntegrationEvent Event(long invoiceId = 100, decimal amount = 1000) => new(
        InvoiceId: invoiceId,
        InvoiceNumber: "INV-100",
        CustomerId: 1,
        InvoiceDate: new DateTime(2026, 9, 1),
        DueDate: new DateTime(2026, 9, 1),
        CurrencyId: 1,
        Rate: 1,
        Amount: amount,
        CreateUserId: 1,
        CreateDate: new DateTime(2026, 9, 1),
        BranchId: null);

    private static (SalesInvoicePostedIntegrationEventHandler handler, Mock<IReceivableRepository> repository, Mock<IUnitOfWork> unitOfWork) BuildHandler(bool alreadyExists = false)
    {
        var repository = new Mock<IReceivableRepository>();
        repository.Setup(r => r.ExistsForSourceDocumentAsync(SourceDocumentType.SalesInvoice, It.IsAny<long>(), It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(alreadyExists);

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.SaveChangeAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var handler = new SalesInvoicePostedIntegrationEventHandler(repository.Object, unitOfWork.Object, InboxTestDoubles.AlwaysClaim());
        return (handler, repository, unitOfWork);
    }

    [Fact]
    public async Task Handle_NewInvoice_CreatesReceivable()
    {
        var (handler, repository, unitOfWork) = BuildHandler();
        Receivable? added = null;
        repository.Setup(r => r.AddAsync(It.IsAny<Receivable>(), It.IsAny<CancellationToken>()))
            .Callback<Receivable, CancellationToken>((r, _) => added = r)
            .Returns(Task.CompletedTask);

        await handler.Handle(Event(invoiceId: 100, amount: 1000), CancellationToken.None);

        Assert.NotNull(added);
        Assert.Equal(1000, added!.OriginalAmount);
        Assert.Equal(1000, added.OutstandingAmount);
        Assert.Equal(SourceDocumentType.SalesInvoice, added.SourceDocumentType);
        Assert.Equal(100, added.SourceDocumentId);
        unitOfWork.Verify(u => u.SaveChangeAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_DuplicateDelivery_DoesNotCreateASecondReceivable()
    {
        var (handler, repository, unitOfWork) = BuildHandler(alreadyExists: true);

        await handler.Handle(Event(invoiceId: 100), CancellationToken.None);

        repository.Verify(r => r.AddAsync(It.IsAny<Receivable>(), It.IsAny<CancellationToken>()), Times.Never);
        unitOfWork.Verify(u => u.SaveChangeAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ZeroNetInvoice_DoesNotCreateAReceivable()
    {
        var (handler, repository, unitOfWork) = BuildHandler();

        await handler.Handle(Event(amount: 0), CancellationToken.None);

        repository.Verify(r => r.AddAsync(It.IsAny<Receivable>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_DuplicateEventId_DoesNotCreateReceivable()
    {
        var repository = new Mock<IReceivableRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();
        var handler = new SalesInvoicePostedIntegrationEventHandler(repository.Object, unitOfWork.Object, InboxTestDoubles.AlreadyClaimed());

        await handler.Handle(Event(), CancellationToken.None);

        repository.Verify(r => r.AddAsync(It.IsAny<Receivable>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}

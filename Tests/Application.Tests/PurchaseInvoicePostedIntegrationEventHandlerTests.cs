using CommercialDocuments.Contracts.IntegrationEvents;
using Moq;
using Payables.Application.Invoices.Integration;
using Payables.Domain;
using Payables.Domain.Repositories;
using Xunit;

namespace Application.Tests;

public class PurchaseInvoicePostedIntegrationEventHandlerTests
{
    private static PurchaseInvoicePostedIntegrationEvent Event(long invoiceId = 100, decimal amount = 1000) => new(
        InvoiceId: invoiceId,
        InvoiceNumber: "PINV-100",
        SupplierId: 1,
        InvoiceDate: new DateTime(2026, 9, 1),
        DueDate: new DateTime(2026, 9, 1),
        CurrencyId: 1,
        Rate: 1,
        Amount: amount,
        CreateUserId: 1,
        CreateDate: new DateTime(2026, 9, 1),
        BranchId: null);

    private static (PurchaseInvoicePostedIntegrationEventHandler handler, Mock<IPayableRepository> repository, Mock<IUnitOfWork> unitOfWork) BuildHandler(bool alreadyExists = false)
    {
        var repository = new Mock<IPayableRepository>();
        repository.Setup(r => r.ExistsForSourceDocumentAsync(SourceDocumentType.PurchaseInvoice, It.IsAny<long>(), It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(alreadyExists);

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.SaveChangeAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var handler = new PurchaseInvoicePostedIntegrationEventHandler(repository.Object, unitOfWork.Object, InboxTestDoubles.AlwaysClaim());
        return (handler, repository, unitOfWork);
    }

    [Fact]
    public async Task Handle_NewInvoice_CreatesPayable()
    {
        var (handler, repository, unitOfWork) = BuildHandler();
        Payable? added = null;
        repository.Setup(r => r.AddAsync(It.IsAny<Payable>(), It.IsAny<CancellationToken>()))
            .Callback<Payable, CancellationToken>((p, _) => added = p)
            .Returns(Task.CompletedTask);

        await handler.Handle(Event(invoiceId: 100, amount: 1000), CancellationToken.None);

        Assert.NotNull(added);
        Assert.Equal(1000, added!.OriginalAmount);
        Assert.Equal(1000, added.OutstandingAmount);
        Assert.Equal(SourceDocumentType.PurchaseInvoice, added.SourceDocumentType);
        Assert.Equal(100, added.SourceDocumentId);
        unitOfWork.Verify(u => u.SaveChangeAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_DuplicateDelivery_DoesNotCreateASecondPayable()
    {
        var (handler, repository, unitOfWork) = BuildHandler(alreadyExists: true);

        await handler.Handle(Event(invoiceId: 100), CancellationToken.None);

        repository.Verify(r => r.AddAsync(It.IsAny<Payable>(), It.IsAny<CancellationToken>()), Times.Never);
        unitOfWork.Verify(u => u.SaveChangeAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ZeroNetInvoice_DoesNotCreateAPayable()
    {
        var (handler, repository, unitOfWork) = BuildHandler();

        await handler.Handle(Event(amount: 0), CancellationToken.None);

        repository.Verify(r => r.AddAsync(It.IsAny<Payable>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}

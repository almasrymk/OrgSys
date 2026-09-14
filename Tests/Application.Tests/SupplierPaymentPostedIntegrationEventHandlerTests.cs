using Moq;
using Payables.Application.Payments.Integration;
using Payables.Domain;
using Payables.Domain.Repositories;
using Treasury.Contracts.IntegrationEvents;
using Xunit;

namespace Application.Tests;

public class SupplierPaymentPostedIntegrationEventHandlerTests
{
    private static readonly DateTime PaymentDate = new(2026, 9, 10);

    private static SupplierPaymentPostedIntegrationEvent Event(long financialId = 500, decimal amount = 1000) => new(
        FinancialId: financialId,
        SupplierId: 1,
        Amount: amount,
        CurrencyId: 1,
        Rate: 1,
        PaymentDate: PaymentDate,
        CreateUserId: 1,
        CreateDate: PaymentDate,
        BranchId: null);

    private static Payable OpenPayable(long id, decimal originalAmount, DateTime documentDate)
    {
        var payable = Payable.Create(
            supplierId: 1, sourceDocumentType: SourceDocumentType.PurchaseInvoice, sourceDocumentId: id,
            sourceDocumentNumber: $"PINV-{id}", documentDate: documentDate, dueDate: documentDate,
            currencyId: 1, rate: 1, originalAmount: originalAmount, createUserId: 1, createDate: documentDate);
        payable.Id = id;
        return payable;
    }

    private static (SupplierPaymentPostedIntegrationEventHandler handler, Mock<IPayableRepository> payableRepository, Mock<ISupplierPaymentApplicationRepository> paymentApplicationRepository, Mock<IUnitOfWork> unitOfWork) BuildHandler(
        IReadOnlyList<Payable> openPayables, bool alreadyApplied = false)
    {
        var payableRepository = new Mock<IPayableRepository>();
        payableRepository.Setup(r => r.GetOpenBySupplierAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(openPayables);

        var paymentApplicationRepository = new Mock<ISupplierPaymentApplicationRepository>();
        paymentApplicationRepository.Setup(r => r.ExistsForSourceFinancialAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(alreadyApplied);

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.SaveChangeAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var handler = new SupplierPaymentPostedIntegrationEventHandler(payableRepository.Object, paymentApplicationRepository.Object, unitOfWork.Object);
        return (handler, payableRepository, paymentApplicationRepository, unitOfWork);
    }

    [Fact]
    public async Task Handle_PaymentSmallerThanOldestPayable_PartiallySettlesOldestOnly()
    {
        var oldest = OpenPayable(1, 1000, new DateTime(2026, 8, 1));
        var newest = OpenPayable(2, 500, new DateTime(2026, 9, 1));
        var (handler, _, paymentApplicationRepository, unitOfWork) = BuildHandler([oldest, newest]);

        await handler.Handle(Event(amount: 400), CancellationToken.None);

        Assert.Equal(600, oldest.OutstandingAmount);
        Assert.Equal(500, newest.OutstandingAmount);
        paymentApplicationRepository.Verify(r => r.AddAsync(It.IsAny<SupplierPaymentApplication>(), It.IsAny<CancellationToken>()), Times.Once);
        unitOfWork.Verify(u => u.SaveChangeAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_PaymentSpansMultiplePayables_AppliesFifoOldestFirst()
    {
        var oldest = OpenPayable(1, 400, new DateTime(2026, 8, 1));
        var newest = OpenPayable(2, 500, new DateTime(2026, 9, 1));
        var (handler, _, _, _) = BuildHandler([oldest, newest]);

        await handler.Handle(Event(amount: 700), CancellationToken.None);

        Assert.Equal(0, oldest.OutstandingAmount);
        Assert.Equal(200, newest.OutstandingAmount);
    }

    [Fact]
    public async Task Handle_PaymentExceedsAllOutstanding_LeavesRemainderUnapplied()
    {
        var only = OpenPayable(1, 400, new DateTime(2026, 8, 1));
        var (handler, _, paymentApplicationRepository, _) = BuildHandler([only]);
        SupplierPaymentApplication? captured = null;
        paymentApplicationRepository.Setup(r => r.AddAsync(It.IsAny<SupplierPaymentApplication>(), It.IsAny<CancellationToken>()))
            .Callback<SupplierPaymentApplication, CancellationToken>((p, _) => captured = p)
            .Returns(Task.CompletedTask);

        await handler.Handle(Event(amount: 1000), CancellationToken.None);

        Assert.Equal(0, only.OutstandingAmount);
        Assert.NotNull(captured);
        Assert.Equal(600, captured!.UnappliedAmount);
    }

    [Fact]
    public async Task Handle_NoOpenPayables_LeavesEntirePaymentUnapplied()
    {
        var (handler, _, paymentApplicationRepository, unitOfWork) = BuildHandler([]);
        SupplierPaymentApplication? captured = null;
        paymentApplicationRepository.Setup(r => r.AddAsync(It.IsAny<SupplierPaymentApplication>(), It.IsAny<CancellationToken>()))
            .Callback<SupplierPaymentApplication, CancellationToken>((p, _) => captured = p)
            .Returns(Task.CompletedTask);

        await handler.Handle(Event(amount: 1000), CancellationToken.None);

        Assert.NotNull(captured);
        Assert.Equal(1000, captured!.UnappliedAmount);
        Assert.Empty(captured.Lines);
        unitOfWork.Verify(u => u.SaveChangeAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_DuplicateDelivery_DoesNotApplyTwice()
    {
        var only = OpenPayable(1, 400, new DateTime(2026, 8, 1));
        var (handler, _, paymentApplicationRepository, unitOfWork) = BuildHandler([only], alreadyApplied: true);

        await handler.Handle(Event(amount: 400), CancellationToken.None);

        Assert.Equal(400, only.OutstandingAmount);
        paymentApplicationRepository.Verify(r => r.AddAsync(It.IsAny<SupplierPaymentApplication>(), It.IsAny<CancellationToken>()), Times.Never);
        unitOfWork.Verify(u => u.SaveChangeAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}

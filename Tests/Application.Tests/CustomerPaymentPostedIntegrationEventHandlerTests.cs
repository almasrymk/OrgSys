using Moq;
using Receivables.Application.Payments.Integration;
using Receivables.Domain;
using Receivables.Domain.Repositories;
using Treasury.Contracts.IntegrationEvents;
using Xunit;

namespace Application.Tests;

public class CustomerPaymentPostedIntegrationEventHandlerTests
{
    private static readonly DateTime PaymentDate = new(2026, 9, 10);

    private static CustomerPaymentPostedIntegrationEvent Event(long financialId = 500, decimal amount = 1000) => new(
        FinancialId: financialId,
        CustomerId: 1,
        Amount: amount,
        CurrencyId: 1,
        Rate: 1,
        PaymentDate: PaymentDate,
        CreateUserId: 1,
        CreateDate: PaymentDate,
        BranchId: null);

    private static Receivable OpenReceivable(long id, decimal originalAmount, DateTime documentDate)
    {
        var receivable = Receivable.Create(
            customerId: 1, sourceDocumentType: SourceDocumentType.SalesInvoice, sourceDocumentId: id,
            sourceDocumentNumber: $"INV-{id}", documentDate: documentDate, dueDate: documentDate,
            currencyId: 1, rate: 1, originalAmount: originalAmount, createUserId: 1, createDate: documentDate);
        receivable.Id = id;
        return receivable;
    }

    private static (CustomerPaymentPostedIntegrationEventHandler handler, Mock<IReceivableRepository> receivableRepository, Mock<IPaymentApplicationRepository> paymentApplicationRepository, Mock<IUnitOfWork> unitOfWork) BuildHandler(
        IReadOnlyList<Receivable> openReceivables, bool alreadyApplied = false)
    {
        var receivableRepository = new Mock<IReceivableRepository>();
        receivableRepository.Setup(r => r.GetOpenByCustomerAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(openReceivables);

        var paymentApplicationRepository = new Mock<IPaymentApplicationRepository>();
        paymentApplicationRepository.Setup(r => r.ExistsForSourceFinancialAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(alreadyApplied);

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.SaveChangeAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var handler = new CustomerPaymentPostedIntegrationEventHandler(receivableRepository.Object, paymentApplicationRepository.Object, unitOfWork.Object, InboxTestDoubles.AlwaysClaim());
        return (handler, receivableRepository, paymentApplicationRepository, unitOfWork);
    }

    [Fact]
    public async Task Handle_PaymentSmallerThanOldestReceivable_PartiallySettlesOldestOnly()
    {
        var oldest = OpenReceivable(1, 1000, new DateTime(2026, 8, 1));
        var newest = OpenReceivable(2, 500, new DateTime(2026, 9, 1));
        var (handler, _, paymentApplicationRepository, unitOfWork) = BuildHandler([oldest, newest]);

        await handler.Handle(Event(amount: 400), CancellationToken.None);

        Assert.Equal(600, oldest.OutstandingAmount);
        Assert.Equal(500, newest.OutstandingAmount);
        paymentApplicationRepository.Verify(r => r.AddAsync(It.IsAny<PaymentApplication>(), It.IsAny<CancellationToken>()), Times.Once);
        unitOfWork.Verify(u => u.SaveChangeAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_PaymentSpansMultipleReceivables_AppliesFifoOldestFirst()
    {
        var oldest = OpenReceivable(1, 400, new DateTime(2026, 8, 1));
        var newest = OpenReceivable(2, 500, new DateTime(2026, 9, 1));
        var (handler, _, _, _) = BuildHandler([oldest, newest]);

        await handler.Handle(Event(amount: 700), CancellationToken.None);

        Assert.Equal(0, oldest.OutstandingAmount);
        Assert.Equal(200, newest.OutstandingAmount);
    }

    [Fact]
    public async Task Handle_PaymentExceedsAllOutstanding_LeavesRemainderUnapplied()
    {
        var only = OpenReceivable(1, 400, new DateTime(2026, 8, 1));
        var (handler, _, paymentApplicationRepository, _) = BuildHandler([only]);
        PaymentApplication? captured = null;
        paymentApplicationRepository.Setup(r => r.AddAsync(It.IsAny<PaymentApplication>(), It.IsAny<CancellationToken>()))
            .Callback<PaymentApplication, CancellationToken>((p, _) => captured = p)
            .Returns(Task.CompletedTask);

        await handler.Handle(Event(amount: 1000), CancellationToken.None);

        Assert.Equal(0, only.OutstandingAmount);
        Assert.NotNull(captured);
        Assert.Equal(600, captured!.UnappliedAmount);
    }

    [Fact]
    public async Task Handle_NoOpenReceivables_LeavesEntirePaymentUnapplied()
    {
        var (handler, _, paymentApplicationRepository, unitOfWork) = BuildHandler([]);
        PaymentApplication? captured = null;
        paymentApplicationRepository.Setup(r => r.AddAsync(It.IsAny<PaymentApplication>(), It.IsAny<CancellationToken>()))
            .Callback<PaymentApplication, CancellationToken>((p, _) => captured = p)
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
        var only = OpenReceivable(1, 400, new DateTime(2026, 8, 1));
        var (handler, _, paymentApplicationRepository, unitOfWork) = BuildHandler([only], alreadyApplied: true);

        await handler.Handle(Event(amount: 400), CancellationToken.None);

        Assert.Equal(400, only.OutstandingAmount);
        paymentApplicationRepository.Verify(r => r.AddAsync(It.IsAny<PaymentApplication>(), It.IsAny<CancellationToken>()), Times.Never);
        unitOfWork.Verify(u => u.SaveChangeAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}

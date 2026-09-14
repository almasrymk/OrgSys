namespace Receivables.Application.Payments.Integration;

using MediatR;
using OrgSys.SharedKernel;
using Receivables.Domain;
using Receivables.Domain.Repositories;
using Treasury.Contracts.IntegrationEvents;

/// <summary>
/// FIFO-applies a posted customer receipt against the customer's open Receivables (oldest
/// DocumentDate first) — see docs/architecture/receivables-ddd-migration.md §9/§12 (Phase 6). Never
/// re-posts to the General Ledger; that already happened in Treasury before this event was raised.
/// Runs in the same transaction as the triggering Financial posting (see PostTransactionCommandHandler
/// in Treasury.Application — the publish call happens before that handler's own commit).
/// </summary>
public sealed class CustomerPaymentPostedIntegrationEventHandler(
    IReceivableRepository receivableRepository,
    IPaymentApplicationRepository paymentApplicationRepository,
    IUnitOfWork unitOfWork) : INotificationHandler<CustomerPaymentPostedIntegrationEvent>
{
    public async Task Handle(CustomerPaymentPostedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        // Duplicate delivery of the same posted receipt must apply exactly once (brief §32) — the DB
        // unique index on PaymentApplication.SourceFinancialId is the final safety net.
        if (await paymentApplicationRepository.ExistsForSourceFinancialAsync(notification.FinancialId, cancellationToken))
            return;

        if (notification.Amount <= 0)
            return;

        var application = PaymentApplication.Create(
            notification.FinancialId, notification.CustomerId, notification.Amount, notification.CreateUserId, notification.CreateDate, notification.BranchId);

        var openReceivables = await receivableRepository.GetOpenByCustomerAsync(notification.CustomerId, cancellationToken);

        var remaining = notification.Amount;
        foreach (var receivable in openReceivables)
        {
            if (remaining <= 0)
                break;

            var toApply = Math.Min(remaining, receivable.OutstandingAmount);
            receivable.Apply(toApply);
            application.RecordLine(receivable.Id, toApply);
            remaining -= toApply;
        }

        // Any remainder (no open items, or payment exceeds total outstanding) stays on
        // application.UnappliedAmount — see PaymentApplication's own doc comment on why this pass
        // does not track it as a separate on-account credit ledger.
        await paymentApplicationRepository.AddAsync(application, cancellationToken);
        await unitOfWork.SaveChangeAsync(cancellationToken);
    }
}

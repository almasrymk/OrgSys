namespace Payables.Application.Payments.Integration;

using MediatR;
using OrgSys.SharedKernel;
using Payables.Domain;
using Payables.Domain.Repositories;
using Treasury.Contracts.IntegrationEvents;

/// <summary>
/// FIFO-applies a posted supplier payment against the supplier's open Payables (oldest
/// DocumentDate first) — mirrors
/// Receivables.Application.Payments.Integration.CustomerPaymentPostedIntegrationEventHandler.
/// Never re-posts to the General Ledger; that already happened in Treasury before this event was
/// raised. Runs in the same transaction as the triggering Financial posting (see
/// PostTransactionCommandHandler in Treasury.Application — the publish call happens before that
/// handler's own commit).
/// </summary>
public sealed class SupplierPaymentPostedIntegrationEventHandler(
    IPayableRepository payableRepository,
    ISupplierPaymentApplicationRepository paymentApplicationRepository,
    IUnitOfWork unitOfWork,
    IInboxStore inbox) : INotificationHandler<SupplierPaymentPostedIntegrationEvent>
{
    public async Task Handle(SupplierPaymentPostedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        if (!await inbox.TryClaimAsync(notification.EventId, nameof(SupplierPaymentPostedIntegrationEventHandler), cancellationToken))
            return;
        // Duplicate delivery of the same posted payment must apply exactly once — the DB unique
        // index on SupplierPaymentApplication.SourceFinancialId is the final safety net.
        if (await paymentApplicationRepository.ExistsForSourceFinancialAsync(notification.FinancialId, cancellationToken))
            return;

        if (notification.Amount <= 0)
            return;

        var application = SupplierPaymentApplication.Create(
            notification.FinancialId, notification.SupplierId, notification.Amount, notification.CreateUserId, notification.CreateDate, notification.BranchId);

        var openPayables = await payableRepository.GetOpenBySupplierAsync(notification.SupplierId, cancellationToken);

        var remaining = notification.Amount;
        foreach (var payable in openPayables)
        {
            if (remaining <= 0)
                break;

            var toApply = Math.Min(remaining, payable.OutstandingAmount);
            payable.Apply(toApply);
            application.RecordLine(payable.Id, toApply);
            remaining -= toApply;
        }

        // Any remainder (no open items, or payment exceeds total outstanding) stays on
        // application.UnappliedAmount — see SupplierPaymentApplication's own doc comment on why this
        // pass does not track it as a separate supplier-advance ledger.
        await paymentApplicationRepository.AddAsync(application, cancellationToken);
        await unitOfWork.SaveChangeAsync(cancellationToken);
    }
}

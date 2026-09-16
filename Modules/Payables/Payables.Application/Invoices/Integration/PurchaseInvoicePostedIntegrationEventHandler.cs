namespace Payables.Application.Invoices.Integration;

using CommercialDocuments.Contracts.IntegrationEvents;
using MediatR;
using OrgSys.SharedKernel;
using Payables.Domain;
using Payables.Domain.Repositories;

/// <summary>
/// Idempotently creates the AP open item for a posted Purchase Invoice — mirrors
/// Receivables.Application.Invoices.Integration.SalesInvoicePostedIntegrationEventHandler. Never
/// re-posts to the General Ledger; that already happened in CommercialDocuments before this event
/// was raised. Runs in the same transaction as the triggering Invoice creation (see
/// CreateCommandHandler in CommercialDocuments.Application — the publish call happens before that
/// handler's own commit).
/// </summary>
public sealed class PurchaseInvoicePostedIntegrationEventHandler(
    IPayableRepository payableRepository,
    IUnitOfWork unitOfWork,
    IInboxStore inbox) : INotificationHandler<PurchaseInvoicePostedIntegrationEvent>
{
    public async Task Handle(PurchaseInvoicePostedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        if (!await inbox.TryClaimAsync(notification.EventId, nameof(PurchaseInvoicePostedIntegrationEventHandler), cancellationToken))
            return;
        // Duplicate delivery of the same posted invoice must create exactly one Payable — the DB
        // unique index on (SourceDocumentType, SourceDocumentId, SupplierId) is the final safety
        // net; this check is the fast path that avoids hitting it in the common case.
        if (await payableRepository.ExistsForSourceDocumentAsync(SourceDocumentType.PurchaseInvoice, notification.InvoiceId, notification.SupplierId, cancellationToken))
            return;

        // A fully-discounted (net-zero) invoice creates no AP obligation — Payable.Create requires
        // OriginalAmount > 0, and there is nothing to settle either way.
        if (notification.Amount <= 0)
            return;

        var payable = Payable.Create(
            supplierId: notification.SupplierId,
            sourceDocumentType: SourceDocumentType.PurchaseInvoice,
            sourceDocumentId: notification.InvoiceId,
            sourceDocumentNumber: notification.InvoiceNumber,
            documentDate: notification.InvoiceDate,
            dueDate: notification.DueDate,
            currencyId: notification.CurrencyId,
            rate: notification.Rate,
            originalAmount: notification.Amount,
            createUserId: notification.CreateUserId,
            createDate: notification.CreateDate,
            branchId: notification.BranchId);

        await payableRepository.AddAsync(payable, cancellationToken);
        await unitOfWork.SaveChangeAsync(cancellationToken);
    }
}

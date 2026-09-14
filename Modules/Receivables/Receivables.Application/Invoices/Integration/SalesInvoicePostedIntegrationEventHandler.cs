namespace Receivables.Application.Invoices.Integration;

using CommercialDocuments.Contracts.IntegrationEvents;
using MediatR;
using OrgSys.SharedKernel;
using Receivables.Domain;
using Receivables.Domain.Repositories;

/// <summary>
/// Idempotently creates the AR open item for a posted Sales Invoice — see
/// docs/architecture/receivables-ddd-migration.md §9. Never re-posts to the General Ledger; that
/// already happened in CommercialDocuments before this event was raised. Runs in the same
/// transaction as the triggering Invoice creation (see CreateCommandHandler in
/// CommercialDocuments.Application — the publish call happens before that handler's own commit).
/// </summary>
public sealed class SalesInvoicePostedIntegrationEventHandler(
    IReceivableRepository receivableRepository,
    IUnitOfWork unitOfWork) : INotificationHandler<SalesInvoicePostedIntegrationEvent>
{
    public async Task Handle(SalesInvoicePostedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        // Duplicate delivery of the same posted invoice must create exactly one Receivable (brief
        // §32) — the DB unique index on (SourceDocumentType, SourceDocumentId) is the final
        // safety net; this check is the fast path that avoids hitting it in the common case.
        if (await receivableRepository.ExistsForSourceDocumentAsync(SourceDocumentType.SalesInvoice, notification.InvoiceId, notification.CustomerId, cancellationToken))
            return;

        // A fully-discounted (net-zero) invoice creates no AR obligation — Receivable.Create
        // requires OriginalAmount > 0, and there is nothing to collect either way.
        if (notification.Amount <= 0)
            return;

        var receivable = Receivable.Create(
            customerId: notification.CustomerId,
            sourceDocumentType: SourceDocumentType.SalesInvoice,
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

        await receivableRepository.AddAsync(receivable, cancellationToken);
        await unitOfWork.SaveChangeAsync(cancellationToken);
    }
}

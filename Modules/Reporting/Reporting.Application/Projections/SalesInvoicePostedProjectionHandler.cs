namespace Reporting.Application.Projections;

using CommercialDocuments.Contracts.IntegrationEvents;
using MediatR;
using OrgSys.SharedKernel;

public sealed class SalesInvoicePostedProjectionHandler(IReportingProjectionStore store, IInboxStore inbox)
    : INotificationHandler<SalesInvoicePostedIntegrationEvent>
{
    public async Task Handle(SalesInvoicePostedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        if (!await inbox.TryClaimAsync(notification.EventId, nameof(SalesInvoicePostedProjectionHandler), cancellationToken))
            return;

        await store.ApplySalesInvoicePostedAsync(
            notification.CustomerId,
            notification.InvoiceId,
            notification.InvoiceDate,
            notification.Amount,
            notification.BranchId,
            cancellationToken);
    }
}

using CommercialDocuments.Contracts.IntegrationEvents;
using Moq;
using OrgSys.SharedKernel;
using Reporting.Application;
using Reporting.Application.Projections;
using Xunit;

namespace Application.Tests;

public class SalesInvoicePostedProjectionHandlerTests
{
    [Fact]
    public async Task Handle_NewEvent_AppliesProjectionWithoutMutatingInvoice()
    {
        var store = new Mock<IReportingProjectionStore>();
        var handler = new SalesInvoicePostedProjectionHandler(store.Object, InboxTestDoubles.AlwaysClaim());
        var evt = new SalesInvoicePostedIntegrationEvent(9, "INV", 3, new DateTime(2026, 1, 2), new DateTime(2026, 1, 2), 1, 1, 250, 1, DateTime.UtcNow, 4);

        await handler.Handle(evt, CancellationToken.None);

        store.Verify(s => s.ApplySalesInvoicePostedAsync(3, 9, evt.InvoiceDate, 250, 4, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_DuplicateEvent_DoesNotApply()
    {
        var store = new Mock<IReportingProjectionStore>();
        var handler = new SalesInvoicePostedProjectionHandler(store.Object, InboxTestDoubles.AlreadyClaimed());

        await handler.Handle(new SalesInvoicePostedIntegrationEvent(9, "INV", 3, DateTime.UtcNow, DateTime.UtcNow, 1, 1, 250, 1, DateTime.UtcNow, null), CancellationToken.None);

        store.Verify(s => s.ApplySalesInvoicePostedAsync(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<DateTime>(), It.IsAny<decimal>(), It.IsAny<long?>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}

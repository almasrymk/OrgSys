using CommercialDocuments.Contracts.Invoices;
using Inventory.Contracts.Receipts;
using MediatR;
using Moq;
using OrgSys.SharedKernel;
using Purchasing.Application.PurchaseOrders.Integration;
using Purchasing.Application.PurchaseOrders.Queries;
using Purchasing.Contracts.PurchaseOrders;
using Purchasing.Domain;
using System.Linq.Expressions;
using System.Net;
using Xunit;

namespace Application.Tests;

public class PurchaseOrderReceiptMatchTests
{
    private static readonly DateTime Day = new(2026, 9, 1);

    private static PurchaseOrder OpenOrder()
    {
        var order = PurchaseOrder.Create(7, 1, Day);
        var line = order.AddLine(10, 1, 4, 12);
        typeof(BaseModel).GetProperty(nameof(BaseModel.Id))!.SetValue(order, 44);
        typeof(BaseModel).GetProperty(nameof(BaseModel.Id))!.SetValue(line, 91);
        return order;
    }

    [Fact]
    public async Task GoodsReceiptPosted_RecordsReceiptAgainstMatchingPurchaseOrderLine()
    {
        var order = OpenOrder();
        var repository = new Mock<IRepository<PurchaseOrder>>();
        repository.Setup(r => r.GetByFilterAsync(It.IsAny<Expression<Func<PurchaseOrder, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync(order);
        repository.Setup(r => r.UpdateAsync(It.IsAny<PurchaseOrder>())).ReturnsAsync(true);
        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.SaveChangeAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var handler = new GoodsReceiptPostedIntegrationEventHandler(repository.Object, unitOfWork.Object, InboxTestDoubles.AlwaysClaim());
        await handler.Handle(
            new GoodsReceiptPostedIntegrationEvent(
                5, 3, null, Day, 44,
                [new GoodsReceiptPostedLine(10, 1, 2)]),
            CancellationToken.None);

        Assert.Equal(2, order.PurchaseOrderProducts.Single().ReceivedQuantity);
        Assert.Equal(2, order.PurchaseOrderProducts.Single().RemainingQuantity);
    }

    [Fact]
    public async Task GoodsReceiptPosted_DuplicateEventId_DoesNotReceiveTwice()
    {
        var order = OpenOrder();
        var repository = new Mock<IRepository<PurchaseOrder>>();
        repository.Setup(r => r.GetByFilterAsync(It.IsAny<Expression<Func<PurchaseOrder, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync(order);
        var handler = new GoodsReceiptPostedIntegrationEventHandler(repository.Object, Mock.Of<IUnitOfWork>(), InboxTestDoubles.AlreadyClaimed());

        await handler.Handle(
            new GoodsReceiptPostedIntegrationEvent(5, 3, null, Day, 44, [new GoodsReceiptPostedLine(10, 1, 2)]),
            CancellationToken.None);

        Assert.Equal(0, order.PurchaseOrderProducts.Single().ReceivedQuantity);
        repository.Verify(r => r.UpdateAsync(It.IsAny<PurchaseOrder>()), Times.Never);
    }

    [Fact]
    public async Task ThreeWayMatch_JoinsOrderedReceivedAndInvoicedQuantities()
    {
        var order = OpenOrder();
        order.RecordReceipt(91, 2);
        order.LinkInvoice(100);

        var repository = new Mock<IRepository<PurchaseOrder>>();
        repository.Setup(r => r.GetByFilterAsync(It.IsAny<Expression<Func<PurchaseOrder, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync(order);
        var sender = new Mock<ISender>();
        sender.Setup(s => s.Send(It.IsAny<GetInvoiceInventoryImpactQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Result<InvoiceInventoryImpactDto?>(
                HttpStatusCode.OK,
                new InvoiceInventoryImpactDto(
                    100, "PINV-100", 2, 7, 3, null, Day, Day, 1, null, null, null, true,
                    [new InvoiceInventoryLineDto(10, 1, 3, 4, 12, null)]),
                null));

        var result = await new GetThreeWayMatchQueryHandler(repository.Object, sender.Object)
            .Handle(new GetThreeWayMatchQuery(44), CancellationToken.None);

        Assert.NotNull(result.Response);
        var line = Assert.Single(result.Response!.Lines);
        Assert.Equal(4, line.OrderedQuantity);
        Assert.Equal(2, line.ReceivedQuantity);
        Assert.Equal(4, line.InvoicedQuantity);
        Assert.Equal(0, line.VarianceOrderedMinusInvoiced);
    }
}

using CommercialDocuments.Contracts.Invoices;
using MediatR;
using Moq;
using OrgSys.SharedKernel;
using Purchasing.Application.PurchaseOrders.Commands;
using Purchasing.Domain;
using System.Net;
using Xunit;

namespace Application.Tests;

public class LinkInvoiceCommandHandlerTests
{
    private static readonly DateTime CreateDate = new(2026, 9, 1);

    private static PurchaseOrder NewOrder() => PurchaseOrder.Create(dealerId: 1, createUserId: 1, createDate: CreateDate);

    private static (LinkInvoiceCommandHandler handler, Mock<IRepository<PurchaseOrder>> orderRepository, Mock<ISender> sender, Mock<IUnitOfWork> unitOfWork)
        BuildHandler(PurchaseOrder? order, InvoiceReferenceDto? invoice)
    {
        var orderRepository = new Mock<IRepository<PurchaseOrder>>();
        orderRepository.Setup(r => r.GetByFilterAsync(It.IsAny<System.Linq.Expressions.Expression<Func<PurchaseOrder, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync(order);

        var sender = new Mock<ISender>();
        sender.Setup(s => s.Send(It.IsAny<GetInvoiceReferenceQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Result<InvoiceReferenceDto?>(HttpStatusCode.OK, invoice, null));

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.SaveChangeAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var handler = new LinkInvoiceCommandHandler(unitOfWork.Object, orderRepository.Object, sender.Object);

        return (handler, orderRepository, sender, unitOfWork);
    }

    [Fact]
    public async Task Handle_ValidPurchaseInvoice_LinksAndApprovesOrder()
    {
        var order = NewOrder();
        var invoice = new InvoiceReferenceDto(100, "INV-100", InvoiceTypeId.Purchase, 5, IsDeleted: false);
        var (handler, _, _, _) = BuildHandler(order, invoice);

        var result = await handler.Handle(new LinkInvoiceCommand(1, 100), CancellationToken.None);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.Equal(100, order.InvoiceId);
        Assert.Equal(Status.Approved, order.Status);
    }

    [Fact]
    public async Task Handle_ValidPurchaseReturnInvoice_LinksAndApprovesOrder()
    {
        var order = NewOrder();
        var invoice = new InvoiceReferenceDto(101, "INV-101", InvoiceTypeId.PurchaseReturn, 5, IsDeleted: false);
        var (handler, _, _, _) = BuildHandler(order, invoice);

        var result = await handler.Handle(new LinkInvoiceCommand(1, 101), CancellationToken.None);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.Equal(101, order.InvoiceId);
    }

    [Fact]
    public async Task Handle_SalesInvoice_IsRejected()
    {
        var order = NewOrder();
        var invoice = new InvoiceReferenceDto(200, "INV-200", InvoiceTypeId.Sales, 5, IsDeleted: false);
        var (handler, _, _, unitOfWork) = BuildHandler(order, invoice);

        var result = await handler.Handle(new LinkInvoiceCommand(1, 200), CancellationToken.None);

        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        Assert.Null(order.InvoiceId);
        unitOfWork.Verify(u => u.SaveChangeAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_InvoiceNotFound_IsRejected()
    {
        var order = NewOrder();
        var (handler, _, _, _) = BuildHandler(order, invoice: null);

        var result = await handler.Handle(new LinkInvoiceCommand(1, 999), CancellationToken.None);

        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
    }

    [Fact]
    public async Task Handle_OrderAlreadyLinked_IsRejected()
    {
        var order = NewOrder();
        order.LinkInvoice(50);
        var invoice = new InvoiceReferenceDto(100, "INV-100", InvoiceTypeId.Purchase, 5, IsDeleted: false);
        var (handler, _, sender, _) = BuildHandler(order, invoice);

        var result = await handler.Handle(new LinkInvoiceCommand(1, 100), CancellationToken.None);

        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        sender.Verify(s => s.Send(It.IsAny<GetInvoiceReferenceQuery>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}

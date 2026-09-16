using Catalog.Contracts.Products;
using Inventory.Contracts.Availability;
using MediatR;
using Moq;
using OrgSys.SharedKernel;
using Sales.Application.SalesOrders.Commands;
using Sales.Domain;
using System.Net;
using Xunit;

namespace Application.Tests;

public class ConfirmSalesOrderCommandHandlerTests
{
    private static readonly DateTime CreateDate = new(2026, 9, 1);

    [Fact]
    public async Task Confirm_DraftOrder_ReservesThenConfirms()
    {
        var order = SalesOrder.Create(1, 1, 1, CreateDate, 1, CreateDate);
        order.AddLine(10, "Widget", 1, 2, 5, 0, 0);
        var repository = new Mock<IRepository<SalesOrder>>();
        repository.Setup(r => r.GetByFilterAsync(It.IsAny<System.Linq.Expressions.Expression<Func<SalesOrder, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync(order);
        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.SaveChangeAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        var sender = new Mock<ISender>();
        sender.Setup(s => s.Send(It.IsAny<ReserveInventoryCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Result<long>(HttpStatusCode.OK, 9, null));

        var handler = new ConfirmSalesOrderCommandHandler(repository.Object, unitOfWork.Object, sender.Object);
        var result = await handler.Handle(new ConfirmSalesOrderCommand(1, 3, null, 1), CancellationToken.None);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.Equal(SalesOrderStatus.Confirmed, order.LifecycleStatus);
        sender.Verify(s => s.Send(It.Is<ReserveInventoryCommand>(c => c.ProductId == 10 && c.StockId == 3 && c.Quantity == 2), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Create_LooksUpProductNames()
    {
        var repository = new Mock<IRepository<SalesOrder>>();
        repository.Setup(r => r.CreateAsync(It.IsAny<SalesOrder>())).ReturnsAsync((SalesOrder o) => o);
        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.SaveChangeAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        var sender = new Mock<ISender>();
        sender.Setup(s => s.Send(It.IsAny<GetProductNamesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Result<Dictionary<long, string?>>(HttpStatusCode.OK, new Dictionary<long, string?> { [10] = "Widget" }, null));

        var handler = new CreateSalesOrderCommandHandler(repository.Object, unitOfWork.Object, sender.Object);
        var result = await handler.Handle(new CreateSalesOrderCommand(
            1, 1, 1, CreateDate, 1, CreateDate, null, null, null, null, "SO-1",
            [new SalesOrderLineInput(10, 1, 2, 5, 0, 0, null)]), CancellationToken.None);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        repository.Verify(r => r.CreateAsync(It.Is<SalesOrder>(o => o.Lines.Single().ProductName == "Widget")), Times.Once);
    }
}

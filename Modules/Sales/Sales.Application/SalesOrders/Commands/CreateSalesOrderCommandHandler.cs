namespace Sales.Application.SalesOrders.Commands;

using Catalog.Contracts.Products;
using MediatR;
using System.Net;

public sealed record SalesOrderLineInput(long ProductId, long UnitId, decimal OrderedQuantity, decimal UnitPrice, decimal DiscountAmount, decimal TaxAmount, string? Notes);

public sealed record CreateSalesOrderCommand(
    long CustomerId,
    long CurrencyId,
    decimal Rate,
    DateTime OrderDate,
    long CreateUserId,
    DateTime CreateDate,
    long? SourceQuotationId,
    DateTime? RequestedDeliveryDate,
    long? BranchId,
    string? Notes,
    string? Code,
    List<SalesOrderLineInput> Lines) : ICommand<long>;

public sealed class CreateSalesOrderCommandHandler(IRepository<SalesOrder> repository, IUnitOfWork unitOfWork, ISender sender)
    : ICommandHandler<CreateSalesOrderCommand, long>
{
    public async Task<Result<long>> Handle(CreateSalesOrderCommand request, CancellationToken cancellationToken)
    {
        if (request.Lines is null || request.Lines.Count == 0)
            return new Result<long>(HttpStatusCode.BadRequest, 0, [new Error("At least one line is required.")]);

        try
        {
            var order = SalesOrder.Create(
                request.CustomerId, request.CurrencyId, request.Rate,
                request.OrderDate == default ? DateTime.Now : request.OrderDate,
                request.CreateUserId,
                request.CreateDate == default ? DateTime.Now : request.CreateDate,
                request.SourceQuotationId, request.RequestedDeliveryDate, request.BranchId, request.Notes);
            order.Code = request.Code;

            var productIds = request.Lines.Select(l => l.ProductId).Distinct().ToList();
            var names = (await sender.Send(new GetProductNamesQuery(productIds), cancellationToken)).Response ?? [];

            foreach (var line in request.Lines)
                order.AddLine(
                    line.ProductId,
                    names.GetValueOrDefault(line.ProductId) ?? string.Empty,
                    line.UnitId,
                    line.OrderedQuantity,
                    line.UnitPrice,
                    line.DiscountAmount,
                    line.TaxAmount,
                    null,
                    line.Notes);

            await repository.CreateAsync(order);
            await unitOfWork.SaveChangeAsync(cancellationToken);
            return new Result<long>(HttpStatusCode.OK, order.Id, null);
        }
        catch (SalesOrderDomainException ex)
        {
            return new Result<long>(HttpStatusCode.BadRequest, 0, [new Error(ex.Message)]);
        }
    }
}

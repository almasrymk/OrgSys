namespace Sales.Application.Quotations.Commands;

using System.Net;

public sealed record ConvertQuotationCommand(long Id, long CreateUserId) : ICommand<long>;

public sealed class ConvertQuotationCommandHandler(
    IRepository<Quotation> quotationRepository,
    IRepository<SalesOrder> orderRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<ConvertQuotationCommand, long>
{
    public async Task<Result<long>> Handle(ConvertQuotationCommand request, CancellationToken cancellationToken)
    {
        var quotation = await quotationRepository.GetByFilterAsync(e => e.Id == request.Id && e.Status != Status.Deleted, "Lines");
        if (quotation is null)
            return new Result<long>(HttpStatusCode.NotFound, 0, [new Error("Quotation not found.")]);

        try
        {
            var order = SalesOrder.Create(
                quotation.CustomerId, quotation.CurrencyId, quotation.Rate, DateTime.Now,
                request.CreateUserId, DateTime.Now, quotation.Id, null, quotation.BranchId, quotation.Notes);

            foreach (var line in quotation.Lines)
                order.AddLine(line.ProductId, line.ProductName, line.UnitId, line.Quantity, line.UnitPrice, line.DiscountAmount, line.TaxAmount, line.RequestedDeliveryDate, line.Notes);

            await orderRepository.CreateAsync(order);
            await unitOfWork.SaveChangeAsync(cancellationToken);

            quotation.MarkConverted(order.Id);
            await quotationRepository.UpdateAsync(quotation);
            await unitOfWork.SaveChangeAsync(cancellationToken);
            return new Result<long>(HttpStatusCode.OK, order.Id, null);
        }
        catch (QuotationDomainException ex)
        {
            return new Result<long>(HttpStatusCode.BadRequest, 0, [new Error(ex.Message)]);
        }
        catch (SalesOrderDomainException ex)
        {
            return new Result<long>(HttpStatusCode.BadRequest, 0, [new Error(ex.Message)]);
        }
    }
}

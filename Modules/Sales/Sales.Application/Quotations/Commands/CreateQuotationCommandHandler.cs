namespace Sales.Application.Quotations.Commands;

using Catalog.Contracts.Products;
using MediatR;
using System.Net;

public sealed record QuotationLineInput(long ProductId, long UnitId, decimal Quantity, decimal UnitPrice, decimal DiscountAmount, decimal TaxAmount, string? Notes);

public sealed record CreateQuotationCommand(
    long CustomerId,
    long CurrencyId,
    decimal Rate,
    DateTime QuotationDate,
    DateTime ValidUntil,
    long CreateUserId,
    DateTime CreateDate,
    long? BranchId,
    string? Notes,
    string? Code,
    List<QuotationLineInput> Lines) : ICommand<long>;

public sealed class CreateQuotationCommandHandler(IRepository<Quotation> repository, IUnitOfWork unitOfWork, ISender sender)
    : ICommandHandler<CreateQuotationCommand, long>
{
    public async Task<Result<long>> Handle(CreateQuotationCommand request, CancellationToken cancellationToken)
    {
        if (request.Lines is null || request.Lines.Count == 0)
            return new Result<long>(HttpStatusCode.BadRequest, 0, [new Error("At least one line is required.")]);

        try
        {
            var quotation = Quotation.Create(
                request.CustomerId, request.CurrencyId, request.Rate,
                request.QuotationDate == default ? DateTime.Now : request.QuotationDate,
                request.ValidUntil,
                request.CreateUserId,
                request.CreateDate == default ? DateTime.Now : request.CreateDate,
                request.BranchId, request.Notes);
            quotation.Code = request.Code;

            var productIds = request.Lines.Select(l => l.ProductId).Distinct().ToList();
            var names = (await sender.Send(new GetProductNamesQuery(productIds), cancellationToken)).Response ?? [];

            foreach (var line in request.Lines)
                quotation.AddLine(
                    line.ProductId,
                    names.GetValueOrDefault(line.ProductId) ?? string.Empty,
                    line.UnitId,
                    line.Quantity,
                    line.UnitPrice,
                    line.DiscountAmount,
                    line.TaxAmount,
                    null,
                    line.Notes);

            await repository.CreateAsync(quotation);
            await unitOfWork.SaveChangeAsync(cancellationToken);
            return new Result<long>(HttpStatusCode.OK, quotation.Id, null);
        }
        catch (QuotationDomainException ex)
        {
            return new Result<long>(HttpStatusCode.BadRequest, 0, [new Error(ex.Message)]);
        }
    }
}

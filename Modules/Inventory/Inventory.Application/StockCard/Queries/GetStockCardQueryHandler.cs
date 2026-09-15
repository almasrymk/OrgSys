namespace Inventory.Application.StockCard.Queries;

using System.Net;

/// <summary>Traditional ERP stock card (brief §39) — a read model over the InventoryMovement ledger
/// directly (Transaction/TransactionProduct), never via a domain repository loading full aggregates
/// (brief §38). Running balance/value are historically correct because they're computed in date
/// order from the ledger, not read off today's InventoryBalance snapshot.</summary>
public sealed record StockCardLineDto(
    DateTime Date, string? Reference, long MovementTypeId, string? DocumentNumber,
    decimal ReceiptQuantity, decimal IssueQuantity, decimal RunningBalance,
    decimal UnitCost, decimal TransactionValue, decimal RunningValue);

public sealed record GetStockCardQuery(long ProductId, long? StockId, DateTime? DateFrom, DateTime? DateTo) : ICommandCollection<StockCardLineDto>;

public sealed class GetStockCardQueryHandler(IRepository<TransactionProduct> transactionProductRepository)
    : ICommandCollectionHandler<GetStockCardQuery, StockCardLineDto>
{
    public async Task<ResultCollection<StockCardLineDto>> Handle(GetStockCardQuery request, CancellationToken cancellationToken)
    {
        var lines = (await transactionProductRepository.GetListByFilterAsync(
            e => e.ProductId == request.ProductId
                && (request.StockId == null || e.StockId == request.StockId)
                && (request.DateFrom == null || e.Transaction!.Date >= request.DateFrom)
                && (request.DateTo == null || e.Transaction!.Date <= request.DateTo),
            "Transaction"))?
            .Where(e => e.Transaction is not null)
            .OrderBy(e => e.Transaction!.Date).ThenBy(e => e.Transaction!.Id)
            .ToList() ?? [];

        decimal runningBalance = 0;
        decimal runningValue = 0;
        var result = new List<StockCardLineDto>(lines.Count);

        foreach (var line in lines)
        {
            var transaction = line.Transaction!;
            var movementType = MovementTypeExtensions.FromTransactionTypeId(transaction.TypeId);
            var signedQuantity = movementType.SignedQuantity(line.Quantity);
            var lineValue = signedQuantity * line.Cost;

            runningBalance += signedQuantity;
            runningValue += lineValue;

            result.Add(new StockCardLineDto(
                transaction.Date,
                transaction.SourceDocumentType?.ToString(),
                transaction.TypeId,
                transaction.Code,
                ReceiptQuantity: signedQuantity > 0 ? signedQuantity : 0,
                IssueQuantity: signedQuantity < 0 ? -signedQuantity : 0,
                RunningBalance: runningBalance,
                UnitCost: line.Cost,
                TransactionValue: lineValue,
                RunningValue: runningValue));
        }

        return new ResultCollection<StockCardLineDto>(HttpStatusCode.OK, result, null);
    }
}

namespace Inventory.Application.InventoryBatches.Queries;

using System.Net;

/// <summary>Backs the Expiry report (brief §41) — "expired" and "expires within N days" are both
/// just filters over ExpiryDate, computed from the current date at query time (not stored).</summary>
public sealed record BatchExpiryDto(long Id, long ProductId, string BatchNumber, DateTime? ExpiryDate, int? DaysUntilExpiry, bool IsExpired);

public sealed record GetExpiringBatchesQuery(long? ProductId, int WithinDays) : ICommandCollection<BatchExpiryDto>;

public sealed class GetExpiringBatchesQueryHandler(IRepository<InventoryBatch> batchRepository)
    : ICommandCollectionHandler<GetExpiringBatchesQuery, BatchExpiryDto>
{
    public async Task<ResultCollection<BatchExpiryDto>> Handle(GetExpiringBatchesQuery request, CancellationToken cancellationToken)
    {
        var today = DateTime.Today;
        var horizon = today.AddDays(request.WithinDays);

        var batches = (await batchRepository.GetListByFilterAsync(b =>
            b.ExpiryDate != null
            && b.ExpiryDate <= horizon
            && (request.ProductId == null || b.ProductId == request.ProductId)))?.ToList() ?? [];

        var dtos = batches.Select(b => new BatchExpiryDto(
            b.Id, b.ProductId, b.BatchNumber, b.ExpiryDate,
            b.ExpiryDate.HasValue ? (int)(b.ExpiryDate.Value.Date - today).TotalDays : null,
            b.IsExpired(today))).ToList();

        return new ResultCollection<BatchExpiryDto>(HttpStatusCode.OK, dtos, null);
    }
}

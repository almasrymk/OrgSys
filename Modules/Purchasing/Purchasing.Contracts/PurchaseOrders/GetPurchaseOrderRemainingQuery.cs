namespace Purchasing.Contracts.PurchaseOrders;

using OrgSys.SharedKernel;

public sealed record PurchaseOrderRemainingLineDto(
    long LineId,
    long ProductId,
    long UnitId,
    decimal OrderedQuantity,
    decimal RemainingQuantity);

public sealed record PurchaseOrderRemainingDto(
    long PurchaseOrderId,
    string? Code,
    long DealerId,
    IReadOnlyList<PurchaseOrderRemainingLineDto> Lines);

public sealed record GetPurchaseOrderRemainingQuery(long PurchaseOrderId) : IQuery<PurchaseOrderRemainingDto?>;

namespace Purchasing.Contracts.PurchaseOrders;

using OrgSys.SharedKernel;

public sealed record ThreeWayMatchLineDto(
    long LineId,
    long ProductId,
    long UnitId,
    decimal OrderedQuantity,
    decimal ReceivedQuantity,
    decimal InvoicedQuantity,
    decimal RemainingToReceive,
    decimal VarianceOrderedMinusInvoiced);

public sealed record ThreeWayMatchDto(
    long PurchaseOrderId,
    long? InvoiceId,
    IReadOnlyList<ThreeWayMatchLineDto> Lines);

public sealed record GetThreeWayMatchQuery(long PurchaseOrderId) : IQuery<ThreeWayMatchDto?>;

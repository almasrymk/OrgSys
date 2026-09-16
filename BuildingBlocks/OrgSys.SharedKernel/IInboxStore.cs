namespace OrgSys.SharedKernel;

/// <summary>
/// Claims an (EventId, handler) pair once. Duplicate deliveries return false so InvoicePosted /
/// PaymentPosted / GoodsReceiptPosted handlers stay idempotent even after unique indexes fire.
/// </summary>
public interface IInboxStore
{
    Task<bool> TryClaimAsync(Guid eventId, string handlerName, CancellationToken cancellationToken = default);
}

namespace Inventory.Domain.Exceptions;

/// <summary>
/// Base type for every Inventory-invariant violation across the bounded context (Warehouse,
/// InventoryReceipt, InventoryIssue, StockTransfer, StockAdjustment, StockReservation,
/// InventoryBatch, InventorySerial). The Application layer catches these at the command-handler
/// boundary and translates them into the existing OrgSys.SharedKernel.Result/Error shape — Domain
/// itself has no knowledge of HTTP status codes. Mirrors Advances.Domain.Exceptions.CustodyDomainException
/// / Payables.Domain.Exceptions.PayableDomainException. Grouped in one file (brief §52's own flat
/// list) rather than one file per aggregate, since these are the bounded context's shared error
/// vocabulary.
/// </summary>
public abstract class InventoryDomainException(string message) : Exception(message);

// ----- Warehouse (Stock) -----

/// <summary>A warehouse code collided with another warehouse's code.</summary>
public sealed class DuplicateWarehouseCodeException(string message) : InventoryDomainException(message);

/// <summary>A warehouse was created without a code, or without a valid owning branch.</summary>
public sealed class WarehouseCodeRequiredException(string message) : InventoryDomainException(message);

/// <summary>A transactional document (receipt/issue/transfer/adjustment/count) targeted an inactive warehouse.</summary>
public sealed class WarehouseInactiveException(string message) : InventoryDomainException(message);

/// <summary>A warehouse deletion was attempted while stock or posted movements still reference it — deactivate instead.</summary>
public sealed class WarehouseNotEmptyException(string message) : InventoryDomainException(message);

// ----- Item (Product) -----

/// <summary>A transactional document line referenced an inactive item.</summary>
public sealed class ItemInactiveException(string message) : InventoryDomainException(message);

/// <summary>Product.TrackingType was changed after movements already exist for it (brief §16: tracking policy is fixed once movements exist).</summary>
public sealed class ItemTrackingPolicyLockedException(string message) : InventoryDomainException(message);

// ----- Availability / posting -----

/// <summary>An issue/reservation/transfer-out would take Available below zero and the warehouse/item does not allow negative stock.</summary>
public sealed class InsufficientStockException(string message) : InventoryDomainException(message);

/// <summary>A duplicate post was attempted for a (SourceDocumentType, SourceDocumentId[, SourceDocumentLineId]) that already produced a movement (brief §31 idempotency).</summary>
public sealed class DuplicateInventoryOperationException(string message) : InventoryDomainException(message);

// ----- Document lifecycle (InventoryReceipt / InventoryIssue / StockAdjustment) -----

public sealed class InventoryDocumentLineRequiredException(string message) : InventoryDomainException(message);

/// <summary>A field edit or Post() was attempted on a document that is already Posted — posted documents are immutable (brief §9/§57).</summary>
public sealed class CannotModifyPostedDocumentException(string message) : InventoryDomainException(message);

/// <summary>Cancel() was attempted on a document that is already Posted — a posted issue/receipt must be reversed, not cancelled (brief §56).</summary>
public sealed class CannotCancelPostedDocumentWithoutReversalException(string message) : InventoryDomainException(message);

// ----- StockTransfer -----

public sealed class InvalidStockTransferException(string message) : InventoryDomainException(message);

/// <summary>An action was attempted on a StockTransfer whose current status doesn't allow it (e.g. re-posting an already-Completed transfer).</summary>
public sealed class TransferAlreadyPostedException(string message) : InventoryDomainException(message);

// ----- StockReservation -----

public sealed class ReservationNotActiveException(string message) : InventoryDomainException(message);

public sealed class ReservationAlreadyFulfilledException(string message) : InventoryDomainException(message);

// ----- Batch / Serial -----

public sealed class InvalidBatchException(string message) : InventoryDomainException(message);

/// <summary>An issue targeted an expired batch and the caller did not explicitly override (brief §16: expired batch cannot normally be issued).</summary>
public sealed class BatchExpiredException(string message) : InventoryDomainException(message);

public sealed class SerialNotAvailableException(string message) : InventoryDomainException(message);

/// <summary>A serial already in Issued status was targeted by another issue (brief §17: a serial cannot be issued twice).</summary>
public sealed class SerialAlreadyIssuedException(string message) : InventoryDomainException(message);

// ----- Movement reversal -----

/// <summary>Reverse() was attempted on a movement that was already reversed, or on one that is itself a reversal.</summary>
public sealed class MovementAlreadyReversedException(string message) : InventoryDomainException(message);

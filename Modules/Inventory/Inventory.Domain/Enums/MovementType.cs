namespace Inventory.Domain.Enums;

/// <summary>
/// Wraps the existing seeded TransactionType.Id values (1-8) so movement-direction logic stops
/// being expressed as scattered `TypeId == n` magic numbers (previously duplicated across
/// GetListByBalanceQueryHandler and TransactionJournalPostingService — see
/// docs/ddd/inventory-current-state.md §3 for how these values were reverse-engineered). Values are
/// fixed to match the existing seeded TransactionType table exactly — this is a typed wrapper over
/// existing data, not a new numbering scheme, so it must never be reordered.
/// </summary>
public enum MovementType
{
    Receipt = 1,
    Issue = 2,
    TransferIssue = 3,
    TransferReceipt = 4,
    AdjustmentIncrease = 5,
    AdjustmentDecrease = 6,
    OpeningBalance = 7,
    DamageLoss = 8
}

public enum MovementDirection
{
    In,
    Out
}

public static class MovementTypeExtensions
{
    public static MovementDirection Direction(this MovementType type) => type switch
    {
        MovementType.Receipt or MovementType.TransferReceipt or MovementType.AdjustmentIncrease or MovementType.OpeningBalance
            => MovementDirection.In,
        MovementType.Issue or MovementType.TransferIssue or MovementType.AdjustmentDecrease or MovementType.DamageLoss
            => MovementDirection.Out,
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Unknown movement type.")
    };

    /// <summary>Returns quantity signed for balance arithmetic (+ for In, - for Out) — the single
    /// place this sign decision is made, replacing every ad hoc `TypeId == 2 || TypeId == 3 || ...`
    /// check.</summary>
    public static decimal SignedQuantity(this MovementType type, decimal quantity) =>
        type.Direction() == MovementDirection.In ? quantity : -quantity;

    public static MovementType FromTransactionTypeId(long typeId) =>
        Enum.IsDefined(typeof(MovementType), (int)typeId)
            ? (MovementType)typeId
            : throw new ArgumentOutOfRangeException(nameof(typeId), typeId, "Unrecognized inventory transaction type id.");
}

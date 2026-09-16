namespace Inventory.Domain
{
    [Table("Transaction")]
    public class Transaction : MovementModel
    {
        /// <summary>Scalar-only reference into Parties.Domain.Dealer — no EF navigation.
        /// FK preserved via Fluent HasOne(typeof(Dealer)) in OrgContext.</summary>
        public virtual long? DealerId { get; set; }

        [ForeignKey("Stock")]
        public virtual long? StockId { get; set; }

        public virtual Stock? Stock { get; set; }

        [ForeignKey("ToStock")]
        public virtual long? ToStockId { get; set; }

        public virtual Stock? ToStock { get; set; }

        [ForeignKey("Inventory")]
        public virtual long? InventoryId { get; set; }

        public virtual Inventory? Inventory { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Total { get; set; }

        [StringLength(500)]
        public virtual string? Notes { get; set; }

        /// <summary>Additive (brief §31/§55) — structured replacement for the previous string-based
        /// correlation (`Notes = "InventoryId: {id}"`). Nullable so existing rows/handlers that never
        /// set it keep working; new posting code (InventoryReceipt/Issue/StockTransfer/
        /// StockAdjustment/StockCount) populates it going forward.</summary>
        public virtual SourceDocumentType? SourceDocumentType { get; set; }

        public virtual long? SourceDocumentId { get; set; }

        public virtual long? SourceDocumentLineId { get; set; }

        /// <summary>Additive (brief §31) — a unique index on this column (added in Phase 5's EF
        /// configuration) is what actually prevents a duplicate post; nullable so it's opt-in per
        /// caller rather than forced on every historical row.</summary>
        [StringLength(200)]
        public virtual string? IdempotencyKey { get; set; }

        /// <summary>Additive (brief §56) — set only on a compensating movement created by
        /// <see cref="CreateReversal"/>; null on every ordinary movement.</summary>
        public virtual long? ReversalOfMovementId { get; set; }

        public virtual ICollection<TransactionProduct>? TransactionProducts { get; set; }

        /// <summary>
        /// Builds the compensating movement for <paramref name="original"/> — brief §56: correcting a
        /// posted movement always creates an opposite movement, never edits or deletes the original.
        /// A pure builder (not a method on the immutable posted instance, since Transaction keeps its
        /// existing AutoMapper-CRUD shape — docs/ddd/inventory-target-architecture.md §2) so existing
        /// handlers are unaffected; only new reversal-posting code calls this.
        /// </summary>
        public static Transaction CreateReversal(Transaction original, long createUserId, DateTime createDate, DateTime reversalDate, string? reason)
        {
            if (original.Id <= 0)
                throw new MovementAlreadyReversedException("Only an already-posted movement can be reversed.");

            var reversalNotes = string.IsNullOrWhiteSpace(reason)
                ? $"Reversal of Transaction {original.Id}"
                : $"Reversal of Transaction {original.Id}: {reason}";

            return new Transaction
            {
                TypeId = original.TypeId,
                DealerId = original.DealerId,
                StockId = original.ToStockId ?? original.StockId,
                ToStockId = original.StockId != (original.ToStockId ?? original.StockId) ? original.StockId : null,
                Total = original.Total,
                Notes = reversalNotes.Length <= 500 ? reversalNotes : reversalNotes[..500],
                Date = reversalDate,
                CreateUserId = createUserId,
                CreateDate = createDate,
                BranchId = original.BranchId,
                ShiftId = original.ShiftId,
                SourceDocumentType = global::Inventory.Domain.SourceDocumentType.Reversal,
                SourceDocumentId = original.Id,
                ReversalOfMovementId = original.Id
            };
        }
    }
}

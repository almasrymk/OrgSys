namespace Inventory.Domain
{
    [Table("Stock")]
    public class Stock : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public virtual string? Name { get; set; }

        [ForeignKey("Branch")]
        public virtual long BranchId { get; set; }

        public virtual Branch? Branch { get; set; }

        // No navigation to Accounting.Domain.Account — see Dealer.cs (Parties.Domain) for why;
        // same GeneralLedger bounded-context isolation rule applies here.
        public virtual long? AccountId { get; set; }

        /// <summary>Additive (brief §4.1) — when true, InventoryBalance.IssueOut/Reserve skip the
        /// InsufficientStockException guard for this warehouse. Defaults to false: negative stock is
        /// never silently allowed (brief §10/§70).</summary>
        public virtual bool AllowNegativeStock { get; set; }

        /// <summary>Additive (brief §4.1) — default WarehouseLocation a new InventoryReceipt line
        /// pre-fills for this warehouse. Nullable: locations are optional (brief §4.2).</summary>
        public virtual long? DefaultReceivingLocationId { get; set; }

        public virtual long? DefaultIssueLocationId { get; set; }

        /// <summary>Additive (brief §4.1) — an inactive warehouse rejects new transactional
        /// documents (WarehouseInactiveException) but keeps its historical movements/balance
        /// queryable.</summary>
        public virtual bool IsActive { get; set; } = true;

        /// <summary>Minimal invariant-checked construction path (brief §4.1/§62 "Warehouse creation
        /// invariants"). Kept alongside — not instead of — the existing plain-object-initializer/
        /// AutoMapper CRUD path (docs/ddd/inventory-target-architecture.md §2): code uniqueness
        /// itself is an infrastructure-level unique index (Phase 5), not something Domain can check
        /// without a repository.</summary>
        public static Stock Create(string code, string? name, long branchId)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new WarehouseCodeRequiredException("A warehouse code is required.");
            if (branchId <= 0)
                throw new WarehouseCodeRequiredException("A warehouse must belong to a valid branch.");

            return new Stock { Code = code.Trim(), Name = name, BranchId = branchId, IsActive = true };
        }

        /// <summary>Guard used by every new posting handler (Receipt/Issue/Transfer/Adjustment) before
        /// creating a movement against this warehouse — brief §4.1: "Inactive warehouse cannot
        /// receive new operational transactions."</summary>
        public void EnsureActiveForPosting()
        {
            if (!IsActive)
                throw new WarehouseInactiveException($"Warehouse {Code} is inactive and cannot accept new transactional documents.");
        }

        public void Deactivate() => IsActive = false;

        public void Activate() => IsActive = true;
    }
}

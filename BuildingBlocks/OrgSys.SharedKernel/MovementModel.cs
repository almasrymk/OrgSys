namespace OrgSys.SharedKernel;

/// <summary>
/// Shared shape for "auditable transactional document" entities (Financial, FinancialTransfer,
/// Invoice, Journal, Order, Transaction, Inventory) — moved from Domain/Entities/MovementModel.cs.
///
/// The original class also carried CreateUser/ModifyUser (User), Shift, and Branch navigation
/// properties. Those are deliberately dropped here rather than moved — see
/// docs/modular-monolith-target-architecture.md §2: SharedKernel must not carry navigation
/// properties into other modules' entities. Confirmed safe to drop (not just relocate) because:
///   - No lazy-loading proxies are registered anywhere (grep confirmed), and
///   - No code calls Include("CreateUser"/"ModifyUser"/"Shift") or dereferences .CreateUser/
///     .ModifyUser/.Shift/.Branch (grep confirmed) — the DTOs' CreateUserName/ModifyUserName
///     properties were already always null in production (AutoMapper's flattening convention
///     had nothing to flatten from, since the navigation was never loaded).
/// The FK *columns* (and their DB constraints) are preserved exactly via Fluent API
/// "no navigation" relationships added to OrgContext.OnModelCreating for each derived entity —
/// see Infrastructure/Persistence/Data/OrgContext.cs. This keeps the schema byte-for-byte
/// identical (verified with `dotnet ef migrations has-pending-model-changes`).
/// </summary>
public class MovementModel : BaseModel
{
    public DateTime Date { get; set; }

    public long CreateUserId { get; set; }

    public DateTime CreateDate { get; set; }

    public long? ModifyUserId { get; set; }

    public DateTime? ModifyDate { get; set; }

    public long? ShiftId { get; set; }

    public long? BranchId { get; set; }

    public bool HasJournal { get; set; }

    public bool Review { get; set; }

    public bool Posted { get; set; }
}

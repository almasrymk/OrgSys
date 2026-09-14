namespace Receivables.Domain;

using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// Audit/idempotency record of one customer payment (Treasury Financial, ReferenceType.Customer)
/// being FIFO-applied across a customer's open Receivables — see
/// docs/architecture/receivables-ddd-migration.md §9/§12 (Phase 6). Not a general-purpose
/// "CustomerPayment" aggregate: the source Treasury payment itself remains Treasury's own
/// Financial/FinancialAccount concern (brief §10/§48) — this only records *how AR applied it*, one
/// row per posted Financial, with a Lines breakdown of which Receivable(s) received how much.
/// </summary>
[Table("PaymentApplication")]
public class PaymentApplication : MovementModel
{
    private readonly List<PaymentApplicationLine> _lines = [];

    public IReadOnlyCollection<PaymentApplicationLine> Lines => _lines.AsReadOnly();

    /// <summary>EF materialization constructor only. Business code creates a PaymentApplication
    /// through <see cref="Create"/>.</summary>
    protected PaymentApplication() { }

    /// <summary>The Treasury Financial (customer receipt) this application was built from — unique
    /// per Financial, the idempotency key for duplicate event delivery (brief §32).</summary>
    public virtual long SourceFinancialId { get; private set; }

    public virtual long CustomerId { get; private set; }

    [Column(TypeName = "decimal(18,2)")]
    public virtual decimal Amount { get; private set; }

    /// <summary>Amount not applied to any Receivable — either the customer had no open items left,
    /// or the payment exceeded total outstanding. Stays on this record as an audit trail; there is
    /// no separate "unapplied customer credit" ledger in this pass (see brief §16/§20, deferred) —
    /// this amount is on-account and untracked beyond this row.</summary>
    [Column(TypeName = "decimal(18,2)")]
    public virtual decimal UnappliedAmount { get; private set; }

    public static PaymentApplication Create(long sourceFinancialId, long customerId, decimal amount, long createUserId, DateTime createDate, long? branchId = null)
    {
        if (amount <= 0)
            throw new InvalidAllocationAmountException("A payment application amount must be greater than zero.");

        return new PaymentApplication
        {
            SourceFinancialId = sourceFinancialId,
            CustomerId = customerId,
            Amount = amount,
            UnappliedAmount = amount,
            CreateUserId = createUserId,
            CreateDate = createDate,
            BranchId = branchId
        };
    }

    /// <summary>Records that part of this payment was applied to one Receivable — the caller
    /// (Receivables.Application) is responsible for actually calling Receivable.Apply(appliedAmount)
    /// on that Receivable; this only tracks the allocation for audit/idempotency.</summary>
    public void RecordLine(long receivableId, decimal appliedAmount)
    {
        if (appliedAmount <= 0)
            throw new InvalidAllocationAmountException("An applied amount must be greater than zero.");
        if (appliedAmount > UnappliedAmount)
            throw new InvalidAllocationAmountException($"Cannot apply {appliedAmount} — only {UnappliedAmount} of this payment remains unapplied.");

        _lines.Add(new PaymentApplicationLine(receivableId, appliedAmount) { PaymentApplicationId = Id });
        UnappliedAmount -= appliedAmount;
    }
}

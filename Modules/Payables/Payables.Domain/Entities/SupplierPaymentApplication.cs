namespace Payables.Domain;

using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// Audit/idempotency record of one supplier payment (Treasury Financial, ReferenceType.Supplier,
/// Direction.Out) being FIFO-applied across a supplier's open Payables. Not a general-purpose
/// "SupplierPayment" aggregate: the source Treasury payment itself remains Treasury's own
/// Financial/FinancialAccount concern — this only records *how AP applied it*, one row per posted
/// Financial, with a Lines breakdown of which Payable(s) received how much. Mirrors
/// Receivables.Domain.PaymentApplication.
/// </summary>
[Table("SupplierPaymentApplication")]
public class SupplierPaymentApplication : MovementModel
{
    private readonly List<SupplierPaymentApplicationLine> _lines = [];

    public IReadOnlyCollection<SupplierPaymentApplicationLine> Lines => _lines.AsReadOnly();

    /// <summary>EF materialization constructor only. Business code creates a
    /// SupplierPaymentApplication through <see cref="Create"/>.</summary>
    protected SupplierPaymentApplication() { }

    /// <summary>The Treasury Financial (supplier payment) this application was built from — unique
    /// per Financial, the idempotency key for duplicate event delivery.</summary>
    public virtual long SourceFinancialId { get; private set; }

    public virtual long SupplierId { get; private set; }

    [Column(TypeName = "decimal(18,2)")]
    public virtual decimal Amount { get; private set; }

    /// <summary>Amount not applied to any Payable — either the supplier had no open items left, or
    /// the payment exceeded total outstanding. Stays on this record as an audit trail; there is no
    /// separate "supplier advance" ledger in this pass — this amount is on-account and untracked
    /// beyond this row.</summary>
    [Column(TypeName = "decimal(18,2)")]
    public virtual decimal UnappliedAmount { get; private set; }

    public static SupplierPaymentApplication Create(long sourceFinancialId, long supplierId, decimal amount, long createUserId, DateTime createDate, long? branchId = null)
    {
        if (amount <= 0)
            throw new InvalidAllocationAmountException("A payment application amount must be greater than zero.");

        return new SupplierPaymentApplication
        {
            SourceFinancialId = sourceFinancialId,
            SupplierId = supplierId,
            Amount = amount,
            UnappliedAmount = amount,
            CreateUserId = createUserId,
            CreateDate = createDate,
            BranchId = branchId
        };
    }

    /// <summary>Records that part of this payment was applied to one Payable — the caller
    /// (Payables.Application) is responsible for actually calling Payable.Apply(appliedAmount) on
    /// that Payable; this only tracks the allocation for audit/idempotency.</summary>
    public void RecordLine(long payableId, decimal appliedAmount)
    {
        if (appliedAmount <= 0)
            throw new InvalidAllocationAmountException("An applied amount must be greater than zero.");
        if (appliedAmount > UnappliedAmount)
            throw new InvalidAllocationAmountException($"Cannot apply {appliedAmount} — only {UnappliedAmount} of this payment remains unapplied.");

        _lines.Add(new SupplierPaymentApplicationLine(payableId, appliedAmount) { SupplierPaymentApplicationId = Id });
        UnappliedAmount -= appliedAmount;
    }
}

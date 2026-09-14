namespace Payables.Domain;

using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// One AP open item — a liability to a supplier created from a posted source document (currently:
/// a posted purchase invoice, see SourceDocumentType). This is the subledger's own aggregate; it
/// does not post to or read the General Ledger itself (that stays Accounting's job via
/// Accounting.Contracts) and does not hold a Supplier/Invoice navigation (SupplierId/
/// SourceDocumentId are external references only). Mirrors Receivables.Domain.Receivable — see
/// docs/architecture/payables-ddd-migration.md for the AP-specific design notes (Credit-natured
/// balance, outbound settlement).
/// </summary>
[Table("Payable")]
public class Payable : MovementModel
{
    private readonly List<IDomainEvent> _domainEvents = [];

    /// <summary>
    /// Payable cannot inherit OrgSys.SharedKernel.AggregateRoot as well as MovementModel (C# has no
    /// multiple inheritance) and MovementModel must be kept so Payable satisfies the existing
    /// generic IRepository&lt;TEntity&gt; where TEntity : BaseModel constraint — so domain events are
    /// tracked manually here, the same way Receivables.Domain.Receivable does for the identical
    /// reason.
    /// </summary>
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void ClearDomainEvents() => _domainEvents.Clear();

    /// <summary>EF materialization constructor only. Business code creates a Payable through
    /// <see cref="Create"/> — never via a bare object initializer, which would leave OutstandingAmount/
    /// LifecycleStatus in an unvalidated state.</summary>
    protected Payable() { }

    public virtual long SupplierId { get; private set; }

    public virtual SourceDocumentType SourceDocumentType { get; private set; }

    public virtual long SourceDocumentId { get; private set; }

    public virtual string? SourceDocumentNumber { get; private set; }

    public virtual DateTime DocumentDate { get; private set; }

    public virtual DateTime DueDate { get; private set; }

    public virtual long CurrencyId { get; private set; }

    [Column(TypeName = "decimal(18,2)")]
    public virtual decimal Rate { get; private set; }

    [Column(TypeName = "decimal(18,2)")]
    public virtual decimal OriginalAmount { get; private set; }

    [Column(TypeName = "decimal(18,2)")]
    public virtual decimal OutstandingAmount { get; private set; }

    /// <summary>Named distinctly from the inherited BaseModel.Status — see
    /// Receivables.Domain.Receivable.LifecycleStatus's identical remark.</summary>
    public virtual PayableStatus LifecycleStatus { get; private set; }

    /// <summary>Derived, not stored — see PayableStatus's remark on why "Overdue" is not a lifecycle state.</summary>
    public bool IsOverdue(DateTime asOfDate) => OutstandingAmount > 0 && DueDate.Date < asOfDate.Date;

    // ----- Construction -----

    public static Payable Create(
        long supplierId,
        SourceDocumentType sourceDocumentType,
        long sourceDocumentId,
        string? sourceDocumentNumber,
        DateTime documentDate,
        DateTime dueDate,
        long currencyId,
        decimal rate,
        decimal originalAmount,
        long createUserId,
        DateTime createDate,
        long? branchId = null)
    {
        if (originalAmount <= 0)
            throw new InvalidPayableAmountException("A payable's original amount must be greater than zero.");

        return new Payable
        {
            SupplierId = supplierId,
            SourceDocumentType = sourceDocumentType,
            SourceDocumentId = sourceDocumentId,
            SourceDocumentNumber = sourceDocumentNumber,
            DocumentDate = documentDate,
            DueDate = dueDate,
            CurrencyId = currencyId,
            Rate = rate,
            OriginalAmount = originalAmount,
            OutstandingAmount = originalAmount,
            LifecycleStatus = PayableStatus.Open,
            CreateUserId = createUserId,
            CreateDate = createDate,
            BranchId = branchId
        };
    }

    // ----- Settlement -----

    /// <summary>Applies a payment/allocation against this open item. Settles it once OutstandingAmount reaches zero.</summary>
    public void Apply(decimal amount)
    {
        if (amount <= 0)
            throw new InvalidAllocationAmountException("A payment allocation amount must be greater than zero.");

        EnsureOpenForApplication();

        if (amount > OutstandingAmount)
            throw new InvalidAllocationAmountException($"Allocation amount {amount} exceeds the outstanding amount {OutstandingAmount}.");

        OutstandingAmount -= amount;
        LifecycleStatus = OutstandingAmount == 0 ? PayableStatus.Settled : PayableStatus.PartiallySettled;

        Raise(new PaymentAppliedDomainEvent(Id, amount, OutstandingAmount));
        if (LifecycleStatus == PayableStatus.Settled)
            Raise(new PayableSettledDomainEvent(Id));
    }

    /// <summary>Restores a previously-applied amount (e.g. the underlying payment was reversed). Never restores more than the original amount.</summary>
    public void Unapply(decimal amount)
    {
        if (amount <= 0)
            throw new InvalidAllocationAmountException("An unapply amount must be greater than zero.");

        if (LifecycleStatus is PayableStatus.Cancelled or PayableStatus.WrittenOff)
            throw new PayableNotOpenForApplicationException($"Cannot unapply a payment on a {LifecycleStatus} payable.");

        var restored = OutstandingAmount + amount;
        if (restored > OriginalAmount)
            throw new InvalidAllocationAmountException($"Unapplying {amount} would restore the outstanding amount above the original amount {OriginalAmount}.");

        OutstandingAmount = restored;
        LifecycleStatus = OutstandingAmount == OriginalAmount ? PayableStatus.Open : PayableStatus.PartiallySettled;

        Raise(new PaymentUnappliedDomainEvent(Id, amount, OutstandingAmount));
    }

    // ----- Cancellation / write-off -----

    /// <summary>Draft-equivalent cancellation — only valid while nothing has been applied or written off yet. Idempotent if already Cancelled.</summary>
    public void Cancel()
    {
        if (LifecycleStatus == PayableStatus.Cancelled)
            return;

        if (OutstandingAmount != OriginalAmount)
            throw new PayableCannotBeCancelledException("A payable with an applied payment or write-off cannot be cancelled — unapply/reverse first.");

        LifecycleStatus = PayableStatus.Cancelled;
        Raise(new PayableCancelledDomainEvent(Id));
    }

    /// <summary>Writes off part or all of the outstanding amount. A partial write-off leaves the payable open for the remaining balance; LifecycleStatus only moves to WrittenOff once OutstandingAmount reaches zero.</summary>
    public void WriteOff(decimal amount, string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new WriteOffReasonRequiredException("A write-off reason is required.");

        if (amount <= 0)
            throw new InvalidAllocationAmountException("A write-off amount must be greater than zero.");

        EnsureOpenForApplication();

        if (amount > OutstandingAmount)
            throw new InvalidAllocationAmountException($"Write-off amount {amount} exceeds the outstanding amount {OutstandingAmount}.");

        OutstandingAmount -= amount;
        if (OutstandingAmount == 0)
            LifecycleStatus = PayableStatus.WrittenOff;

        Raise(new PayableWrittenOffDomainEvent(Id, amount, reason, OutstandingAmount));
    }

    private void Raise(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    private void EnsureOpenForApplication()
    {
        if (LifecycleStatus == PayableStatus.Cancelled)
            throw new PayableNotOpenForApplicationException("Cannot apply a payment or write-off to a cancelled payable.");
        if (LifecycleStatus == PayableStatus.Settled)
            throw new PayableNotOpenForApplicationException("Cannot apply a payment or write-off to an already-settled payable.");
        if (LifecycleStatus == PayableStatus.WrittenOff)
            throw new PayableNotOpenForApplicationException("Cannot apply a payment or write-off to a written-off payable.");
    }
}

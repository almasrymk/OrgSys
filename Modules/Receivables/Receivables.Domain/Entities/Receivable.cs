namespace Receivables.Domain;

using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// One AR open item — a customer's obligation created from a posted source document (currently:
/// a posted sales invoice, see SourceDocumentType). This is the subledger's own aggregate; it does
/// not post to or read the General Ledger itself (that stays Accounting's job via
/// Accounting.Contracts) and does not hold a Customer/Invoice navigation (CustomerId/
/// SourceDocumentId are external references only — see docs/architecture/receivables-ddd-migration.md
/// §6/§9).
/// </summary>
[Table("Receivable")]
public class Receivable : MovementModel
{
    private readonly List<IDomainEvent> _domainEvents = [];

    /// <summary>
    /// Receivable cannot inherit OrgSys.SharedKernel.AggregateRoot as well as MovementModel (C# has
    /// no multiple inheritance) and MovementModel must be kept so Receivable satisfies the existing
    /// generic IRepository&lt;TEntity&gt; where TEntity : BaseModel constraint (see
    /// docs/architecture/receivables-ddd-migration.md §6/§9 — reusing the generic repository rather
    /// than inventing a second persistence abstraction) — so domain events are tracked manually
    /// here, the same way Accounting.Domain.Journal does for the identical reason.
    /// </summary>
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void ClearDomainEvents() => _domainEvents.Clear();

    /// <summary>EF materialization constructor only. Business code creates a Receivable through
    /// <see cref="Create"/> — never via a bare object initializer, which would leave OutstandingAmount/
    /// LifecycleStatus in an unvalidated state.</summary>
    protected Receivable() { }

    public virtual long CustomerId { get; private set; }

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

    /// <summary>Named distinctly from the inherited BaseModel.Status (a generic New/Approved/Cancel/
    /// Reversed/... enum that doesn't represent Open/PartiallySettled/Settled/WrittenOff cleanly —
    /// unlike Journal, which reuses BaseModel.Status because its own states happen to map onto it).
    /// BaseModel.Status is left at its default and unused here.</summary>
    public virtual ReceivableStatus LifecycleStatus { get; private set; }

    /// <summary>Derived, not stored — see ReceivableStatus's remark on why "Overdue" is not a lifecycle state.</summary>
    public bool IsOverdue(DateTime asOfDate) => OutstandingAmount > 0 && DueDate.Date < asOfDate.Date;

    // ----- Construction -----

    public static Receivable Create(
        long customerId,
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
            throw new InvalidReceivableAmountException("A receivable's original amount must be greater than zero.");

        return new Receivable
        {
            CustomerId = customerId,
            SourceDocumentType = sourceDocumentType,
            SourceDocumentId = sourceDocumentId,
            SourceDocumentNumber = sourceDocumentNumber,
            DocumentDate = documentDate,
            DueDate = dueDate,
            CurrencyId = currencyId,
            Rate = rate,
            OriginalAmount = originalAmount,
            OutstandingAmount = originalAmount,
            LifecycleStatus = ReceivableStatus.Open,
            CreateUserId = createUserId,
            CreateDate = createDate,
            BranchId = branchId
        };
    }

    // ----- Settlement -----

    /// <summary>Applies a payment/credit allocation against this open item. Settles it once OutstandingAmount reaches zero.</summary>
    public void Apply(decimal amount)
    {
        if (amount <= 0)
            throw new InvalidAllocationAmountException("A payment allocation amount must be greater than zero.");

        EnsureOpenForApplication();

        if (amount > OutstandingAmount)
            throw new InvalidAllocationAmountException($"Allocation amount {amount} exceeds the outstanding amount {OutstandingAmount}.");

        OutstandingAmount -= amount;
        LifecycleStatus = OutstandingAmount == 0 ? ReceivableStatus.Settled : ReceivableStatus.PartiallySettled;

        Raise(new PaymentAppliedDomainEvent(Id, amount, OutstandingAmount));
        if (LifecycleStatus == ReceivableStatus.Settled)
            Raise(new ReceivableSettledDomainEvent(Id));
    }

    /// <summary>Restores a previously-applied amount (e.g. the underlying receipt was reversed). Never restores more than the original amount.</summary>
    public void Unapply(decimal amount)
    {
        if (amount <= 0)
            throw new InvalidAllocationAmountException("An unapply amount must be greater than zero.");

        if (LifecycleStatus is ReceivableStatus.Cancelled or ReceivableStatus.WrittenOff)
            throw new ReceivableNotOpenForApplicationException($"Cannot unapply a payment on a {LifecycleStatus} receivable.");

        var restored = OutstandingAmount + amount;
        if (restored > OriginalAmount)
            throw new InvalidAllocationAmountException($"Unapplying {amount} would restore the outstanding amount above the original amount {OriginalAmount}.");

        OutstandingAmount = restored;
        LifecycleStatus = OutstandingAmount == OriginalAmount ? ReceivableStatus.Open : ReceivableStatus.PartiallySettled;

        Raise(new PaymentUnappliedDomainEvent(Id, amount, OutstandingAmount));
    }

    // ----- Cancellation / write-off -----

    /// <summary>Draft-equivalent cancellation — only valid while nothing has been applied or written off yet. Idempotent if already Cancelled.</summary>
    public void Cancel()
    {
        if (LifecycleStatus == ReceivableStatus.Cancelled)
            return;

        if (OutstandingAmount != OriginalAmount)
            throw new ReceivableCannotBeCancelledException("A receivable with an applied payment or write-off cannot be cancelled — unapply/reverse first.");

        LifecycleStatus = ReceivableStatus.Cancelled;
        Raise(new ReceivableCancelledDomainEvent(Id));
    }

    /// <summary>Writes off part or all of the outstanding amount. A partial write-off leaves the receivable open for the remaining balance; LifecycleStatus only moves to WrittenOff once OutstandingAmount reaches zero.</summary>
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
            LifecycleStatus = ReceivableStatus.WrittenOff;

        Raise(new ReceivableWrittenOffDomainEvent(Id, amount, reason, OutstandingAmount));
    }

    private void Raise(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    private void EnsureOpenForApplication()
    {
        if (LifecycleStatus == ReceivableStatus.Cancelled)
            throw new ReceivableNotOpenForApplicationException("Cannot apply a payment or write-off to a cancelled receivable.");
        if (LifecycleStatus == ReceivableStatus.Settled)
            throw new ReceivableNotOpenForApplicationException("Cannot apply a payment or write-off to an already-settled receivable.");
        if (LifecycleStatus == ReceivableStatus.WrittenOff)
            throw new ReceivableNotOpenForApplicationException("Cannot apply a payment or write-off to a written-off receivable.");
    }
}

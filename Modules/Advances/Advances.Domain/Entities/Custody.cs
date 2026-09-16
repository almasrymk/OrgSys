namespace Advances.Domain;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// A Custody (عهدة) — a business obligation/accountability relationship with a holder, NOT a
/// FinancialAccount and NOT a CashBox/BankAccount (brief §27). Treasury owns money; Advances owns
/// responsibility for money entrusted to a person. This aggregate never touches Treasury's
/// FinancialAccount/Financial entities directly — MarkIssued/Return only record the id of the
/// Treasury Financial that Advances.Application obtained by sending a Treasury.Contracts command
/// (brief §33/§50/§61), same arm's-length reference shape Payables.Domain.Payable uses for
/// SupplierId/SourceDocumentId. Mirrors Payables.Domain.Payable's aggregate shape throughout
/// (private setters, protected EF ctor, static Create factory, manual domain-event list because
/// MovementModel can't also inherit OrgSys.SharedKernel.AggregateRoot).
/// </summary>
[Table("Custody")]
public class Custody : MovementModel
{
    private readonly List<IDomainEvent> _domainEvents = [];
    private readonly List<CustodyHandover> _handovers = [];

    [NotMapped]
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void ClearDomainEvents() => _domainEvents.Clear();

    /// <summary>Historical handover trail — see CustodyHandover. Cannot be replaced/cleared from
    /// outside; TransferHolder is the only way an entry is added.</summary>
    public virtual IReadOnlyCollection<CustodyHandover> Handovers => _handovers.AsReadOnly();

    /// <summary>EF materialization constructor only. Business code creates a Custody through
    /// <see cref="Create"/> — never via a bare object initializer, which would leave
    /// OutstandingAmount/LifecycleStatus in an unvalidated state.</summary>
    protected Custody() { }

    /// <summary>Current accountable holder. No Employee/HR navigation — a plain reference id
    /// (brief §31: Advances.Domain must not depend on HR.Domain). Changes only through TransferHolder,
    /// which also appends a Handovers entry — never assigned directly.</summary>
    public virtual long HolderId { get; private set; }

    [Required, StringLength(500)]
    public virtual string Purpose { get; private set; } = string.Empty;

    public virtual DateTime? DueDate { get; private set; }

    public virtual long CurrencyId { get; private set; }

    [Column(TypeName = "decimal(18,6)")]
    public virtual decimal Rate { get; private set; }

    /// <summary>The approved amount this custody was (or will be) issued for — brief §30's own field name.</summary>
    [Column(TypeName = "decimal(18,2)")]
    public virtual decimal IssuedAmount { get; private set; }

    [Column(TypeName = "decimal(18,2)")]
    public virtual decimal SettledAmount { get; private set; }

    [Column(TypeName = "decimal(18,2)")]
    public virtual decimal ReturnedAmount { get; private set; }

    /// <summary>IssuedAmount - SettledAmount - ReturnedAmount, maintained only by Settle/Return —
    /// never assigned directly, so it can't silently drift (brief §47: "Do not trust outstanding
    /// amount sent from UI").</summary>
    [Column(TypeName = "decimal(18,2)")]
    public virtual decimal OutstandingAmount { get; private set; }

    public virtual CustodyStatus LifecycleStatus { get; private set; }

    public virtual DateTime? IssueDate { get; private set; }

    /// <summary>The Treasury Financial that actually paid this custody out — reference id only, set
    /// once by MarkIssued. No navigation (brief §61: no cross-context EF navigation).</summary>
    public virtual long? IssuingFinancialTransactionId { get; private set; }

    /// <summary>The most recent Treasury Financial that received a returned amount — reference id
    /// only, set by Return. Reference id only, same reasoning as IssuingFinancialTransactionId.</summary>
    public virtual long? ReturnFinancialTransactionId { get; private set; }

    [StringLength(500)]
    public virtual string? Notes { get; private set; }

    /// <summary>Derived, never stored — mirrors Payables.Domain.Payable.IsOverdue's reasoning
    /// (changes purely with the passage of time, not because of any action taken on the custody).
    /// Only meaningful once real money is at stake.</summary>
    public bool IsOverdue(DateTime asOfDate) =>
        DueDate is not null
        && OutstandingAmount > 0
        && DueDate.Value.Date < asOfDate.Date
        && LifecycleStatus is CustodyStatus.Issued or CustodyStatus.PartiallySettled;

    // ----- Construction -----

    public static Custody Create(
        long holderId,
        string purpose,
        long currencyId,
        decimal rate,
        decimal issuedAmount,
        DateTime? dueDate,
        long createUserId,
        DateTime createDate,
        long? branchId = null,
        string? notes = null)
    {
        if (string.IsNullOrWhiteSpace(purpose))
            throw new CustodyPurposeRequiredException("A custody's purpose is required.");
        if (issuedAmount <= 0)
            throw new InvalidCustodyAmountException("A custody's issued amount must be greater than zero.");
        if (rate <= 0)
            throw new InvalidCustodyAmountException("A custody's exchange rate must be greater than zero.");

        var custody = new Custody
        {
            HolderId = holderId,
            Purpose = purpose,
            CurrencyId = currencyId,
            Rate = rate,
            IssuedAmount = issuedAmount,
            OutstandingAmount = issuedAmount,
            DueDate = dueDate,
            LifecycleStatus = CustodyStatus.Draft,
            CreateUserId = createUserId,
            CreateDate = createDate,
            Date = createDate,
            BranchId = branchId,
            Notes = notes
        };

        custody.Raise(new CustodyCreatedDomainEvent(custody.Id, holderId, issuedAmount));
        return custody;
    }

    // ----- Lifecycle -----

    /// <summary>Draft -> Approved.</summary>
    public void Approve()
    {
        if (LifecycleStatus != CustodyStatus.Draft)
            throw new CustodyNotApprovableException("Only a Draft custody can be approved.");

        LifecycleStatus = CustodyStatus.Approved;
        Raise(new CustodyApprovedDomainEvent(Id));
    }

    /// <summary>
    /// Approved -> Issued. The ONE place Custody records that money actually moved — the caller
    /// (Advances.Application) is responsible for first requesting the Treasury payment (via
    /// Treasury.Contracts, brief §33/§50) and passing back the resulting Financial's id here.
    /// Custody never creates or touches a Treasury FinancialAccount/Financial itself. Guarding on
    /// LifecycleStatus == Approved (not just "not yet Issued") is what makes a duplicate/retried
    /// issue request fail rather than double-issue (brief §52 idempotency, brief §74 "cannot issue twice").
    /// </summary>
    public void MarkIssued(long financialTransactionId, DateTime issueDate)
    {
        if (LifecycleStatus != CustodyStatus.Approved)
            throw new CustodyNotIssuableException("Only an Approved custody can be issued.");
        if (financialTransactionId <= 0)
            throw new InvalidCustodyAmountException("A valid Treasury financial transaction id is required to issue a custody.");

        IssuingFinancialTransactionId = financialTransactionId;
        IssueDate = issueDate;
        LifecycleStatus = CustodyStatus.Issued;

        Raise(new CustodyIssuedDomainEvent(Id, financialTransactionId, issueDate));
    }

    /// <summary>
    /// Applies an expense settlement against the outstanding amount (brief §35/§43) — the caller is
    /// responsible for recording the actual expense document(s); this only tracks the aggregate
    /// amount, same split of responsibility Payables.Domain.SupplierPaymentApplication uses between
    /// "recording the allocation" and "applying it to the open item". Settles the custody once
    /// OutstandingAmount reaches zero (brief §37: overspend is rejected here, not silently allowed —
    /// a caller wanting to cover an overspend must top up via a separate custody/claim, out of scope
    /// for this aggregate).
    /// </summary>
    public void Settle(decimal amount)
    {
        if (amount <= 0)
            throw new InvalidCustodySettlementAmountException("A settlement amount must be greater than zero.");

        EnsureOpenForSettlement();

        if (amount > OutstandingAmount)
            throw new InvalidCustodySettlementAmountException($"Settlement amount {amount} exceeds the outstanding amount {OutstandingAmount}.");

        SettledAmount += amount;
        OutstandingAmount -= amount;
        LifecycleStatus = OutstandingAmount == 0 ? CustodyStatus.Settled : CustodyStatus.PartiallySettled;

        Raise(new CustodySettlementPostedDomainEvent(Id, amount, OutstandingAmount));
    }

    /// <summary>
    /// Records that part (or all) of the outstanding amount was physically returned to Treasury
    /// (brief §36) — the caller is responsible for first getting Treasury to actually receive the
    /// cash and passing back the resulting Financial's id, same arm's-length shape as MarkIssued.
    /// </summary>
    public void Return(decimal amount, long returnFinancialTransactionId)
    {
        if (amount <= 0)
            throw new InvalidCustodySettlementAmountException("A return amount must be greater than zero.");
        if (returnFinancialTransactionId <= 0)
            throw new InvalidCustodyAmountException("A valid Treasury financial transaction id is required to record a custody return.");

        EnsureOpenForSettlement();

        if (amount > OutstandingAmount)
            throw new InvalidCustodySettlementAmountException($"Return amount {amount} exceeds the outstanding amount {OutstandingAmount}.");

        ReturnedAmount += amount;
        OutstandingAmount -= amount;
        ReturnFinancialTransactionId = returnFinancialTransactionId;
        LifecycleStatus = OutstandingAmount == 0 ? CustodyStatus.Settled : CustodyStatus.PartiallySettled;

        Raise(new CustodyAmountReturnedDomainEvent(Id, amount, returnFinancialTransactionId, OutstandingAmount));
    }

    /// <summary>Settled (zero outstanding) -> Closed. The one explicit archival step (brief §32) — never automatic.</summary>
    public void Close()
    {
        if (LifecycleStatus != CustodyStatus.Settled)
            throw new CustodyCannotBeClosedException("Only a fully Settled custody (zero outstanding) can be closed.");

        LifecycleStatus = CustodyStatus.Closed;
        Raise(new CustodyClosedDomainEvent(Id));
    }

    /// <summary>Draft -> Cancelled only — an Approved/Issued custody has (or is about to have) a real
    /// money movement and must be settled/returned instead, mirroring
    /// Payables.Domain.Payable.Cancel's "only if untouched" rule. Idempotent if already Cancelled.</summary>
    public void Cancel()
    {
        if (LifecycleStatus == CustodyStatus.Cancelled)
            return;

        if (LifecycleStatus != CustodyStatus.Draft)
            throw new CustodyCannotBeCancelledException("Only a Draft custody can be cancelled — an Approved/Issued custody must be settled/returned instead.");

        LifecycleStatus = CustodyStatus.Cancelled;
        Raise(new CustodyCancelledDomainEvent(Id));
    }

    /// <summary>
    /// Transfers the entire remaining custody to a new holder (brief §38-40) — always the full
    /// current OutstandingAmount, since partial handover is explicitly unsupported and must be
    /// rejected rather than left ambiguous (brief §40). Appends a CustodyHandover audit entry rather
    /// than merely overwriting HolderId (brief §38's core requirement).
    /// </summary>
    public void TransferHolder(long toHolderId, string reason, long approvedByUserId, DateTime transferDate)
    {
        if (LifecycleStatus is not (CustodyStatus.Issued or CustodyStatus.PartiallySettled))
            throw new InvalidCustodyTransferException("Only an Issued or PartiallySettled custody can be transferred to another holder.");
        if (toHolderId == HolderId)
            throw new InvalidCustodyTransferException("Cannot transfer a custody to its current holder.");
        if (string.IsNullOrWhiteSpace(reason))
            throw new InvalidCustodyTransferException("A transfer reason is required.");

        var fromHolderId = HolderId;
        var transferredAmount = OutstandingAmount;

        _handovers.Add(new CustodyHandover(fromHolderId, toHolderId, transferDate, transferredAmount, reason, approvedByUserId) { CustodyId = Id });
        HolderId = toHolderId;

        Raise(new CustodyTransferredDomainEvent(Id, fromHolderId, toHolderId, transferredAmount));
    }

    private void Raise(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    private void EnsureOpenForSettlement()
    {
        if (LifecycleStatus is not (CustodyStatus.Issued or CustodyStatus.PartiallySettled))
            throw new CustodyNotOpenForSettlementException($"Cannot settle or return an amount on a custody that is {LifecycleStatus}.");
    }
}

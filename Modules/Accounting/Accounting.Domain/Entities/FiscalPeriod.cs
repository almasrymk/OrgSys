namespace Accounting.Domain
{
    using Accounting.Domain.Events;
    using Accounting.Domain.Exceptions;

    [Table("FiscalPeriod")]
    public class FiscalPeriod : BaseModel
    {
        private readonly List<IDomainEvent> _domainEvents = [];

        /// <summary>
        /// FiscalPeriod cannot inherit OrgSys.SharedKernel.AggregateRoot as well as BaseModel (C# has
        /// no multiple inheritance) and BaseModel must be kept to preserve the existing EF table
        /// shape — so this aggregate tracks its own domain events the same way AggregateRoot does.
        /// </summary>
        public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        public void ClearDomainEvents() => _domainEvents.Clear();

        [ForeignKey(nameof(FiscalYear))]
        public long FiscalYearId { get; set; }

        public virtual FiscalYear FiscalYear { get; set; } = default!;

        public int PeriodNumber { get; set; }

        public string Name { get; set; } = default!;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        /// <summary>
        /// Kept as a normal public setter (not private) rather than fully locked down: the existing
        /// FiscalYear "edit with nested Periods" endpoint (UpdateFiscalYearCommand, consumed by the
        /// Angular bulk-edit screen — see docs/dependency-rules.md-style note in
        /// FiscalYears/Commands/UpdateCommandHandler.cs) maps a whole FiscalPeriodDto list onto
        /// fresh FiscalPeriod instances via AutoMapper and must keep working unchanged for backward
        /// compatibility (migration brief §34). <see cref="Close"/>/<see cref="Reopen"/> are the
        /// domain-sanctioned entry points for the new CloseFiscalPeriodCommand/ReopenFiscalPeriodCommand
        /// and for <see cref="EnsureOpenForPosting"/>'s callers; anything reaching this setter
        /// directly bypasses those invariants and domain events, which is exactly why the new
        /// commands exist as the preferred path.
        /// </summary>
        public FiscalPeriodStatus FiscalPeriodStatus { get; set; }

        /// <summary>Closes the period so no journal may be posted into it going forward. Already-posted journals are unaffected — closing is forward-looking only.</summary>
        public void Close()
        {
            if (FiscalPeriodStatus == FiscalPeriodStatus.Closed)
                return;

            FiscalPeriodStatus = FiscalPeriodStatus.Closed;
            _domainEvents.Add(new FiscalPeriodClosedDomainEvent(Id, FiscalYearId));
        }

        /// <summary>Reopens a previously closed period so posting can resume. A Locked period is a
        /// stronger state (used once a period has been reconciled/reported on) and is not reopened
        /// by this method — that distinction already existed in FiscalPeriodStatus before this
        /// migration and is preserved as-is.</summary>
        public void Reopen()
        {
            if (FiscalPeriodStatus == FiscalPeriodStatus.Open)
                return;

            if (FiscalPeriodStatus == FiscalPeriodStatus.Locked)
                throw new AccountingPeriodClosedException($"Fiscal period {Name} is locked and cannot be reopened.");

            FiscalPeriodStatus = FiscalPeriodStatus.Open;
            _domainEvents.Add(new FiscalPeriodReopenedDomainEvent(Id, FiscalYearId));
        }

        /// <summary>Final-authority check used by <see cref="Journal.Post"/> — a Journal must never be posted into a Closed or Locked period.</summary>
        public void EnsureOpenForPosting()
        {
            if (FiscalPeriodStatus == FiscalPeriodStatus.Closed)
                throw new AccountingPeriodClosedException($"Fiscal period {Name} is closed. Journal entries cannot be created or posted.");

            if (FiscalPeriodStatus == FiscalPeriodStatus.Locked)
                throw new AccountingPeriodClosedException($"Fiscal period {Name} is locked. Journal entries cannot be created or posted.");
        }
    }
}

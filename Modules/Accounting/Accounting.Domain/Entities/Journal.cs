namespace Accounting.Domain
{
    using Accounting.Domain.Events;
    using Accounting.Domain.Exceptions;

    [Table("Journal")]
    public class Journal : MovementModel
    {
        private readonly List<IDomainEvent> _domainEvents = [];

        /// <summary>
        /// Journal cannot inherit OrgSys.SharedKernel.AggregateRoot as well as MovementModel (C# has
        /// no multiple inheritance) and MovementModel must be kept to preserve the existing EF table
        /// shape (see docs referenced from MovementModel.cs) — so this aggregate tracks its own
        /// domain events the same way AggregateRoot does. Reversal is the one exception: the
        /// reversing Journal's Id does not exist yet at the point CreateReversal runs (it is
        /// DB-generated on SaveChanges), so JournalReversedDomainEvent is raised by the Application
        /// handler after the save succeeds, not collected here — see ReverseJournalCommandHandler.
        /// </summary>
        public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        public void ClearDomainEvents() => _domainEvents.Clear();

        [ForeignKey("JournalType")]
        public virtual long JournalTypeId { get; set; }

        public virtual JournalType? JournalType { get; set; }

        [ForeignKey("Currency")]
        public virtual long CurrencyId { get; set; }

        public virtual Currency? Currency { get; set; }

        public virtual decimal Rate { get; set; }

        [ForeignKey("FiscalYear")]
        public virtual long FiscalYearId { get; set; }

        public virtual FiscalYear? FiscalYear { get; set; }

        [ForeignKey("FiscalPeriod")]
        public virtual long FiscalPeriodId { get; set; }

        public virtual FiscalPeriod? FiscalPeriod { get; set; }

        public virtual long RefranceId { get; set; }

        public virtual string? RefranceCode { get; set; }

        public virtual long RefranceTypeId { get; set; }

        /// <summary>Non-empty when this journal is owned/controlled by another module's document
        /// (e.g. an Invoice or inventory Transaction posting bridge) rather than created directly
        /// against the ledger — such a journal cannot be Posted/Cancelled/Reversed/edited/deleted
        /// through the direct Journal commands; only the owning module (via Accounting.Contracts)
        /// may touch it. See EnsureNotControlledByAnotherResource / SyncStatusFromSourceDocument.</summary>
        public virtual string? RefranceTable { get; set; }

        public virtual string? Note { get; set; }

        /// <summary>
        /// Stays a normal EF-mapped public-setter collection — not a private backing field exposed
        /// as IReadOnlyCollection — because several other bounded contexts (Treasury, and this
        /// module's own Postings integration handlers) construct a Journal in one shot via object
        /// initializer (`new Journal { JournalItems = [...] }`), a pre-existing, already-documented
        /// cross-module pattern (see ModuleLayerDependencyTests' AcceptedApplicationDomainExceptions
        /// and docs on Accounting.Contracts.Postings) that this phase does not redesign — see the
        /// GeneralLedger migration report's "Remaining Technical Debt" section. Within this module,
        /// AddLine/UpdateLine/RemoveLine are the sanctioned way to mutate lines once a Journal already
        /// exists — enforced by JournalAggregateEncapsulationTests (Architecture.Tests).
        /// </summary>
        public virtual ICollection<JournalItem> JournalItems { get; set; } = new List<JournalItem>();

        /// <summary>Set only on a reversing entry — the Posted journal it reverses.</summary>
        [ForeignKey("OriginalJournal")]
        public virtual long? OriginalJournalId { get; set; }

        public virtual Journal? OriginalJournal { get; set; }

        /// <summary>Inverse of <see cref="OriginalJournal"/> — set only on the original once reversed. No own column.</summary>
        public virtual Journal? ReversalJournal { get; set; }

        public decimal TotalDebit => JournalItems.Where(i => i.IsValid).Sum(i => i.Debit);

        public decimal TotalCredit => JournalItems.Where(i => i.IsValid).Sum(i => i.Credit);

        /// <summary>Double-entry is satisfied only when there is at least one valid (single-sided,
        /// non-zero) line and the valid lines balance — a journal made entirely of 0/0 lines must
        /// never read as "balanced" just because 0 == 0.</summary>
        public bool IsBalanced => JournalItems.Any(i => i.IsValid) && TotalDebit == TotalCredit;

        // ----- Line mutation (Draft only) -----

        public JournalItem AddLine(Account account, decimal debit, decimal credit, string? note = null)
        {
            EnsureEditable();
            account.EnsurePostable();

            var line = new JournalItem
            {
                JournalId = Id,
                AccountId = account.Id,
                Debit = debit,
                Credit = credit,
                Note = note
            };
            JournalItems.Add(line);
            return line;
        }

        public void UpdateLine(long lineId, Account account, decimal debit, decimal credit, string? note = null)
        {
            EnsureEditable();
            account.EnsurePostable();

            var line = JournalItems.FirstOrDefault(i => i.Id == lineId)
                ?? throw new InvalidOperationException($"Journal line {lineId} does not belong to journal {Code}.");

            line.AccountId = account.Id;
            line.Debit = debit;
            line.Credit = credit;
            line.Note = note;
        }

        public void RemoveLine(long lineId)
        {
            EnsureEditable();

            var line = JournalItems.FirstOrDefault(i => i.Id == lineId);
            if (line is not null)
                JournalItems.Remove(line);
        }

        // ----- Lifecycle -----

        /// <summary>
        /// Draft → Posted. Requires: not already Posted, not owned by another module's resource,
        /// at least one valid (single-sided) line, Debit == Credit across valid lines, every
        /// referenced Account postable/active, and the resolved FiscalYear/FiscalPeriod open.
        /// Opening-Balance duplicate-per-year validation is a cross-aggregate check (it must query
        /// every OTHER journal) and stays in Accounting.Application's IAccountingPeriodService,
        /// invoked by the command handler before calling Post — see the migration report.
        /// </summary>
        public void Post(FiscalYear fiscalYear, FiscalPeriod fiscalPeriod, IReadOnlyCollection<Account> referencedAccounts)
        {
            EnsureNotControlledByAnotherResource();

            if (Posted)
                throw new JournalAlreadyPostedException("Journal entry is already posted.");

            if (!IsBalanced)
                throw new JournalNotBalancedException("Total Debit must equal total Credit before posting.");

            foreach (var line in JournalItems.Where(i => i.IsValid))
            {
                var account = referencedAccounts.FirstOrDefault(a => a.Id == line.AccountId)
                    ?? throw new AccountNotPostableException($"Account {line.AccountId} referenced by journal line could not be resolved for posting.");
                account.EnsurePostable();
            }

            fiscalYear.EnsureOpenForPosting();
            fiscalPeriod.EnsureOpenForPosting();

            FiscalYearId = fiscalYear.Id;
            FiscalPeriodId = fiscalPeriod.Id;
            Posted = true;

            _domainEvents.Add(new JournalPostedDomainEvent(Id, FiscalYearId, FiscalPeriodId));
        }

        /// <summary>
        /// Books a proper accounting reversal: this (original) journal's lines are never mutated —
        /// a brand-new Posted journal is returned with every line's Debit/Credit swapped, the two
        /// are linked (OriginalJournalId), and this journal's Status flips to Reversed. The caller
        /// (ReverseJournalCommandHandler) is responsible for persisting the returned journal and
        /// resolving reversalCodeNumber/fiscalYear/fiscalPeriod beforehand — those require repository
        /// queries that don't belong on the aggregate.
        /// </summary>
        public Journal CreateReversal(long reversalCodeNumber, DateTime reversalDate, FiscalYear fiscalYear, FiscalPeriod fiscalPeriod)
        {
            EnsureNotControlledByAnotherResource();

            if (!Posted)
                throw new JournalCannotBeReversedException("Only a Posted journal entry can be reversed.");

            if (Status == Status.Reversed || ReversalJournal is not null)
                throw new JournalCannotBeReversedException("This journal entry has already been reversed.");

            if (JournalItems.Count == 0)
                throw new JournalCannotBeReversedException("Journal entry has no lines to reverse.");

            fiscalYear.EnsureOpenForPosting();
            fiscalPeriod.EnsureOpenForPosting();

            var reversal = new Journal
            {
                JournalTypeId = JournalTypeId,
                CurrencyId = CurrencyId,
                Rate = Rate,
                Date = reversalDate,
                FiscalYearId = fiscalYear.Id,
                FiscalPeriodId = fiscalPeriod.Id,
                Note = $"Reversal of Journal Entry {Code}",
                TypeId = TypeId,
                ParentId = ParentId,
                CodeNumber = reversalCodeNumber,
                Code = reversalCodeNumber.ToString(),
                CreateUserId = CreateUserId,
                CreateDate = DateTime.Now,
                Posted = true,
                Status = Status.New,
                OriginalJournalId = Id,
                JournalItems = JournalItems.Select(line => new JournalItem
                {
                    AccountId = line.AccountId,
                    // The whole point of a reversal: swap Debit and Credit on every line.
                    Debit = line.Credit,
                    Credit = line.Debit,
                    Note = line.Note,
                    Status = Status.New
                }).ToList()
            };

            Status = Status.Reversed;
            return reversal;
        }

        /// <summary>Draft → Cancelled. A Posted journal's accounting effect must never be voided this
        /// way — see CreateReversal. Idempotent if already Cancelled (matches pre-migration behavior).</summary>
        public void Cancel()
        {
            EnsureNotControlledByAnotherResource();

            if (Posted)
                throw new JournalCannotBeCancelledException("A posted journal entry cannot be cancelled. Use Reverse instead.");

            if (Status == Status.Cancel)
                return;

            Status = Status.Cancel;
            _domainEvents.Add(new JournalCancelledDomainEvent(Id));
        }

        /// <summary>Cancelled → Draft. Never valid from Posted or Reversed — those are permanent
        /// accounting history. No-ops if not currently Cancelled (matches pre-migration behavior).</summary>
        public void Redo()
        {
            EnsureNotControlledByAnotherResource();

            if (Posted || Status == Status.Reversed)
                throw new JournalCannotBeRedoneException("A posted or reversed journal entry cannot be redone.");

            if (Status != Status.Cancel)
                return;

            Status = Status.New;
        }

        /// <summary>
        /// The one sanctioned way another module's Postings integration (Accounting.Contracts.
        /// Postings.SetAccountingDocumentJournalStatusCommand) may change a resource-controlled
        /// journal's Status — deliberately bypasses EnsureNotControlledByAnotherResource, because
        /// that guard exists to stop the *direct* Journal commands from touching a resource-owned
        /// journal, not to stop the owning resource itself. Mirrors the pre-migration
        /// SetStatusByInvoiceIdAsync/SetStatusByTransactionIdAsync bridges exactly (no extra guards
        /// — those bridges applied the status unconditionally too).
        /// </summary>
        public void SyncStatusFromSourceDocument(Status status) => Status = status;

        /// <summary>
        /// Replaces every line on a resource-controlled journal (RefranceTable is set) in one shot —
        /// the one legal way Accounting.Contracts.Postings.PostAccountingDocumentCommandHandler may
        /// set/update lines on a journal owned by another module's source document. Deliberately
        /// bypasses the "not controlled by another resource" guard AddLine/UpdateLine/RemoveLine
        /// enforce, since this IS that resource acting on its own journal — mirrors the
        /// pre-migration integration bridges, which likewise replaced JournalItems wholesale on
        /// every sync (see the GeneralLedger migration report). Still enforces that every account
        /// is postable — the one gap those bridges never actually checked.
        /// </summary>
        public void ReplaceLinesFromSourceDocument(IReadOnlyCollection<(Account Account, decimal Debit, decimal Credit, string? Note)> lines)
        {
            foreach (var line in lines)
                line.Account.EnsurePostable();

            JournalItems.Clear();
            foreach (var line in lines)
                JournalItems.Add(new JournalItem { AccountId = line.Account.Id, Debit = line.Debit, Credit = line.Credit, Note = line.Note });
        }

        /// <summary>Same rule as line editing (only an unposted, non-resource-controlled Draft may be
        /// removed outright) but with the delete-specific wording the pre-migration handlers used.</summary>
        public void EnsureDeletable()
        {
            EnsureNotControlledByAnotherResource();

            if (Posted)
                throw new JournalAlreadyPostedException("A posted journal entry cannot be deleted. Use Reverse instead.");
        }

        /// <summary>Validates deletability and clears all lines in one step — the sanctioned way
        /// DeleteCommandHandler/DeleteListCommandHandler remove a Draft journal's lines before the
        /// base handler hard-deletes the Journal row itself. Wholesale removal (not a per-line
        /// operation) is exactly what is happening here, so clearing the whole collection at once —
        /// rather than one RemoveLine call per line — is the correct aggregate-owned operation, not
        /// a bypass of it.</summary>
        public void PrepareForDeletion()
        {
            EnsureDeletable();
            JournalItems.Clear();
        }

        private void EnsureEditable()
        {
            EnsureNotControlledByAnotherResource();

            if (Posted)
                throw new JournalAlreadyPostedException("A posted journal entry cannot be edited. Use Reverse instead.");
        }

        private void EnsureNotControlledByAnotherResource()
        {
            if (!string.IsNullOrEmpty(RefranceTable))
                throw new JournalControlledByResourceException("A journal created from a resource is controlled by that resource.");
        }
    }
}

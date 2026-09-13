namespace Accounting.Domain
{
    using Accounting.Domain.Events;
    using Accounting.Domain.Exceptions;

    [Table("Journal")]
    public class Journal : MovementModel
    {
        private readonly List<IDomainEvent> _domainEvents = [];
        private readonly List<JournalItem> _journalItems = [];

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

        /// <summary>EF materialization constructor only. Business code creates a Journal through
        /// <see cref="CreateDraft"/> or <see cref="CreateForSourceDocument"/> — never via a bare
        /// object initializer, which would leave header/lifecycle fields in an unvalidated state.</summary>
        protected Journal() { }

        public virtual long JournalTypeId { get; private set; }

        public virtual JournalType? JournalType { get; set; }

        /// <summary>
        /// Currency identity only — Accounting.Domain must not reference MasterData.Domain (see the
        /// Accounting DDD cleanup report). Accounting.Application resolves/validates the Currency
        /// this id refers to through MasterData.Contracts.Currencies (GetDefaultCurrencyQuery /
        /// GetCurrencyNamesQuery), never through a MasterData.Domain.Currency navigation.
        /// </summary>
        public virtual long CurrencyId { get; private set; }

        public virtual decimal Rate { get; private set; }

        [ForeignKey("FiscalYear")]
        public virtual long FiscalYearId { get; private set; }

        public virtual FiscalYear? FiscalYear { get; set; }

        [ForeignKey("FiscalPeriod")]
        public virtual long FiscalPeriodId { get; private set; }

        public virtual FiscalPeriod? FiscalPeriod { get; set; }

        public virtual long RefranceId { get; private set; }

        public virtual string? RefranceCode { get; private set; }

        public virtual long RefranceTypeId { get; private set; }

        /// <summary>Non-empty when this journal is owned/controlled by another module's document
        /// (e.g. an Invoice or inventory Transaction posting bridge) rather than created directly
        /// against the ledger — such a journal cannot be Posted/Cancelled/Reversed/edited/deleted
        /// through the direct Journal commands; only the owning module (via Accounting.Contracts)
        /// may touch it. See EnsureNotControlledByAnotherResource / SyncStatusFromSourceDocument.</summary>
        public virtual string? RefranceTable { get; private set; }

        public virtual string? Note { get; private set; }

        /// <summary>
        /// Private backing field, exposed read-only — the collection cannot be replaced, cleared, or
        /// added/removed to from outside Journal. AddLine/UpdateLine/RemoveLine/
        /// ReplaceLinesFromSourceDocument are the only ways lines change; there is no
        /// IJournalItemRepository and JournalItem has no public constructor (see the Accounting DDD
        /// cleanup report and JournalAggregateEncapsulationTests in Architecture.Tests, which proves
        /// this at the repo-scan level, and JournalEncapsulationTests in Accounting.Domain.Tests,
        /// which proves it at compile time).
        /// </summary>
        public virtual IReadOnlyCollection<JournalItem> JournalItems => _journalItems.AsReadOnly();

        /// <summary>Set only on a reversing entry — the Posted journal it reverses.</summary>
        [ForeignKey("OriginalJournal")]
        public virtual long? OriginalJournalId { get; private set; }

        public virtual Journal? OriginalJournal { get; set; }

        /// <summary>Inverse of <see cref="OriginalJournal"/> — set only on the original once reversed. No own column.</summary>
        public virtual Journal? ReversalJournal { get; set; }

        public decimal TotalDebit => _journalItems.Where(i => i.IsValid).Sum(i => i.Debit);

        public decimal TotalCredit => _journalItems.Where(i => i.IsValid).Sum(i => i.Credit);

        /// <summary>Double-entry is satisfied only when there is at least one valid (single-sided,
        /// non-zero) line and the valid lines balance — a journal made entirely of 0/0 lines must
        /// never read as "balanced" just because 0 == 0.</summary>
        public bool IsBalanced => _journalItems.Any(i => i.IsValid) && TotalDebit == TotalCredit;

        // ----- Construction -----

        /// <summary>Creates a new, empty Draft journal entered directly against the ledger (the
        /// "manual Journal" screen) — lines are added afterward via AddLine.</summary>
        public static Journal CreateDraft(
            long journalTypeId, long typeId, long codeNumber, string? code, DateTime date,
            long createUserId, DateTime createDate, long? branchId, long? shiftId,
            long currencyId, decimal rate, string? note)
        {
            return new Journal
            {
                JournalTypeId = journalTypeId,
                TypeId = typeId,
                CodeNumber = codeNumber,
                Code = code,
                Date = date,
                CreateUserId = createUserId,
                CreateDate = createDate,
                BranchId = branchId,
                ShiftId = shiftId,
                CurrencyId = currencyId,
                Rate = rate,
                Note = note,
                Status = Status.New,
                Posted = false
            };
        }

        /// <summary>Creates a new Draft journal owned by another module's source document (the
        /// Accounting.Contracts.Postings bridge) — resource-controlled from the moment it exists, so
        /// AddLine/UpdateLine/RemoveLine/Post/Cancel/Reverse/Redo never apply to it; only
        /// ReplaceLinesFromSourceDocument and SyncStatusFromSourceDocument do.</summary>
        public static Journal CreateForSourceDocument(
            string referenceTable, long sourceDocumentId, long sourceDocumentTypeId, string? sourceDocumentCode,
            long journalTypeId, long codeNumber, DateTime date, long createUserId, DateTime createDate,
            long? branchId, long? shiftId, long currencyId, decimal rate, string? note)
        {
            return new Journal
            {
                RefranceTable = referenceTable,
                RefranceId = sourceDocumentId,
                RefranceTypeId = sourceDocumentTypeId,
                RefranceCode = sourceDocumentCode,
                JournalTypeId = journalTypeId,
                TypeId = journalTypeId,
                CodeNumber = codeNumber,
                Code = codeNumber.ToString(),
                Date = date,
                CreateUserId = createUserId,
                CreateDate = createDate,
                BranchId = branchId,
                ShiftId = shiftId,
                CurrencyId = currencyId,
                Rate = rate,
                Note = note,
                Status = Status.New,
                Posted = false
            };
        }

        /// <summary>Assigns the fiscal year/period a journal date resolves to. Resolution itself
        /// requires querying every FiscalYear/FiscalPeriod and stays in Accounting.Application's
        /// IAccountingPeriodService (a cross-aggregate concern); this method only records the result,
        /// so FiscalYearId/FiscalPeriodId are never set except through it or Post/CreateReversal.</summary>
        public void AssignFiscalPeriod(FiscalYear fiscalYear, FiscalPeriod fiscalPeriod)
        {
            FiscalYearId = fiscalYear.Id;
            FiscalPeriodId = fiscalPeriod.Id;
        }

        /// <summary>Updates the header (non-lifecycle) fields of a Draft journal — the same fields
        /// CreateDraft accepts. Guarded the same way line edits are.</summary>
        public void UpdateHeader(long journalTypeId, DateTime date, long currencyId, decimal rate, string? note, long? branchId, long? shiftId)
        {
            EnsureEditable();

            JournalTypeId = journalTypeId;
            Date = date;
            CurrencyId = currencyId;
            Rate = rate;
            Note = note;
            BranchId = branchId;
            ShiftId = shiftId;
        }

        // ----- Line mutation (Draft only) -----

        public JournalItem AddLine(Account account, decimal debit, decimal credit, string? note = null)
        {
            EnsureEditable();
            account.EnsurePostable();

            var line = new JournalItem(account.Id, debit, credit, note) { JournalId = Id };
            _journalItems.Add(line);
            return line;
        }

        public void UpdateLine(long lineId, Account account, decimal debit, decimal credit, string? note = null)
        {
            EnsureEditable();
            account.EnsurePostable();

            var line = _journalItems.FirstOrDefault(i => i.Id == lineId)
                ?? throw new InvalidOperationException($"Journal line {lineId} does not belong to journal {Code}.");

            line.Update(account.Id, debit, credit, note);
        }

        public void RemoveLine(long lineId)
        {
            EnsureEditable();

            var line = _journalItems.FirstOrDefault(i => i.Id == lineId);
            if (line is not null)
                _journalItems.Remove(line);
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
            PostCore(fiscalYear, fiscalPeriod, referencedAccounts);
        }

        /// <summary>
        /// The one sanctioned way the Accounting.Contracts.Postings bridge may Post a
        /// resource-controlled journal immediately at creation — Treasury's Financial/
        /// FinancialTransfer transactions must become genuinely Posted the moment they exist
        /// (unlike Invoice/Transaction, which only track their source document's own Status over
        /// time via SyncStatusFromSourceDocument and never set Posted). Deliberately bypasses
        /// EnsureNotControlledByAnotherResource for the same reason SyncStatusFromSourceDocument/
        /// ReplaceLinesFromSourceDocument/UpdateHeaderFromSourceDocument do: this IS the owning
        /// resource acting on its own journal, not an outside caller reaching around it. Every other
        /// invariant (balanced, postable accounts, open fiscal period) still applies.
        /// </summary>
        public void PostForSourceDocument(FiscalYear fiscalYear, FiscalPeriod fiscalPeriod, IReadOnlyCollection<Account> referencedAccounts)
            => PostCore(fiscalYear, fiscalPeriod, referencedAccounts);

        private void PostCore(FiscalYear fiscalYear, FiscalPeriod fiscalPeriod, IReadOnlyCollection<Account> referencedAccounts)
        {
            if (Posted)
                throw new JournalAlreadyPostedException("Journal entry is already posted.");

            if (!IsBalanced)
                throw new JournalNotBalancedException("Total Debit must equal total Credit before posting.");

            foreach (var line in _journalItems.Where(i => i.IsValid))
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
            return CreateReversalCore(reversalCodeNumber, reversalDate, fiscalYear, fiscalPeriod, preserveSourceDocumentLink: false);
        }

        /// <summary>
        /// The one sanctioned way the Accounting.Contracts.Postings bridge may reverse a
        /// resource-controlled journal (ReverseAccountingDocumentJournalCommand, e.g. Treasury's
        /// Financial/FinancialTransfer). Deliberately bypasses EnsureNotControlledByAnotherResource
        /// for the same reason PostForSourceDocument does — this IS the owning resource acting on
        /// its own journal. Unlike CreateReversal, the RefranceTable/RefranceId/RefranceCode/
        /// RefranceTypeId link is carried onto the reversal too, so it stays exactly as
        /// resource-controlled as the original — unreachable through the generic Journal commands,
        /// matching the pre-migration integration bridges exactly.
        /// </summary>
        public Journal CreateReversalForSourceDocument(long reversalCodeNumber, DateTime reversalDate, FiscalYear fiscalYear, FiscalPeriod fiscalPeriod)
            => CreateReversalCore(reversalCodeNumber, reversalDate, fiscalYear, fiscalPeriod, preserveSourceDocumentLink: true);

        private Journal CreateReversalCore(long reversalCodeNumber, DateTime reversalDate, FiscalYear fiscalYear, FiscalPeriod fiscalPeriod, bool preserveSourceDocumentLink)
        {
            if (!Posted)
                throw new JournalCannotBeReversedException("Only a Posted journal entry can be reversed.");

            if (Status == Status.Reversed || ReversalJournal is not null)
                throw new JournalCannotBeReversedException("This journal entry has already been reversed.");

            if (_journalItems.Count == 0)
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
                RefranceTable = preserveSourceDocumentLink ? RefranceTable : null,
                RefranceId = preserveSourceDocumentLink ? RefranceId : 0,
                RefranceCode = preserveSourceDocumentLink ? RefranceCode : null,
                RefranceTypeId = preserveSourceDocumentLink ? RefranceTypeId : 0
            };

            // The whole point of a reversal: swap Debit and Credit on every line.
            foreach (var line in _journalItems)
                reversal._journalItems.Add(new JournalItem(line.AccountId, line.Credit, line.Debit, line.Note) { JournalId = reversal.Id });

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

            _journalItems.Clear();
            foreach (var line in lines)
                _journalItems.Add(new JournalItem(line.Account.Id, line.Debit, line.Credit, line.Note) { JournalId = Id });
        }

        /// <summary>
        /// Updates the header fields of an existing resource-controlled journal on every re-sync —
        /// the one legal way Accounting.Contracts.Postings.PostAccountingDocumentCommandHandler may
        /// update header fields on a journal owned by another module's source document. Does not
        /// go through EnsureEditable (this journal IS resource-controlled by design), mirroring
        /// ReplaceLinesFromSourceDocument.
        /// </summary>
        public void UpdateHeaderFromSourceDocument(DateTime date, DateTime? modifyDate, long? modifyUserId, long? branchId, long? shiftId, long currencyId, decimal rate, string? sourceDocumentCode, string? note)
        {
            Date = date;
            ModifyDate = modifyDate;
            ModifyUserId = modifyUserId;
            BranchId = branchId;
            ShiftId = shiftId;
            CurrencyId = currencyId;
            Rate = rate;
            RefranceCode = sourceDocumentCode;
            Note = note;
        }

        /// <summary>
        /// Upserts this journal's own line for <paramref name="accountId"/> and rebalances a shared
        /// clearing line — the one legal way a Receivables/Payables-style "one line per party inside
        /// a shared per-fiscal-year Opening Balance journal" integration may touch lines on a journal
        /// it does not exclusively own. Unlike ReplaceLinesFromSourceDocument this journal is NOT
        /// resource-controlled (RefranceTable stays empty — the manual Journal screen must still be
        /// able to show/edit it like any other Draft before it is posted), so the normal
        /// EnsureEditable guard still applies.
        /// </summary>
        public void SetOpeningBalanceLine(Account account, decimal debit, decimal credit, string? note, Account clearingAccount)
        {
            EnsureEditable();
            account.EnsurePostable();
            clearingAccount.EnsurePostable();

            var existing = _journalItems.FirstOrDefault(i => i.AccountId == account.Id);
            if (existing is not null)
                existing.Update(account.Id, debit, credit, note);
            else
                _journalItems.Add(new JournalItem(account.Id, debit, credit, note) { JournalId = Id });

            var otherLines = _journalItems.Where(i => i.AccountId != clearingAccount.Id).ToList();
            var net = otherLines.Sum(i => i.Debit) - otherLines.Sum(i => i.Credit);
            var clearingDebit = net < 0 ? -net : 0;
            var clearingCredit = net > 0 ? net : 0;

            var clearingLine = _journalItems.FirstOrDefault(i => i.AccountId == clearingAccount.Id);
            if (clearingLine is not null)
                clearingLine.Update(clearingAccount.Id, clearingDebit, clearingCredit, "Opening balance clearing");
            else if (clearingDebit != 0 || clearingCredit != 0)
                _journalItems.Add(new JournalItem(clearingAccount.Id, clearingDebit, clearingCredit, "Opening balance clearing") { JournalId = Id });
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
            _journalItems.Clear();
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

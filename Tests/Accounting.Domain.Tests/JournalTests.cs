namespace Accounting.Domain.Tests;

public class JournalTests
{
    private static Account PostableAccount(long id = 1) => new() { Id = id, Name = $"Account {id}", IsPostable = true };

    private static Account NonPostableAccount(long id = 1) => new() { Id = id, Name = $"Group {id}", IsPostable = false };

    private static FiscalYear OpenYear(long id = 10) => new()
    { Id = id, Name = "2026", StartDate = new DateTime(2026, 1, 1), EndDate = new DateTime(2026, 12, 31), FiscalYearStatus = FiscalYearStatus.Open };

    private static FiscalYear ClosedYear(long id = 10) => new()
    { Id = id, Name = "2026", StartDate = new DateTime(2026, 1, 1), EndDate = new DateTime(2026, 12, 31), FiscalYearStatus = FiscalYearStatus.Closed };

    private static FiscalPeriod OpenPeriod(long id = 20, long yearId = 10) => new()
    { Id = id, FiscalYearId = yearId, Name = "August 2026", PeriodNumber = 8, StartDate = new DateTime(2026, 8, 1), EndDate = new DateTime(2026, 8, 31), FiscalPeriodStatus = FiscalPeriodStatus.Open };

    private static FiscalPeriod ClosedPeriod(long id = 20, long yearId = 10) => new()
    { Id = id, FiscalYearId = yearId, Name = "August 2026", PeriodNumber = 8, StartDate = new DateTime(2026, 8, 1), EndDate = new DateTime(2026, 8, 31), FiscalPeriodStatus = FiscalPeriodStatus.Closed };

    private static Journal DraftJournal(long id = 1)
    {
        var journal = Journal.CreateDraft(
            journalTypeId: 2, typeId: 2, codeNumber: 1, code: "GJ-1", date: new DateTime(2026, 8, 17),
            createUserId: 1, createDate: new DateTime(2026, 8, 17), branchId: null, shiftId: null,
            currencyId: 1, rate: 1, note: null);
        journal.Id = id;
        return journal;
    }

    private static Journal BalancedDraftWithLines(long id = 1)
    {
        var journal = DraftJournal(id);
        // Explicit, distinct line Ids — mirrors what a real EF round-trip assigns; without this,
        // both freshly-added lines default to Id 0 like any other un-persisted BaseModel and become
        // indistinguishable by Id in these pure in-memory Domain tests.
        journal.AddLine(PostableAccount(101), 100, 0, "Cash").Id = 1;
        journal.AddLine(PostableAccount(102), 0, 100, "Capital").Id = 2;
        return journal;
    }

    /// <summary>A journal owned by another module's source document (RefranceTable set) — the only
    /// way to reach that state now that RefranceTable has a private setter (see Journal.
    /// CreateForSourceDocument). Has no lines; callers that need lines call
    /// ReplaceLinesFromSourceDocument themselves (the only legal way to add lines to one of these).</summary>
    private static Journal ResourceControlledDraftJournal(long id = 1)
    {
        var journal = Journal.CreateForSourceDocument(
            referenceTable: "invoice", sourceDocumentId: 1, sourceDocumentTypeId: 1, sourceDocumentCode: "INV-1",
            journalTypeId: 2, codeNumber: 1, date: new DateTime(2026, 8, 17), createUserId: 1,
            createDate: new DateTime(2026, 8, 17), branchId: null, shiftId: null, currencyId: 1, rate: 1, note: null);
        journal.Id = id;
        return journal;
    }

    // ----- Draft creation and line mutation -----

    [Fact]
    public void NewJournal_IsDraft_NotPostedNotBalanced()
    {
        var journal = DraftJournal();

        Assert.False(journal.Posted);
        Assert.Equal(Status.New, journal.Status);
        Assert.False(journal.IsBalanced);
        Assert.Empty(journal.JournalItems);
    }

    [Fact]
    public void AddLine_ToDraft_AddsLineAgainstPostableAccount()
    {
        var journal = DraftJournal();

        var line = journal.AddLine(PostableAccount(101), 100, 0, "Cash");

        Assert.Single(journal.JournalItems);
        Assert.Equal(101, line.AccountId);
        Assert.Equal(100, line.Debit);
    }

    [Fact]
    public void AddLine_ReferencingNonPostableAccount_Throws()
    {
        var journal = DraftJournal();

        Assert.Throws<AccountNotPostableException>(() => journal.AddLine(NonPostableAccount(), 100, 0));
    }

    [Fact]
    public void RemoveLine_FromDraft_RemovesIt()
    {
        var journal = BalancedDraftWithLines();
        var lineId = journal.JournalItems.First().Id;

        journal.RemoveLine(lineId);

        Assert.DoesNotContain(journal.JournalItems, i => i.Id == lineId);
    }

    [Fact]
    public void UpdateLine_OnDraft_ChangesAmountsAndAccount()
    {
        var journal = BalancedDraftWithLines();
        var line = journal.JournalItems.First();

        journal.UpdateLine(line.Id, PostableAccount(999), 250, 0, "Adjusted");

        Assert.Equal(999, line.AccountId);
        Assert.Equal(250, line.Debit);
        Assert.Equal("Adjusted", line.Note);
    }

    // ----- Posting invariants -----

    [Fact]
    public void Post_UnbalancedLines_ThrowsJournalNotBalanced()
    {
        var journal = DraftJournal();
        journal.AddLine(PostableAccount(101), 100, 0);
        journal.AddLine(PostableAccount(102), 0, 50);

        var ex = Assert.Throws<JournalNotBalancedException>(
            () => journal.Post(OpenYear(), OpenPeriod(), [PostableAccount(101), PostableAccount(102)]));

        Assert.Contains("Debit", ex.Message);
        Assert.False(journal.Posted);
    }

    [Fact]
    public void Post_NoLinesAtAll_ThrowsJournalNotBalanced_ZeroIsNotBalanced()
    {
        var journal = DraftJournal();

        Assert.Throws<JournalNotBalancedException>(() => journal.Post(OpenYear(), OpenPeriod(), []));
    }

    [Fact]
    public void Post_OnlyZeroValueLines_ThrowsJournalNotBalanced_ApparentZeroEqualsZeroIsRejected()
    {
        var journal = DraftJournal();
        journal.AddLine(PostableAccount(101), 0, 0);
        journal.AddLine(PostableAccount(102), 0, 0);

        Assert.Throws<JournalNotBalancedException>(
            () => journal.Post(OpenYear(), OpenPeriod(), [PostableAccount(101), PostableAccount(102)]));
    }

    [Fact]
    public void Post_LineReferencesNonPostableAccount_ThrowsAccountNotPostable()
    {
        var journal = BalancedDraftWithLines(); // lines reference accounts 101 and 102

        Assert.Throws<AccountNotPostableException>(
            () => journal.Post(OpenYear(), OpenPeriod(), [NonPostableAccount(101), PostableAccount(102)]));
    }

    [Fact]
    public void Post_IntoClosedFiscalPeriod_ThrowsAccountingPeriodClosed()
    {
        var journal = BalancedDraftWithLines();

        Assert.Throws<AccountingPeriodClosedException>(
            () => journal.Post(OpenYear(), ClosedPeriod(), [PostableAccount(101), PostableAccount(102)]));
    }

    [Fact]
    public void Post_IntoClosedFiscalYear_ThrowsAccountingPeriodClosed()
    {
        var journal = BalancedDraftWithLines();

        Assert.Throws<AccountingPeriodClosedException>(
            () => journal.Post(ClosedYear(), OpenPeriod(), [PostableAccount(101), PostableAccount(102)]));
    }

    [Fact]
    public void Post_BalancedDraftInOpenPeriod_Succeeds_AssignsPeriodAndRaisesDomainEvent()
    {
        var journal = BalancedDraftWithLines();
        var year = OpenYear();
        var period = OpenPeriod();

        journal.Post(year, period, [PostableAccount(101), PostableAccount(102)]);

        Assert.True(journal.Posted);
        Assert.Equal(year.Id, journal.FiscalYearId);
        Assert.Equal(period.Id, journal.FiscalPeriodId);
        var raised = Assert.Single(journal.DomainEvents.OfType<JournalPostedDomainEvent>());
        Assert.Equal(journal.Id, raised.JournalId);
    }

    [Fact]
    public void Post_AlreadyPosted_ThrowsJournalAlreadyPosted()
    {
        var journal = BalancedDraftWithLines();
        journal.Post(OpenYear(), OpenPeriod(), [PostableAccount(101), PostableAccount(102)]);

        Assert.Throws<JournalAlreadyPostedException>(
            () => journal.Post(OpenYear(), OpenPeriod(), [PostableAccount(101), PostableAccount(102)]));
    }

    [Fact]
    public void Post_ResourceControlledJournal_ThrowsJournalControlledByResource()
    {
        var journal = ResourceControlledDraftJournal();
        journal.ReplaceLinesFromSourceDocument([(PostableAccount(101), 100m, 0m, "Cash"), (PostableAccount(102), 0m, 100m, "Capital")]);

        Assert.Throws<JournalControlledByResourceException>(
            () => journal.Post(OpenYear(), OpenPeriod(), [PostableAccount(101), PostableAccount(102)]));
    }

    // ----- Posted immutability -----

    [Fact]
    public void PostedJournal_CannotHaveLinesAdded()
    {
        var journal = BalancedDraftWithLines();
        journal.Post(OpenYear(), OpenPeriod(), [PostableAccount(101), PostableAccount(102)]);

        Assert.Throws<JournalAlreadyPostedException>(() => journal.AddLine(PostableAccount(103), 10, 0));
    }

    [Fact]
    public void PostedJournal_CannotHaveLinesUpdatedOrRemoved()
    {
        var journal = BalancedDraftWithLines();
        var lineId = journal.JournalItems.First().Id;
        journal.Post(OpenYear(), OpenPeriod(), [PostableAccount(101), PostableAccount(102)]);

        Assert.Throws<JournalAlreadyPostedException>(() => journal.UpdateLine(lineId, PostableAccount(101), 200, 0));
        Assert.Throws<JournalAlreadyPostedException>(() => journal.RemoveLine(lineId));
    }

    [Fact]
    public void PostedJournal_CannotBeDeleted()
    {
        var journal = BalancedDraftWithLines();
        journal.Post(OpenYear(), OpenPeriod(), [PostableAccount(101), PostableAccount(102)]);

        Assert.Throws<JournalAlreadyPostedException>(() => journal.EnsureDeletable());
    }

    [Fact]
    public void DraftJournal_CanBeDeleted()
    {
        var journal = BalancedDraftWithLines();

        var ex = Record.Exception(() => journal.EnsureDeletable());

        Assert.Null(ex);
    }

    // ----- Reversal -----

    [Fact]
    public void CreateReversal_OfPostedJournal_ProducesSwappedLinesAndLinksBack()
    {
        var journal = BalancedDraftWithLines();
        journal.Post(OpenYear(), OpenPeriod(), [PostableAccount(101), PostableAccount(102)]);

        var reversal = journal.CreateReversal(2, new DateTime(2026, 8, 20), OpenYear(), OpenPeriod());

        Assert.True(reversal.Posted);
        Assert.Equal(journal.Id, reversal.OriginalJournalId);
        Assert.Equal(Status.Reversed, journal.Status);
        Assert.Equal(2, reversal.JournalItems.Count);

        var reversedCashLine = reversal.JournalItems.Single(i => i.AccountId == 101);
        Assert.Equal(0, reversedCashLine.Debit);
        Assert.Equal(100, reversedCashLine.Credit);

        var reversedCapitalLine = reversal.JournalItems.Single(i => i.AccountId == 102);
        Assert.Equal(100, reversedCapitalLine.Debit);
        Assert.Equal(0, reversedCapitalLine.Credit);
    }

    [Fact]
    public void CreateReversal_OriginalLinesAndHeaderAreUntouched()
    {
        var journal = BalancedDraftWithLines();
        journal.Post(OpenYear(), OpenPeriod(), [PostableAccount(101), PostableAccount(102)]);
        var originalDate = journal.Date;
        var originalDebit = journal.JournalItems.First().Debit;

        journal.CreateReversal(2, new DateTime(2026, 8, 20), OpenYear(), OpenPeriod());

        Assert.Equal(originalDate, journal.Date);
        Assert.Equal(originalDebit, journal.JournalItems.First().Debit);
        Assert.Equal(2, journal.JournalItems.Count);
    }

    [Fact]
    public void CreateReversal_OfDraftJournal_ThrowsJournalCannotBeReversed()
    {
        var journal = BalancedDraftWithLines(); // never Posted

        Assert.Throws<JournalCannotBeReversedException>(
            () => journal.CreateReversal(2, DateTime.Today, OpenYear(), OpenPeriod()));
    }

    [Fact]
    public void CreateReversal_Twice_ThrowsJournalCannotBeReversed_NoDuplicateReversal()
    {
        var journal = BalancedDraftWithLines();
        journal.Post(OpenYear(), OpenPeriod(), [PostableAccount(101), PostableAccount(102)]);
        journal.CreateReversal(2, new DateTime(2026, 8, 20), OpenYear(), OpenPeriod());

        Assert.Throws<JournalCannotBeReversedException>(
            () => journal.CreateReversal(3, new DateTime(2026, 8, 21), OpenYear(), OpenPeriod()));
    }

    [Fact]
    public void CreateReversal_WithNoLines_ThrowsJournalCannotBeReversed()
    {
        var journal = DraftJournal();
        journal.Posted = true; // simulate a posted-but-lineless state directly, bypassing Post()

        Assert.Throws<JournalCannotBeReversedException>(
            () => journal.CreateReversal(2, DateTime.Today, OpenYear(), OpenPeriod()));
    }

    // ----- Cancel / Redo -----

    [Fact]
    public void Cancel_DraftJournal_TransitionsToCancelAndRaisesEvent()
    {
        var journal = DraftJournal();

        journal.Cancel();

        Assert.Equal(Status.Cancel, journal.Status);
        Assert.Single(journal.DomainEvents.OfType<JournalCancelledDomainEvent>());
    }

    [Fact]
    public void Cancel_AlreadyCancelled_IsIdempotent_NoDuplicateEvent()
    {
        var journal = DraftJournal();
        journal.Cancel();
        journal.ClearDomainEvents();

        journal.Cancel();

        Assert.Equal(Status.Cancel, journal.Status);
        Assert.Empty(journal.DomainEvents);
    }

    [Fact]
    public void Cancel_PostedJournal_ThrowsJournalCannotBeCancelled_MustUseReverseInstead()
    {
        var journal = BalancedDraftWithLines();
        journal.Post(OpenYear(), OpenPeriod(), [PostableAccount(101), PostableAccount(102)]);

        var ex = Assert.Throws<JournalCannotBeCancelledException>(() => journal.Cancel());

        Assert.Contains("Reverse", ex.Message);
    }

    [Fact]
    public void Redo_CancelledJournal_ReopensToNew()
    {
        var journal = DraftJournal();
        journal.Cancel();

        journal.Redo();

        Assert.Equal(Status.New, journal.Status);
    }

    [Fact]
    public void Redo_PostedJournal_ThrowsJournalCannotBeRedone()
    {
        var journal = BalancedDraftWithLines();
        journal.Post(OpenYear(), OpenPeriod(), [PostableAccount(101), PostableAccount(102)]);
        journal.Status = Status.Cancel; // hypothetical inconsistent state — Posted must still win

        Assert.Throws<JournalCannotBeRedoneException>(() => journal.Redo());
    }

    [Fact]
    public void Redo_ReversedJournal_ThrowsJournalCannotBeRedone_HistoryStaysImmutable()
    {
        var journal = DraftJournal();
        journal.Status = Status.Reversed;

        Assert.Throws<JournalCannotBeRedoneException>(() => journal.Redo());
    }

    [Fact]
    public void Redo_JournalNotCancelled_IsNoOp()
    {
        var journal = DraftJournal();

        journal.Redo();

        Assert.Equal(Status.New, journal.Status);
    }

    // ----- Resource-controlled journals (owned by another module via Accounting.Contracts) -----

    [Fact]
    public void ResourceControlledJournal_CannotBeCancelledOrRedoneDirectly()
    {
        var journal = ResourceControlledDraftJournal();

        Assert.Throws<JournalControlledByResourceException>(() => journal.Cancel());
        Assert.Throws<JournalControlledByResourceException>(() => journal.Redo());
    }

    [Fact]
    public void SyncStatusFromSourceDocument_BypassesResourceControlGuard()
    {
        var journal = ResourceControlledDraftJournal();

        journal.SyncStatusFromSourceDocument(Status.Cancel);

        Assert.Equal(Status.Cancel, journal.Status);
    }

    [Fact]
    public void ReplaceLinesFromSourceDocument_OnResourceControlledJournal_ValidatesPostableAccounts()
    {
        var journal = ResourceControlledDraftJournal();

        Assert.Throws<AccountNotPostableException>(() =>
            journal.ReplaceLinesFromSourceDocument([(NonPostableAccount(), 100m, 0m, (string?)null)]));
    }

    [Fact]
    public void ReplaceLinesFromSourceDocument_ReplacesExistingLinesWholesale()
    {
        var journal = ResourceControlledDraftJournal();
        journal.ReplaceLinesFromSourceDocument([(PostableAccount(101), 100m, 0m, "Cash"), (PostableAccount(102), 0m, 100m, "Capital")]);

        journal.ReplaceLinesFromSourceDocument([(PostableAccount(999), 50m, 0m, "Replaced"), (PostableAccount(998), 0m, 50m, "Replaced")]);

        Assert.Equal(2, journal.JournalItems.Count);
        Assert.All(journal.JournalItems, i => Assert.Contains(i.AccountId, new long[] { 999, 998 }));
    }

    // ----- Resource-controlled posting/reversal (Treasury's Financial/FinancialTransfer bridge) -----

    [Fact]
    public void PostForSourceDocument_OnResourceControlledJournal_PostsSuccessfully_BypassingResourceGuard()
    {
        var journal = ResourceControlledDraftJournal();
        journal.ReplaceLinesFromSourceDocument([(PostableAccount(101), 100m, 0m, "Cash"), (PostableAccount(102), 0m, 100m, "Capital")]);

        journal.PostForSourceDocument(OpenYear(), OpenPeriod(), [PostableAccount(101), PostableAccount(102)]);

        Assert.True(journal.Posted);
        var raised = Assert.Single(journal.DomainEvents.OfType<JournalPostedDomainEvent>());
        Assert.Equal(journal.Id, raised.JournalId);
    }

    [Fact]
    public void PostForSourceDocument_Unbalanced_ThrowsJournalNotBalanced()
    {
        var journal = ResourceControlledDraftJournal();
        journal.ReplaceLinesFromSourceDocument([(PostableAccount(101), 100m, 0m, "Cash"), (PostableAccount(102), 0m, 50m, "Capital")]);

        Assert.Throws<JournalNotBalancedException>(
            () => journal.PostForSourceDocument(OpenYear(), OpenPeriod(), [PostableAccount(101), PostableAccount(102)]));
    }

    [Fact]
    public void CreateReversalForSourceDocument_OfPostedJournal_ProducesSwappedLinesAndPreservesResourceLink()
    {
        var journal = ResourceControlledDraftJournal();
        journal.ReplaceLinesFromSourceDocument([(PostableAccount(101), 100m, 0m, "Cash"), (PostableAccount(102), 0m, 100m, "Capital")]);
        journal.PostForSourceDocument(OpenYear(), OpenPeriod(), [PostableAccount(101), PostableAccount(102)]);

        var reversal = journal.CreateReversalForSourceDocument(2, new DateTime(2026, 8, 20), OpenYear(), OpenPeriod());

        Assert.True(reversal.Posted);
        Assert.Equal(journal.Id, reversal.OriginalJournalId);
        Assert.Equal(Status.Reversed, journal.Status);
        Assert.Equal("invoice", reversal.RefranceTable);
        Assert.Equal(journal.RefranceId, reversal.RefranceId);

        var reversedCashLine = reversal.JournalItems.Single(i => i.AccountId == 101);
        Assert.Equal(0, reversedCashLine.Debit);
        Assert.Equal(100, reversedCashLine.Credit);
    }

    [Fact]
    public void CreateReversalForSourceDocument_ReversalStaysResourceControlled_CannotBeCancelledDirectly()
    {
        var journal = ResourceControlledDraftJournal();
        journal.ReplaceLinesFromSourceDocument([(PostableAccount(101), 100m, 0m, "Cash"), (PostableAccount(102), 0m, 100m, "Capital")]);
        journal.PostForSourceDocument(OpenYear(), OpenPeriod(), [PostableAccount(101), PostableAccount(102)]);

        var reversal = journal.CreateReversalForSourceDocument(2, new DateTime(2026, 8, 20), OpenYear(), OpenPeriod());

        Assert.Throws<JournalControlledByResourceException>(() => reversal.Cancel());
    }

    [Fact]
    public void CreateReversal_DoesNotCarrySourceDocumentLink_UnlikeCreateReversalForSourceDocument()
    {
        var journal = BalancedDraftWithLines();
        journal.Post(OpenYear(), OpenPeriod(), [PostableAccount(101), PostableAccount(102)]);

        var reversal = journal.CreateReversal(2, new DateTime(2026, 8, 20), OpenYear(), OpenPeriod());

        Assert.Null(reversal.RefranceTable);
    }
}

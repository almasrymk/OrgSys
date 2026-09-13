using Accounting.Application.Journals.Commands;
using Accounting.Application;
using Accounting.Domain.Repositories;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System.Net;
using Xunit;

namespace Application.Tests;

public class JournalReverseCommandHandlerTests
{
    private static readonly DateTime Today = DateTime.Now.Date;

    private static FiscalYear OpenYear(long id = 10) => new()
    { Id = id, Name = "2026", StartDate = Today.AddMonths(-6), EndDate = Today.AddMonths(6), FiscalYearStatus = FiscalYearStatus.Open };

    private static FiscalPeriod OpenPeriod(long id = 20, long yearId = 10) => new()
    { Id = id, FiscalYearId = yearId, Name = "Current", PeriodNumber = 1, StartDate = Today.AddDays(-10), EndDate = Today.AddDays(10), FiscalPeriodStatus = FiscalPeriodStatus.Open };

    /// <summary>A Posted journal with two balanced lines — built through Journal.CreateDraft/AddLine
    /// (while still a Draft) and then flipped Posted directly (still a public-setter MovementModel
    /// field, unlike the header/line fields — see the Accounting DDD cleanup report), rather than
    /// through Journal.Post itself, so these Application-handler tests stay independent of Post's
    /// own invariants. Pass withLines:false for the "no lines to reverse" case.</summary>
    private static Journal PostedOriginal(long id = 1, bool withLines = true)
    {
        var journal = Journal.CreateDraft(
            journalTypeId: 2, typeId: 0, codeNumber: 100, code: "GJ-100", date: Today.AddDays(-30),
            createUserId: 1, createDate: Today.AddDays(-30), branchId: null, shiftId: null,
            currencyId: 1, rate: 1, note: null);
        journal.Id = id;
        journal.AssignFiscalPeriod(new FiscalYear { Id = 10 }, new FiscalPeriod { Id = 20 });

        if (withLines)
        {
            journal.AddLine(new Account { Id = 101, IsPostable = true }, 1000, 0, "Cash").Id = 1;
            journal.AddLine(new Account { Id = 102, IsPostable = true }, 0, 1000, "Capital").Id = 2;
        }

        journal.Posted = true;
        return journal;
    }

    /// <summary>A journal owned by another module's source document (RefranceTable set) — the only
    /// way to reach that state now that RefranceTable has a private setter.</summary>
    private static Journal ResourceControlledPostedOriginal(long id = 1)
    {
        var journal = Journal.CreateForSourceDocument(
            referenceTable: "invoice", sourceDocumentId: 1, sourceDocumentTypeId: 1, sourceDocumentCode: "INV-1",
            journalTypeId: 2, codeNumber: 100, date: Today.AddDays(-30), createUserId: 1,
            createDate: Today.AddDays(-30), branchId: null, shiftId: null, currencyId: 1, rate: 1, note: null);
        journal.Id = id;
        journal.Posted = true;
        return journal;
    }

    private static (ReverseJournalCommandHandler handler, Mock<IJournalRepository> repository, Mock<IUnitOfWork> unitOfWork, Mock<IAccountingPeriodService> accountingPeriodService) BuildHandler(Journal? existing)
    {
        var repository = new Mock<IJournalRepository>();
        repository.Setup(r => r.GetByIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(existing);
        repository.Setup(r => r.GetNextCodeNumberAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(100L);
        repository.Setup(r => r.AddAsync(It.IsAny<Journal>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.SaveChangeAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var accountingPeriodService = new Mock<IAccountingPeriodService>();

        var handler = new ReverseJournalCommandHandler(
            unitOfWork.Object, repository.Object, accountingPeriodService.Object,
            Mock.Of<IIntegrationEventPublisher>(), NullLogger<ReverseJournalCommandHandler>.Instance);

        return (handler, repository, unitOfWork, accountingPeriodService);
    }

    [Fact]
    public async Task Handle_PostedJournal_CreatesReversingEntryWithSwappedDebitCredit()
    {
        var original = PostedOriginal();
        var (handler, repository, unitOfWork, accountingPeriodService) = BuildHandler(original);
        var year = OpenYear();
        var period = OpenPeriod();
        accountingPeriodService.Setup(s => s.ResolveAndValidateAsync(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(AccountingPeriodResult.Ok(year, period));

        Journal? reversal = null;
        repository.Setup(r => r.AddAsync(It.IsAny<Journal>(), It.IsAny<CancellationToken>()))
            .Callback((Journal j, CancellationToken _) => reversal = j)
            .Returns(Task.CompletedTask);

        var result = await handler.Handle(new ReverseJournalCommand(original.Id), CancellationToken.None);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.NotNull(reversal);
        Assert.True(reversal!.Posted);
        Assert.Equal(original.Id, reversal.OriginalJournalId);
        Assert.Contains(original.Code!, reversal.Note);
        Assert.Equal(2, reversal.JournalItems.Count);

        var line1 = reversal.JournalItems.Single(i => i.AccountId == 101);
        Assert.Equal(0, line1.Debit);
        Assert.Equal(1000, line1.Credit);

        var line2 = reversal.JournalItems.Single(i => i.AccountId == 102);
        Assert.Equal(1000, line2.Debit);
        Assert.Equal(0, line2.Credit);

        Assert.Equal(reversal.JournalItems.Sum(i => i.Debit), reversal.JournalItems.Sum(i => i.Credit));
        Assert.Equal(Status.Reversed, original.Status);
        unitOfWork.Verify(u => u.CommitAsync(), Times.Once);
        unitOfWork.Verify(u => u.RollbackAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_OriginalLinesAndHeaderAreNeverModified()
    {
        var original = PostedOriginal();
        var originalDate = original.Date;
        var originalLine1Debit = original.JournalItems.First().Debit;
        var (handler, repository, unitOfWork, accountingPeriodService) = BuildHandler(original);
        accountingPeriodService.Setup(s => s.ResolveAndValidateAsync(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(AccountingPeriodResult.Ok(OpenYear(), OpenPeriod()));

        await handler.Handle(new ReverseJournalCommand(original.Id), CancellationToken.None);

        Assert.Equal(originalDate, original.Date);
        Assert.Equal(originalLine1Debit, original.JournalItems.First().Debit);
        Assert.Equal(2, original.JournalItems.Count);
    }

    [Fact]
    public async Task Handle_DraftJournal_RejectsReversal()
    {
        var draft = PostedOriginal();
        draft.Posted = false;
        var (handler, repository, unitOfWork, _) = BuildHandler(draft);

        var result = await handler.Handle(new ReverseJournalCommand(draft.Id), CancellationToken.None);

        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        repository.Verify(r => r.AddAsync(It.IsAny<Journal>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_AlreadyReversedJournal_RejectsDoubleReversal()
    {
        var original = PostedOriginal();
        original.Status = Status.Reversed;
        var (handler, repository, unitOfWork, _) = BuildHandler(original);

        var result = await handler.Handle(new ReverseJournalCommand(original.Id), CancellationToken.None);

        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        Assert.Contains("already been reversed", result.Errors!.Select(e => e.MessageError).First(), StringComparison.OrdinalIgnoreCase);
        repository.Verify(r => r.AddAsync(It.IsAny<Journal>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_JournalWithExistingReversalRelationship_RejectsDoubleReversalEvenIfStatusInconsistent()
    {
        // Belt-and-braces: Status somehow still New, but the relationship already shows a reversal exists.
        var original = PostedOriginal();
        original.Status = Status.New;
        var existingReversal = Journal.CreateDraft(2, 0, 101, "GJ-101", Today, 1, Today, null, null, 1, 1, null);
        existingReversal.Id = 99;
        original.ReversalJournal = existingReversal;
        var (handler, repository, unitOfWork, _) = BuildHandler(original);

        var result = await handler.Handle(new ReverseJournalCommand(original.Id), CancellationToken.None);

        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        repository.Verify(r => r.AddAsync(It.IsAny<Journal>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ResourceLinkedJournal_RejectsReversal()
    {
        var original = ResourceControlledPostedOriginal();
        var (handler, repository, unitOfWork, _) = BuildHandler(original);

        var result = await handler.Handle(new ReverseJournalCommand(original.Id), CancellationToken.None);

        Assert.Equal(HttpStatusCode.Forbidden, result.StatusCode);
    }

    [Fact]
    public async Task Handle_CurrentPeriodClosed_RollsBackAndRejects()
    {
        var original = PostedOriginal();
        var (handler, repository, unitOfWork, accountingPeriodService) = BuildHandler(original);
        accountingPeriodService.Setup(s => s.ResolveAndValidateAsync(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(AccountingPeriodResult.Fail("Fiscal period Current is closed. Journal entries cannot be created or posted."));

        var result = await handler.Handle(new ReverseJournalCommand(original.Id), CancellationToken.None);

        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        Assert.Equal(Status.New, original.Status);
        repository.Verify(r => r.AddAsync(It.IsAny<Journal>(), It.IsAny<CancellationToken>()), Times.Never);
        unitOfWork.Verify(u => u.RollbackAsync(), Times.Once);
        unitOfWork.Verify(u => u.CommitAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_JournalNotFound_ReturnsNotFound()
    {
        var (handler, repository, unitOfWork, _) = BuildHandler(null);

        var result = await handler.Handle(new ReverseJournalCommand(999), CancellationToken.None);

        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
    }

    [Fact]
    public async Task Handle_NoLines_RejectsReversal()
    {
        var original = PostedOriginal(withLines: false);
        var (handler, repository, unitOfWork, _) = BuildHandler(original);

        var result = await handler.Handle(new ReverseJournalCommand(original.Id), CancellationToken.None);

        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
    }
}

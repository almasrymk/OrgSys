using Accounting.Application.Journals.Commands;
using Accounting.Application;
using Accounting.Domain.Repositories;
using Moq;
using System.Net;
using Xunit;

namespace Application.Tests;

public class JournalUpdateCommandHandlerTests
{
    private static readonly DateTime OriginalDate = new(2026, 8, 17);
    private static readonly DateTime AnotherOpenDate = new(2026, 9, 15);

    private static FiscalYear Year(long id = 10) => new() { Id = id, Name = "2026", StartDate = new DateTime(2026, 1, 1), EndDate = new DateTime(2026, 12, 31), FiscalYearStatus = FiscalYearStatus.Open };

    private static FiscalPeriod Period(long id = 20, long yearId = 10) => new() { Id = id, FiscalYearId = yearId, Name = "August 2026", PeriodNumber = 8, StartDate = new DateTime(2026, 8, 1), EndDate = new DateTime(2026, 8, 31), FiscalPeriodStatus = FiscalPeriodStatus.Open };

    private static UpdateJournalCommand ValidCommand(long id, DateTime date, long? clientFiscalYearId = null, long? clientFiscalPeriodId = null) => new()
    {
        Id = id,
        Date = date,
        JournalTypeId = 1,
        CurrencyId = 1,
        Rate = 1,
        FiscalYearId = clientFiscalYearId,
        FiscalPeriodId = clientFiscalPeriodId,
        JournalItems = []
    };

    /// <summary>Draft journal already assigned to fiscal year 10 / period 20 — the pre-edit state
    /// UpdateCommandHandler loads via IJournalRepository.GetByIdAsync. Built through the same
    /// CreateDraft/AssignFiscalPeriod domain API the real Create/Post flow uses, since Journal's
    /// header/lifecycle fields no longer have public setters (see the Accounting DDD cleanup report).</summary>
    private static Journal ExistingJournal(long id = 1, bool posted = false)
    {
        var journal = Journal.CreateDraft(
            journalTypeId: 1, typeId: 1, codeNumber: 1, code: "GJ-1", date: OriginalDate,
            createUserId: 1, createDate: OriginalDate, branchId: null, shiftId: null,
            currencyId: 1, rate: 1, note: null);
        journal.Id = id;
        journal.AssignFiscalPeriod(Year(10), Period(20, 10));
        journal.Posted = posted;
        return journal;
    }

    private static (UpdateCommandHandler handler, Mock<IJournalRepository> repository, Mock<IAccountingPeriodService> accountingPeriodService) BuildHandler(Journal? existingJournal)
    {
        var repository = new Mock<IJournalRepository>();
        repository.Setup(r => r.GetByIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingJournal);

        var accountRepository = new Mock<IAccountRepository>();
        accountRepository
            .Setup(r => r.GetByIdsAsync(It.IsAny<IReadOnlyCollection<long>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((IReadOnlyCollection<long> ids, CancellationToken _) => ids.Select(id => new Account { Id = id, IsPostable = true }).ToList());

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.SaveChangeAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var accountingPeriodService = new Mock<IAccountingPeriodService>();

        var handler = new UpdateCommandHandler(
            unitOfWork.Object, repository.Object, accountRepository.Object, accountingPeriodService.Object);

        return (handler, repository, accountingPeriodService);
    }

    [Fact]
    public async Task Handle_DateUnchanged_NonAccountingFieldEditSucceeds_EvenIfPeriodHasSinceClosed()
    {
        // The journal already lives in a period that has since closed, but only its Note
        // (a non-accounting field) is being edited — the date itself is untouched.
        var existing = ExistingJournal();
        var (handler, repository, accountingPeriodService) = BuildHandler(existing);

        var command = ValidCommand(1, OriginalDate);
        command.Note = "updated note only";

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.Equal(10, existing.FiscalYearId);
        Assert.Equal(20, existing.FiscalPeriodId);
        Assert.False(existing.Posted);
        accountingPeriodService.Verify(s => s.ResolveAndValidateAsync(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ClientSuppliedPostedTrue_IsIgnored_DraftCannotSelfPost()
    {
        var existing = ExistingJournal();
        var (handler, repository, accountingPeriodService) = BuildHandler(existing);

        var command = ValidCommand(1, OriginalDate);
        command.Posted = true; // client tries to sneak Posted=true through a plain Update

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.False(existing.Posted);
    }

    [Fact]
    public async Task Handle_DateChangedToAnotherOpenPeriod_ReassignsFiscalYearAndPeriod()
    {
        var existing = ExistingJournal();
        var (handler, repository, accountingPeriodService) = BuildHandler(existing);
        var year = Year(10);
        var newPeriod = new FiscalPeriod { Id = 21, FiscalYearId = 10, Name = "September 2026", PeriodNumber = 9, StartDate = new DateTime(2026, 9, 1), EndDate = new DateTime(2026, 9, 30), FiscalPeriodStatus = FiscalPeriodStatus.Open };
        accountingPeriodService.Setup(s => s.ResolveAndValidateAsync(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(AccountingPeriodResult.Ok(year, newPeriod));

        var result = await handler.Handle(ValidCommand(1, AnotherOpenDate), CancellationToken.None);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.Equal(newPeriod.Id, existing.FiscalPeriodId);
        Assert.Equal(year.Id, existing.FiscalYearId);
    }

    [Fact]
    public async Task Handle_DateChangedIntoClosedPeriod_RejectsUpdate()
    {
        var existing = ExistingJournal();
        var (handler, repository, accountingPeriodService) = BuildHandler(existing);
        accountingPeriodService.Setup(s => s.ResolveAndValidateAsync(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(AccountingPeriodResult.Fail("Fiscal period September 2026 is closed. Journal entries cannot be created or posted."));

        var result = await handler.Handle(ValidCommand(1, AnotherOpenDate), CancellationToken.None);

        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        Assert.Contains("Fiscal period September 2026 is closed. Journal entries cannot be created or posted.", result.Errors!.Select(e => e.MessageError));
        Assert.Equal(20, existing.FiscalPeriodId); // unchanged — the rejected edit never touched the aggregate
    }

    [Fact]
    public async Task Handle_ClientSuppliedFiscalIds_AreIgnoredOnDateChange_ServerResolvedValuesWin()
    {
        var existing = ExistingJournal();
        var (handler, repository, accountingPeriodService) = BuildHandler(existing);
        var year = Year(10);
        var newPeriod = new FiscalPeriod { Id = 21, FiscalYearId = 10, Name = "September 2026", PeriodNumber = 9, StartDate = new DateTime(2026, 9, 1), EndDate = new DateTime(2026, 9, 30), FiscalPeriodStatus = FiscalPeriodStatus.Open };
        accountingPeriodService.Setup(s => s.ResolveAndValidateAsync(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(AccountingPeriodResult.Ok(year, newPeriod));

        // Client tries to smuggle in fiscal ids that belong to neither the old nor the resolved period.
        var result = await handler.Handle(ValidCommand(1, AnotherOpenDate, clientFiscalYearId: 9999, clientFiscalPeriodId: 8888), CancellationToken.None);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.Equal(year.Id, existing.FiscalYearId);
        Assert.Equal(newPeriod.Id, existing.FiscalPeriodId);
    }

    [Fact]
    public async Task Handle_TypeChangedToOpeningBalanceWithDateUnchanged_StillValidatesOpeningBalanceRules()
    {
        // Date is unchanged (so the fast path that skips ResolveAndValidateAsync applies), but the
        // JournalType is being switched to Opening Balance — this must still be validated.
        var existing = ExistingJournal();
        var (handler, repository, accountingPeriodService) = BuildHandler(existing);
        var year = Year(10);
        accountingPeriodService.Setup(s => s.GetFiscalYearAsync(10, It.IsAny<CancellationToken>())).ReturnsAsync(year);
        accountingPeriodService.Setup(s => s.ValidateOpeningBalanceAsync(It.IsAny<long>(), It.IsAny<DateTime>(), It.IsAny<FiscalYear>(), It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([new Error("Opening Balance entry date must equal the fiscal year start date (2026-01-01).")]);

        var result = await handler.Handle(ValidCommand(1, OriginalDate), CancellationToken.None);

        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        Assert.Contains("Opening Balance entry date must equal the fiscal year start date (2026-01-01).", result.Errors!.Select(e => e.MessageError));
    }

    [Fact]
    public async Task Handle_PostedJournal_RejectsAnyEdit()
    {
        var existing = ExistingJournal(posted: true);
        var (handler, repository, accountingPeriodService) = BuildHandler(existing);

        var command = ValidCommand(1, OriginalDate);
        command.Note = "trying to edit a posted entry";

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(HttpStatusCode.Forbidden, result.StatusCode);
        accountingPeriodService.Verify(s => s.ResolveAndValidateAsync(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_JournalNotFound_ReturnsNotFound()
    {
        var (handler, repository, _) = BuildHandler(null);

        var result = await handler.Handle(ValidCommand(1, OriginalDate), CancellationToken.None);

        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
    }
}

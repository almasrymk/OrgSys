using Accounting.Application.Journals.Commands;
using Accounting.Application;
using Accounting.Domain.Repositories;
using Moq;
using System.Net;
using Xunit;

namespace Application.Tests;

public class JournalPostCommandHandlerTests
{
    private static readonly DateTime JournalDate = new(2026, 8, 17);

    private static FiscalYear Year(long id = 10, FiscalYearStatus status = FiscalYearStatus.Open) => new()
    { Id = id, Name = "2026", StartDate = new DateTime(2026, 1, 1), EndDate = new DateTime(2026, 12, 31), FiscalYearStatus = status };

    private static FiscalPeriod Period(long id = 20, long yearId = 10, FiscalPeriodStatus status = FiscalPeriodStatus.Open) => new()
    { Id = id, FiscalYearId = yearId, Name = "August 2026", PeriodNumber = 8, StartDate = new DateTime(2026, 8, 1), EndDate = new DateTime(2026, 8, 31), FiscalPeriodStatus = status };

    private static (PostJournalCommandHandler handler, Mock<IJournalRepository> repository, Mock<IUnitOfWork> unitOfWork, Mock<IAccountingPeriodService> accountingPeriodService) BuildHandler(Journal? existingJournal)
    {
        var repository = new Mock<IJournalRepository>();
        repository.Setup(r => r.GetByIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingJournal);

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.SaveChangeAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var accountingPeriodService = new Mock<IAccountingPeriodService>();

        // Every account referenced by a journal line resolves as postable/active by default —
        // individual tests override this to exercise the "account not postable" rule.
        var accountRepository = new Mock<IAccountRepository>();
        accountRepository
            .Setup(r => r.GetByIdsAsync(It.IsAny<IReadOnlyCollection<long>>(), It.IsAny<CancellationToken>()))
            .Returns((IReadOnlyCollection<long> ids, CancellationToken _) =>
                Task.FromResult<IReadOnlyList<Account>>(ids.Select(id => new Account { Id = id, IsPostable = true }).ToList()));

        var integrationEventPublisher = new Mock<IIntegrationEventPublisher>();

        var handler = new PostJournalCommandHandler(unitOfWork.Object, repository.Object, accountRepository.Object, accountingPeriodService.Object, integrationEventPublisher.Object);

        return (handler, repository, unitOfWork, accountingPeriodService);
    }

    [Fact]
    public async Task Handle_DraftInOpenPeriod_PostsSuccessfully()
    {
        var existing = new Journal
        {
            Id = 1,
            Date = JournalDate,
            Posted = false,
            FiscalYearId = 10,
            FiscalPeriodId = 20,
            JournalItems = [new JournalItem { AccountId = 101, Debit = 100, Credit = 0 }, new JournalItem { AccountId = 102, Debit = 0, Credit = 100 }]
        };
        var (handler, repository, unitOfWork, accountingPeriodService) = BuildHandler(existing);
        var year = Year();
        var period = Period();
        accountingPeriodService.Setup(s => s.ResolveAndValidateAsync(JournalDate, It.IsAny<CancellationToken>()))
            .ReturnsAsync(AccountingPeriodResult.Ok(year, period));

        var result = await handler.Handle(new PostJournalCommand(1), CancellationToken.None);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.True(existing.Posted);
        Assert.Equal(year.Id, existing.FiscalYearId);
        Assert.Equal(period.Id, existing.FiscalPeriodId);
        unitOfWork.Verify(u => u.BeginTransactionAsync(), Times.Once);
        unitOfWork.Verify(u => u.CommitAsync(), Times.Once);
        unitOfWork.Verify(u => u.RollbackAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_PeriodClosedAfterDraftCreatedWhileOpen_RejectsPosting()
    {
        var existing = new Journal
        {
            Id = 1, Date = JournalDate, Posted = false, FiscalYearId = 10, FiscalPeriodId = 20,
            JournalItems = [new JournalItem { AccountId = 101, Debit = 100, Credit = 0 }, new JournalItem { AccountId = 102, Debit = 0, Credit = 100 }]
        };
        var (handler, repository, unitOfWork, accountingPeriodService) = BuildHandler(existing);
        accountingPeriodService.Setup(s => s.ResolveAndValidateAsync(JournalDate, It.IsAny<CancellationToken>()))
            .ReturnsAsync(AccountingPeriodResult.Fail("Fiscal period August 2026 is closed. Journal entries cannot be created or posted."));

        var result = await handler.Handle(new PostJournalCommand(1), CancellationToken.None);

        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        Assert.Contains("Fiscal period August 2026 is closed. Journal entries cannot be created or posted.", result.Errors!.Select(e => e.MessageError));
        Assert.False(existing.Posted);
        unitOfWork.Verify(u => u.RollbackAsync(), Times.Once);
        unitOfWork.Verify(u => u.CommitAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_PeriodLocked_RejectsPosting()
    {
        var existing = new Journal
        {
            Id = 1, Date = JournalDate, Posted = false, FiscalYearId = 10, FiscalPeriodId = 20,
            JournalItems = [new JournalItem { AccountId = 101, Debit = 100, Credit = 0 }, new JournalItem { AccountId = 102, Debit = 0, Credit = 100 }]
        };
        var (handler, repository, unitOfWork, accountingPeriodService) = BuildHandler(existing);
        accountingPeriodService.Setup(s => s.ResolveAndValidateAsync(JournalDate, It.IsAny<CancellationToken>()))
            .ReturnsAsync(AccountingPeriodResult.Fail("Fiscal period August 2026 is locked. Journal entries cannot be created or posted."));

        var result = await handler.Handle(new PostJournalCommand(1), CancellationToken.None);

        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        Assert.Contains("Fiscal period August 2026 is locked. Journal entries cannot be created or posted.", result.Errors!.Select(e => e.MessageError));
        Assert.False(existing.Posted);
    }

    [Fact]
    public async Task Handle_FiscalYearClosedBeforePosting_RejectsPosting()
    {
        var existing = new Journal
        {
            Id = 1, Date = JournalDate, Posted = false, FiscalYearId = 10, FiscalPeriodId = 20,
            JournalItems = [new JournalItem { AccountId = 101, Debit = 100, Credit = 0 }, new JournalItem { AccountId = 102, Debit = 0, Credit = 100 }]
        };
        var (handler, repository, unitOfWork, accountingPeriodService) = BuildHandler(existing);
        accountingPeriodService.Setup(s => s.ResolveAndValidateAsync(JournalDate, It.IsAny<CancellationToken>()))
            .ReturnsAsync(AccountingPeriodResult.Fail("Fiscal year 2026 is closed. Journal entries cannot be created or posted."));

        var result = await handler.Handle(new PostJournalCommand(1), CancellationToken.None);

        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        Assert.Contains("Fiscal year 2026 is closed. Journal entries cannot be created or posted.", result.Errors!.Select(e => e.MessageError));
    }

    [Fact]
    public async Task Handle_DebitDoesNotEqualCredit_RejectsPosting()
    {
        var existing = new Journal
        {
            Id = 1,
            Date = JournalDate,
            Posted = false,
            FiscalYearId = 10,
            FiscalPeriodId = 20,
            JournalItems = [new JournalItem { Debit = 100, Credit = 0 }, new JournalItem { Debit = 0, Credit = 50 }]
        };
        var (handler, repository, unitOfWork, accountingPeriodService) = BuildHandler(existing);

        var result = await handler.Handle(new PostJournalCommand(1), CancellationToken.None);

        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        Assert.Contains("Total Debit must equal total Credit before posting.", result.Errors!.Select(e => e.MessageError));
        Assert.False(existing.Posted);
        unitOfWork.Verify(u => u.BeginTransactionAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_NoValidLines_RejectsPosting_ZeroDoesNotCountAsBalanced()
    {
        // A journal with no lines at all (0 == 0) must never read as "balanced" and post.
        var existing = new Journal { Id = 1, Date = JournalDate, Posted = false, FiscalYearId = 10, FiscalPeriodId = 20 };
        var (handler, repository, unitOfWork, accountingPeriodService) = BuildHandler(existing);

        var result = await handler.Handle(new PostJournalCommand(1), CancellationToken.None);

        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        Assert.Contains("Total Debit must equal total Credit before posting.", result.Errors!.Select(e => e.MessageError));
        Assert.False(existing.Posted);
    }

    [Fact]
    public async Task Handle_OnlyZeroValueLines_RejectsPosting_ZeroDoesNotCountAsBalanced()
    {
        // Two lines that are each individually 0/0 — an apparently-balanced 0==0 total that must
        // still be rejected, since neither line carries any real amount.
        var existing = new Journal
        {
            Id = 1,
            Date = JournalDate,
            Posted = false,
            FiscalYearId = 10,
            FiscalPeriodId = 20,
            JournalItems = [new JournalItem { AccountId = 101, Debit = 0, Credit = 0 }, new JournalItem { AccountId = 102, Debit = 0, Credit = 0 }]
        };
        var (handler, repository, unitOfWork, accountingPeriodService) = BuildHandler(existing);

        var result = await handler.Handle(new PostJournalCommand(1), CancellationToken.None);

        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        Assert.Contains("Total Debit must equal total Credit before posting.", result.Errors!.Select(e => e.MessageError));
        Assert.False(existing.Posted);
    }

    [Fact]
    public async Task Handle_DebitEqualsCredit_PostsSuccessfully()
    {
        var existing = new Journal
        {
            Id = 1,
            Date = JournalDate,
            Posted = false,
            FiscalYearId = 10,
            FiscalPeriodId = 20,
            JournalItems = [new JournalItem { AccountId = 101, Debit = 100, Credit = 0 }, new JournalItem { AccountId = 102, Debit = 0, Credit = 100 }]
        };
        var (handler, repository, unitOfWork, accountingPeriodService) = BuildHandler(existing);
        var year = Year();
        var period = Period();
        accountingPeriodService.Setup(s => s.ResolveAndValidateAsync(JournalDate, It.IsAny<CancellationToken>()))
            .ReturnsAsync(AccountingPeriodResult.Ok(year, period));

        var result = await handler.Handle(new PostJournalCommand(1), CancellationToken.None);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.True(existing.Posted);
    }

    [Fact]
    public async Task Handle_LineReferencesNonPostableAccount_RejectsPosting()
    {
        var existing = new Journal
        {
            Id = 1,
            Date = JournalDate,
            Posted = false,
            FiscalYearId = 10,
            FiscalPeriodId = 20,
            JournalItems = [new JournalItem { AccountId = 101, Debit = 100, Credit = 0 }, new JournalItem { AccountId = 102, Debit = 0, Credit = 100 }]
        };
        var (handler, repository, unitOfWork, accountingPeriodService) = BuildHandler(existing);
        accountingPeriodService.Setup(s => s.ResolveAndValidateAsync(JournalDate, It.IsAny<CancellationToken>()))
            .ReturnsAsync(AccountingPeriodResult.Ok(Year(), Period()));

        var accountRepository = new Mock<IAccountRepository>();
        accountRepository
            .Setup(r => r.GetByIdsAsync(It.IsAny<IReadOnlyCollection<long>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((IReadOnlyCollection<long> ids, CancellationToken _) => ids
                .Select(id => new Account { Id = id, IsPostable = id != 101 }) // Account 101 is a non-postable group account.
                .ToList());

        var handlerWithGroupAccount = new PostJournalCommandHandler(
            unitOfWork.Object, repository.Object, accountRepository.Object, accountingPeriodService.Object, Mock.Of<IIntegrationEventPublisher>());

        var result = await handlerWithGroupAccount.Handle(new PostJournalCommand(1), CancellationToken.None);

        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        Assert.False(existing.Posted);
        unitOfWork.Verify(u => u.RollbackAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_OpeningBalanceRuleViolated_RollsBackAndRejectsPosting()
    {
        var existing = new Journal { Id = 1, Date = JournalDate, Posted = false, FiscalYearId = 10, FiscalPeriodId = 20,
            JournalItems = [new JournalItem { AccountId = 101, Debit = 100, Credit = 0 }, new JournalItem { AccountId = 102, Debit = 0, Credit = 100 }] };
        var (handler, repository, unitOfWork, accountingPeriodService) = BuildHandler(existing);
        var year = Year();
        var period = Period();
        accountingPeriodService.Setup(s => s.ResolveAndValidateAsync(JournalDate, It.IsAny<CancellationToken>()))
            .ReturnsAsync(AccountingPeriodResult.Ok(year, period));
        accountingPeriodService.Setup(s => s.ValidateOpeningBalanceAsync(It.IsAny<long>(), It.IsAny<DateTime>(), It.IsAny<FiscalYear>(), It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([new Error("An Opening Balance journal already exists for fiscal year 2026.")]);

        var result = await handler.Handle(new PostJournalCommand(1), CancellationToken.None);

        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        Assert.Contains("An Opening Balance journal already exists for fiscal year 2026.", result.Errors!.Select(e => e.MessageError));
        Assert.False(existing.Posted);
        unitOfWork.Verify(u => u.RollbackAsync(), Times.Once);
        unitOfWork.Verify(u => u.CommitAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_AlreadyPosted_RejectsWithClearMessage()
    {
        var existing = new Journal { Id = 1, Date = JournalDate, Posted = true, FiscalYearId = 10, FiscalPeriodId = 20 };
        var (handler, repository, unitOfWork, accountingPeriodService) = BuildHandler(existing);

        var result = await handler.Handle(new PostJournalCommand(1), CancellationToken.None);

        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        Assert.Contains("already posted", result.Errors!.Select(e => e.MessageError).First(), StringComparison.OrdinalIgnoreCase);
        accountingPeriodService.Verify(s => s.ResolveAndValidateAsync(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_JournalNotFound_ReturnsNotFound()
    {
        var (handler, repository, unitOfWork, accountingPeriodService) = BuildHandler(null);

        var result = await handler.Handle(new PostJournalCommand(999), CancellationToken.None);

        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
    }
}

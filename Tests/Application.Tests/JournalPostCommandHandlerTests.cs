using Accounting.Application.Journals.Commands;
using Accounting.Application;
using AutoMapper;
using Domain.Abstraction;
using Domain.Entities;
using Domain.Enums;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net;
using Xunit;

namespace Application.Tests;

public class JournalPostCommandHandlerTests
{
    private static IMapper BuildMapper()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddAutoMapper(cfg => { cfg.AddProfile<global::MappingProfile>(); cfg.AddProfile<Accounting.Application.MappingProfile>(); });
        return services.BuildServiceProvider().GetRequiredService<IMapper>();
    }

    private static readonly DateTime JournalDate = new(2026, 8, 17);

    private static FiscalYear Year(long id = 10, FiscalYearStatus status = FiscalYearStatus.Open) => new()
    { Id = id, Name = "2026", StartDate = new DateTime(2026, 1, 1), EndDate = new DateTime(2026, 12, 31), FiscalYearStatus = status };

    private static FiscalPeriod Period(long id = 20, long yearId = 10, FiscalPeriodStatus status = FiscalPeriodStatus.Open) => new()
    { Id = id, FiscalYearId = yearId, Name = "August 2026", PeriodNumber = 8, StartDate = new DateTime(2026, 8, 1), EndDate = new DateTime(2026, 8, 31), FiscalPeriodStatus = status };

    private static (PostJournalCommandHandler handler, Mock<IRepository<Journal>> repository, Mock<IUnitOfWork> unitOfWork, Mock<IAccountingPeriodService> accountingPeriodService) BuildHandler(Journal? existingJournal)
    {
        var repository = new Mock<IRepository<Journal>>();
        repository.Setup(r => r.GetByFilterAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Journal, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync(existingJournal);
        repository.Setup(r => r.UpdateAsync(It.IsAny<Journal>())).ReturnsAsync(true);

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.SaveChangeAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var accountingPeriodService = new Mock<IAccountingPeriodService>();

        var handler = new PostJournalCommandHandler(unitOfWork.Object, repository.Object, accountingPeriodService.Object, BuildMapper(), Mock.Of<IServiceProvider>());

        return (handler, repository, unitOfWork, accountingPeriodService);
    }

    [Fact]
    public async Task Handle_DraftInOpenPeriod_PostsSuccessfully()
    {
        var existing = new Journal { Id = 1, Date = JournalDate, Posted = false, FiscalYearId = 10, FiscalPeriodId = 20 };
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
        var existing = new Journal { Id = 1, Date = JournalDate, Posted = false, FiscalYearId = 10, FiscalPeriodId = 20 };
        var (handler, repository, unitOfWork, accountingPeriodService) = BuildHandler(existing);
        accountingPeriodService.Setup(s => s.ResolveAndValidateAsync(JournalDate, It.IsAny<CancellationToken>()))
            .ReturnsAsync(AccountingPeriodResult.Fail("Fiscal period August 2026 is closed. Journal entries cannot be created or posted."));

        var result = await handler.Handle(new PostJournalCommand(1), CancellationToken.None);

        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        Assert.Contains("Fiscal period August 2026 is closed. Journal entries cannot be created or posted.", result.Errors!.Select(e => e.MessageError));
        Assert.False(existing.Posted);
        repository.Verify(r => r.UpdateAsync(It.IsAny<Journal>()), Times.Never);
        unitOfWork.Verify(u => u.RollbackAsync(), Times.Once);
        unitOfWork.Verify(u => u.CommitAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_PeriodLocked_RejectsPosting()
    {
        var existing = new Journal { Id = 1, Date = JournalDate, Posted = false, FiscalYearId = 10, FiscalPeriodId = 20 };
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
        var existing = new Journal { Id = 1, Date = JournalDate, Posted = false, FiscalYearId = 10, FiscalPeriodId = 20 };
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
        repository.Verify(r => r.UpdateAsync(It.IsAny<Journal>()), Times.Never);
        unitOfWork.Verify(u => u.BeginTransactionAsync(), Times.Never);
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
            JournalItems = [new JournalItem { Debit = 100, Credit = 0 }, new JournalItem { Debit = 0, Credit = 100 }]
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
    public async Task Handle_OpeningBalanceRuleViolated_RollsBackAndRejectsPosting()
    {
        var existing = new Journal { Id = 1, Date = JournalDate, Posted = false, FiscalYearId = 10, FiscalPeriodId = 20 };
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
        repository.Verify(r => r.UpdateAsync(It.IsAny<Journal>()), Times.Never);
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
        repository.Verify(r => r.UpdateAsync(It.IsAny<Journal>()), Times.Never);
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

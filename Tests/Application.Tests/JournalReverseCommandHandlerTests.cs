using Accounting.Application.Journals.Commands;
using Accounting.Application;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System.Net;
using Xunit;

namespace Application.Tests;

public class JournalReverseCommandHandlerTests
{
    private static IMapper BuildMapper()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddAutoMapper(cfg => { cfg.AddProfile<global::MappingProfile>(); cfg.AddProfile<Accounting.Application.MappingProfile>(); });
        return services.BuildServiceProvider().GetRequiredService<IMapper>();
    }

    private static readonly DateTime Today = DateTime.Now.Date;

    private static FiscalYear OpenYear(long id = 10) => new()
    { Id = id, Name = "2026", StartDate = Today.AddMonths(-6), EndDate = Today.AddMonths(6), FiscalYearStatus = FiscalYearStatus.Open };

    private static FiscalPeriod OpenPeriod(long id = 20, long yearId = 10) => new()
    { Id = id, FiscalYearId = yearId, Name = "Current", PeriodNumber = 1, StartDate = Today.AddDays(-10), EndDate = Today.AddDays(10), FiscalPeriodStatus = FiscalPeriodStatus.Open };

    private static Journal PostedOriginal(long id = 1) => new()
    {
        Id = id,
        Code = "GJ-100",
        CodeNumber = 100,
        Date = Today.AddDays(-30),
        Posted = true,
        Status = Status.New,
        JournalTypeId = 2,
        CurrencyId = 1,
        Rate = 1,
        TypeId = 0,
        ParentId = 0,
        CreateUserId = 1,
        FiscalYearId = 10,
        FiscalPeriodId = 20,
        JournalItems =
        [
            new JournalItem { Id = 1, AccountId = 101, Debit = 1000, Credit = 0, Note = "Cash" },
            new JournalItem { Id = 2, AccountId = 102, Debit = 0, Credit = 1000, Note = "Capital" }
        ]
    };

    private static (ReverseJournalCommandHandler handler, Mock<IRepository<Journal>> repository, Mock<IUnitOfWork> unitOfWork, Mock<IAccountingPeriodService> accountingPeriodService) BuildHandler(Journal? existing)
    {
        var repository = new Mock<IRepository<Journal>>();
        repository.Setup(r => r.GetByFilterAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Journal, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync(existing);
        repository.Setup(r => r.UpdateAsync(It.IsAny<Journal>())).ReturnsAsync(true);
        Journal? created = null;
        repository.Setup(r => r.CreateAsync(It.IsAny<Journal>()))
            .Returns((Journal j) => { created = j; return new ValueTask<Journal>(j); });
        repository.Setup(r => r.GetMaxByFilterAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Journal, bool>>>(), It.IsAny<System.Linq.Expressions.Expression<Func<Journal, long>>>()))
            .ReturnsAsync(100L);

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.SaveChangeAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var accountingPeriodService = new Mock<IAccountingPeriodService>();

        var handler = new ReverseJournalCommandHandler(unitOfWork.Object, repository.Object, accountingPeriodService.Object, BuildMapper(), Mock.Of<IServiceProvider>(), NullLogger<ReverseJournalCommandHandler>.Instance);

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
        repository.Setup(r => r.CreateAsync(It.IsAny<Journal>()))
            .Returns((Journal j) => { reversal = j; return new ValueTask<Journal>(j); });

        var result = await handler.Handle(new ReverseJournalCommand(original.Id), CancellationToken.None);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.NotNull(reversal);
        Assert.True(reversal!.Posted);
        Assert.Equal(original.Id, reversal.OriginalJournalId);
        Assert.Contains(original.Code!, reversal.Note);
        Assert.Equal(2, reversal.JournalItems!.Count);

        var line1 = reversal.JournalItems!.Single(i => i.AccountId == 101);
        Assert.Equal(0, line1.Debit);
        Assert.Equal(1000, line1.Credit);

        var line2 = reversal.JournalItems!.Single(i => i.AccountId == 102);
        Assert.Equal(1000, line2.Debit);
        Assert.Equal(0, line2.Credit);

        Assert.Equal(reversal.JournalItems!.Sum(i => i.Debit), reversal.JournalItems!.Sum(i => i.Credit));
        Assert.Equal(Status.Reversed, original.Status);
        unitOfWork.Verify(u => u.CommitAsync(), Times.Once);
        unitOfWork.Verify(u => u.RollbackAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_OriginalLinesAndHeaderAreNeverModified()
    {
        var original = PostedOriginal();
        var originalDate = original.Date;
        var originalLine1Debit = original.JournalItems!.First().Debit;
        var (handler, repository, unitOfWork, accountingPeriodService) = BuildHandler(original);
        accountingPeriodService.Setup(s => s.ResolveAndValidateAsync(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(AccountingPeriodResult.Ok(OpenYear(), OpenPeriod()));

        await handler.Handle(new ReverseJournalCommand(original.Id), CancellationToken.None);

        Assert.Equal(originalDate, original.Date);
        Assert.Equal(originalLine1Debit, original.JournalItems!.First().Debit);
        Assert.Equal(2, original.JournalItems!.Count);
    }

    [Fact]
    public async Task Handle_DraftJournal_RejectsReversal()
    {
        var draft = PostedOriginal();
        draft.Posted = false;
        var (handler, repository, unitOfWork, _) = BuildHandler(draft);

        var result = await handler.Handle(new ReverseJournalCommand(draft.Id), CancellationToken.None);

        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        repository.Verify(r => r.CreateAsync(It.IsAny<Journal>()), Times.Never);
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
        repository.Verify(r => r.CreateAsync(It.IsAny<Journal>()), Times.Never);
    }

    [Fact]
    public async Task Handle_JournalWithExistingReversalRelationship_RejectsDoubleReversalEvenIfStatusInconsistent()
    {
        // Belt-and-braces: Status somehow still New, but the relationship already shows a reversal exists.
        var original = PostedOriginal();
        original.Status = Status.New;
        original.ReversalJournal = new Journal { Id = 99, Code = "GJ-101" };
        var (handler, repository, unitOfWork, _) = BuildHandler(original);

        var result = await handler.Handle(new ReverseJournalCommand(original.Id), CancellationToken.None);

        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        repository.Verify(r => r.CreateAsync(It.IsAny<Journal>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ResourceLinkedJournal_RejectsReversal()
    {
        var original = PostedOriginal();
        original.RefranceTable = "invoice";
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
        repository.Verify(r => r.CreateAsync(It.IsAny<Journal>()), Times.Never);
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
        var original = PostedOriginal();
        original.JournalItems = [];
        var (handler, repository, unitOfWork, _) = BuildHandler(original);

        var result = await handler.Handle(new ReverseJournalCommand(original.Id), CancellationToken.None);

        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
    }
}

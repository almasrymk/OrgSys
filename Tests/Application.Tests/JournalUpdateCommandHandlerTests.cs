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

public class JournalUpdateCommandHandlerTests
{
    private static IMapper BuildMapper()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddAutoMapper(cfg => { cfg.AddProfile<global::MappingProfile>(); cfg.AddProfile<Accounting.Application.MappingProfile>(); });
        return services.BuildServiceProvider().GetRequiredService<IMapper>();
    }

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

    private static (UpdateCommandHandler handler, Mock<IRepository<Journal>> repository, Mock<IAccountingPeriodService> accountingPeriodService) BuildHandler(Journal existingJournal)
    {
        var repository = new Mock<IRepository<Journal>>();
        repository.Setup(r => r.GetByFilterAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Journal, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync(existingJournal);
        repository.Setup(r => r.UpdateAsync(It.IsAny<Journal>())).ReturnsAsync(true);

        var journalItemRepository = new Mock<IRepository<JournalItem>>();
        journalItemRepository.Setup(r => r.GetListByFilterAsync(It.IsAny<System.Linq.Expressions.Expression<Func<JournalItem, bool>>>()))
            .ReturnsAsync((IEnumerable<JournalItem>?)[]);
        journalItemRepository.Setup(r => r.ShiftDeleteAsync(It.IsAny<System.Linq.Expressions.Expression<Func<JournalItem, bool>>>())).ReturnsAsync(true);

        var serviceProvider = new Mock<IServiceProvider>();
        serviceProvider.Setup(p => p.GetService(typeof(IRepository<JournalItem>))).Returns(journalItemRepository.Object);

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.SaveChangeAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var accountingPeriodService = new Mock<IAccountingPeriodService>();

        var handler = new UpdateCommandHandler(
            unitOfWork.Object, repository.Object, journalItemRepository.Object,
            accountingPeriodService.Object, BuildMapper(), serviceProvider.Object);

        return (handler, repository, accountingPeriodService);
    }

    [Fact]
    public async Task Handle_DateUnchanged_NonAccountingFieldEditSucceeds_EvenIfPeriodHasSinceClosed()
    {
        // The journal already lives in a period that has since closed, but only its Note
        // (a non-accounting field) is being edited — the date itself is untouched.
        var existing = new Journal { Id = 1, RefranceTable = null, Posted = false, Date = OriginalDate, FiscalYearId = 10, FiscalPeriodId = 20 };
        var (handler, repository, accountingPeriodService) = BuildHandler(existing);

        Journal? updated = null;
        repository.Setup(r => r.UpdateAsync(It.IsAny<Journal>()))
            .Returns((Journal j) => { updated = j; return new ValueTask<bool>(true); });

        var command = ValidCommand(1, OriginalDate);
        command.Note = "updated note only";

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.NotNull(updated);
        Assert.Equal(10, updated!.FiscalYearId);
        Assert.Equal(20, updated.FiscalPeriodId);
        Assert.False(updated.Posted);
        accountingPeriodService.Verify(s => s.ResolveAndValidateAsync(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ClientSuppliedPostedTrue_IsIgnored_DraftCannotSelfPost()
    {
        var existing = new Journal { Id = 1, RefranceTable = null, Posted = false, Date = OriginalDate, FiscalYearId = 10, FiscalPeriodId = 20 };
        var (handler, repository, accountingPeriodService) = BuildHandler(existing);

        Journal? updated = null;
        repository.Setup(r => r.UpdateAsync(It.IsAny<Journal>()))
            .Returns((Journal j) => { updated = j; return new ValueTask<bool>(true); });

        var command = ValidCommand(1, OriginalDate);
        command.Posted = true; // client tries to sneak Posted=true through a plain Update

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.False(updated!.Posted);
    }

    [Fact]
    public async Task Handle_DateChangedToAnotherOpenPeriod_ReassignsFiscalYearAndPeriod()
    {
        var existing = new Journal { Id = 1, RefranceTable = null, Posted = false, Date = OriginalDate, FiscalYearId = 10, FiscalPeriodId = 20 };
        var (handler, repository, accountingPeriodService) = BuildHandler(existing);
        var year = Year(10);
        var newPeriod = new FiscalPeriod { Id = 21, FiscalYearId = 10, Name = "September 2026", PeriodNumber = 9, StartDate = new DateTime(2026, 9, 1), EndDate = new DateTime(2026, 9, 30), FiscalPeriodStatus = FiscalPeriodStatus.Open };
        accountingPeriodService.Setup(s => s.ResolveAndValidateAsync(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(AccountingPeriodResult.Ok(year, newPeriod));

        Journal? updated = null;
        repository.Setup(r => r.UpdateAsync(It.IsAny<Journal>()))
            .Returns((Journal j) => { updated = j; return new ValueTask<bool>(true); });

        var result = await handler.Handle(ValidCommand(1, AnotherOpenDate), CancellationToken.None);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.Equal(newPeriod.Id, updated!.FiscalPeriodId);
        Assert.Equal(year.Id, updated.FiscalYearId);
    }

    [Fact]
    public async Task Handle_DateChangedIntoClosedPeriod_RejectsUpdate()
    {
        var existing = new Journal { Id = 1, RefranceTable = null, Posted = false, Date = OriginalDate, FiscalYearId = 10, FiscalPeriodId = 20 };
        var (handler, repository, accountingPeriodService) = BuildHandler(existing);
        accountingPeriodService.Setup(s => s.ResolveAndValidateAsync(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(AccountingPeriodResult.Fail("Fiscal period September 2026 is closed. Journal entries cannot be created or posted."));

        var result = await handler.Handle(ValidCommand(1, AnotherOpenDate), CancellationToken.None);

        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        Assert.Contains("Fiscal period September 2026 is closed. Journal entries cannot be created or posted.", result.Errors!.Select(e => e.MessageError));
        repository.Verify(r => r.UpdateAsync(It.IsAny<Journal>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ClientSuppliedFiscalIds_AreIgnoredOnDateChange_ServerResolvedValuesWin()
    {
        var existing = new Journal { Id = 1, RefranceTable = null, Posted = false, Date = OriginalDate, FiscalYearId = 10, FiscalPeriodId = 20 };
        var (handler, repository, accountingPeriodService) = BuildHandler(existing);
        var year = Year(10);
        var newPeriod = new FiscalPeriod { Id = 21, FiscalYearId = 10, Name = "September 2026", PeriodNumber = 9, StartDate = new DateTime(2026, 9, 1), EndDate = new DateTime(2026, 9, 30), FiscalPeriodStatus = FiscalPeriodStatus.Open };
        accountingPeriodService.Setup(s => s.ResolveAndValidateAsync(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(AccountingPeriodResult.Ok(year, newPeriod));

        Journal? updated = null;
        repository.Setup(r => r.UpdateAsync(It.IsAny<Journal>()))
            .Returns((Journal j) => { updated = j; return new ValueTask<bool>(true); });

        // Client tries to smuggle in fiscal ids that belong to neither the old nor the resolved period.
        var result = await handler.Handle(ValidCommand(1, AnotherOpenDate, clientFiscalYearId: 9999, clientFiscalPeriodId: 8888), CancellationToken.None);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.Equal(year.Id, updated!.FiscalYearId);
        Assert.Equal(newPeriod.Id, updated.FiscalPeriodId);
    }

    [Fact]
    public async Task Handle_TypeChangedToOpeningBalanceWithDateUnchanged_StillValidatesOpeningBalanceRules()
    {
        // Date is unchanged (so the fast path that skips ResolveAndValidateAsync applies), but the
        // JournalType is being switched to Opening Balance — this must still be validated.
        var existing = new Journal { Id = 1, RefranceTable = null, Posted = false, Date = OriginalDate, FiscalYearId = 10, FiscalPeriodId = 20 };
        var (handler, repository, accountingPeriodService) = BuildHandler(existing);
        var year = Year(10);
        accountingPeriodService.Setup(s => s.GetFiscalYearAsync(10, It.IsAny<CancellationToken>())).ReturnsAsync(year);
        accountingPeriodService.Setup(s => s.ValidateOpeningBalanceAsync(It.IsAny<long>(), It.IsAny<DateTime>(), It.IsAny<FiscalYear>(), It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([new Error("Opening Balance entry date must equal the fiscal year start date (2026-01-01).")]);

        var result = await handler.Handle(ValidCommand(1, OriginalDate), CancellationToken.None);

        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        Assert.Contains("Opening Balance entry date must equal the fiscal year start date (2026-01-01).", result.Errors!.Select(e => e.MessageError));
        repository.Verify(r => r.UpdateAsync(It.IsAny<Journal>()), Times.Never);
    }

    [Fact]
    public async Task Handle_PostedJournal_RejectsAnyEdit()
    {
        var existing = new Journal { Id = 1, RefranceTable = null, Posted = true, Date = OriginalDate, FiscalYearId = 10, FiscalPeriodId = 20 };
        var (handler, repository, accountingPeriodService) = BuildHandler(existing);

        var command = ValidCommand(1, OriginalDate);
        command.Note = "trying to edit a posted entry";

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(HttpStatusCode.Forbidden, result.StatusCode);
        repository.Verify(r => r.UpdateAsync(It.IsAny<Journal>()), Times.Never);
        accountingPeriodService.Verify(s => s.ResolveAndValidateAsync(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_JournalNotFound_ReturnsNotFound()
    {
        var (handler, repository, _) = BuildHandler(null!);
        repository.Setup(r => r.GetByFilterAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Journal, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync((Journal?)null);

        var result = await handler.Handle(ValidCommand(1, OriginalDate), CancellationToken.None);

        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
    }
}

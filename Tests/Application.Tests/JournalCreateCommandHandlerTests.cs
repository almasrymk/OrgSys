using Application.Commands.Org.Financials.Journal.Commands;
using Application.Common.Services;
using AutoMapper;
using Domain.Abstraction;
using Domain.Entities;
using Domain.Enums;
using Domain.Shared;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net;
using Xunit;

namespace Application.Tests;

public class JournalCreateCommandHandlerTests
{
    private static IMapper BuildMapper()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddAutoMapper(cfg => cfg.AddProfile<global::MappingProfile>());
        return services.BuildServiceProvider().GetRequiredService<IMapper>();
    }

    private static FiscalYear Year(long id = 10) => new() { Id = id, Name = "2026", StartDate = new DateTime(2026, 1, 1), EndDate = new DateTime(2026, 12, 31), FiscalYearStatus = FiscalYearStatus.Open };

    private static FiscalPeriod Period(long id = 20, long yearId = 10) => new() { Id = id, FiscalYearId = yearId, Name = "August 2026", PeriodNumber = 8, StartDate = new DateTime(2026, 8, 1), EndDate = new DateTime(2026, 8, 31), FiscalPeriodStatus = FiscalPeriodStatus.Open };

    private static CreateJournalCommand ValidCommand(long? clientFiscalYearId = null, long? clientFiscalPeriodId = null) => new()
    {
        Date = new DateTime(2026, 8, 17),
        JournalTypeId = 1,
        CurrencyId = 1,
        Rate = 1,
        FiscalYearId = clientFiscalYearId,
        FiscalPeriodId = clientFiscalPeriodId,
        JournalItems = []
    };

    [Fact]
    public async Task Handle_OpenPeriod_CreatesJournalWithResolvedFiscalYearAndPeriod()
    {
        var year = Year();
        var period = Period();
        var accountingPeriodService = new Mock<IAccountingPeriodService>();
        accountingPeriodService.Setup(r => r.ResolveAndValidateAsync(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(AccountingPeriodResult.Ok(year, period));

        Journal? created = null;
        var repository = new Mock<IRepository<Journal>>();
        repository.Setup(r => r.CreateAsync(It.IsAny<Journal>()))
            .Returns((Journal j) => { created = j; return new ValueTask<Journal>(j); });

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.SaveChangeAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var handler = new CreateCommandHandler(unitOfWork.Object, repository.Object, accountingPeriodService.Object, BuildMapper(), Microsoft.Extensions.Logging.Abstractions.NullLogger<CreateCommandHandler>.Instance);

        var result = await handler.Handle(ValidCommand(), CancellationToken.None);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.NotNull(created);
        Assert.Equal(year.Id, created!.FiscalYearId);
        Assert.Equal(period.Id, created.FiscalPeriodId);
        repository.Verify(r => r.CreateAsync(It.IsAny<Journal>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ClientSuppliedFiscalIds_AreIgnoredAndServerResolvedValuesWin()
    {
        var year = Year(10);
        var period = Period(20, 10);
        var accountingPeriodService = new Mock<IAccountingPeriodService>();
        accountingPeriodService.Setup(r => r.ResolveAndValidateAsync(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(AccountingPeriodResult.Ok(year, period));

        Journal? created = null;
        var repository = new Mock<IRepository<Journal>>();
        repository.Setup(r => r.CreateAsync(It.IsAny<Journal>()))
            .Returns((Journal j) => { created = j; return new ValueTask<Journal>(j); });

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.SaveChangeAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var handler = new CreateCommandHandler(unitOfWork.Object, repository.Object, accountingPeriodService.Object, BuildMapper(), Microsoft.Extensions.Logging.Abstractions.NullLogger<CreateCommandHandler>.Instance);

        // Attacker/bogus client-supplied fiscal year/period ids that do NOT match the resolver's result.
        var command = ValidCommand(clientFiscalYearId: 9999, clientFiscalPeriodId: 8888);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.NotNull(created);
        Assert.Equal(year.Id, created!.FiscalYearId);
        Assert.Equal(period.Id, created.FiscalPeriodId);
        Assert.NotEqual(9999, created.FiscalYearId);
        Assert.NotEqual(8888, created.FiscalPeriodId);
    }

    [Fact]
    public async Task Handle_ClientSuppliedPostedTrue_IsIgnored_JournalIsCreatedAsDraft()
    {
        var year = Year();
        var period = Period();
        var accountingPeriodService = new Mock<IAccountingPeriodService>();
        accountingPeriodService.Setup(r => r.ResolveAndValidateAsync(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(AccountingPeriodResult.Ok(year, period));

        Journal? created = null;
        var repository = new Mock<IRepository<Journal>>();
        repository.Setup(r => r.CreateAsync(It.IsAny<Journal>()))
            .Returns((Journal j) => { created = j; return new ValueTask<Journal>(j); });

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.SaveChangeAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var handler = new CreateCommandHandler(unitOfWork.Object, repository.Object, accountingPeriodService.Object, BuildMapper(), Microsoft.Extensions.Logging.Abstractions.NullLogger<CreateCommandHandler>.Instance);

        var command = ValidCommand();
        command.Posted = true; // client tries to skip the Draft state entirely

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.False(created!.Posted);
    }

    [Fact]
    public async Task Handle_OpeningBalanceDateMatchesFiscalYearStart_Succeeds()
    {
        var year = Year();
        var period = Period();
        var accountingPeriodService = new Mock<IAccountingPeriodService>();
        accountingPeriodService.Setup(r => r.ResolveAndValidateAsync(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(AccountingPeriodResult.Ok(year, period));
        accountingPeriodService.Setup(r => r.ValidateOpeningBalanceAsync(It.IsAny<long>(), It.IsAny<DateTime>(), It.IsAny<FiscalYear>(), It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var repository = new Mock<IRepository<Journal>>();
        repository.Setup(r => r.CreateAsync(It.IsAny<Journal>())).Returns((Journal j) => new ValueTask<Journal>(j));

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.SaveChangeAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var handler = new CreateCommandHandler(unitOfWork.Object, repository.Object, accountingPeriodService.Object, BuildMapper(), Microsoft.Extensions.Logging.Abstractions.NullLogger<CreateCommandHandler>.Instance);

        var result = await handler.Handle(ValidCommand(), CancellationToken.None);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
    }

    [Fact]
    public async Task Handle_OpeningBalanceDateDoesNotMatchFiscalYearStart_RejectsAndDoesNotCreate()
    {
        var year = Year();
        var period = Period();
        var accountingPeriodService = new Mock<IAccountingPeriodService>();
        accountingPeriodService.Setup(r => r.ResolveAndValidateAsync(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(AccountingPeriodResult.Ok(year, period));
        accountingPeriodService.Setup(r => r.ValidateOpeningBalanceAsync(It.IsAny<long>(), It.IsAny<DateTime>(), It.IsAny<FiscalYear>(), It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([new Error("Opening Balance entry date must equal the fiscal year start date (2026-01-01).")]);

        var repository = new Mock<IRepository<Journal>>();
        var unitOfWork = new Mock<IUnitOfWork>();

        var handler = new CreateCommandHandler(unitOfWork.Object, repository.Object, accountingPeriodService.Object, BuildMapper(), Microsoft.Extensions.Logging.Abstractions.NullLogger<CreateCommandHandler>.Instance);

        var result = await handler.Handle(ValidCommand(), CancellationToken.None);

        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        Assert.Contains("Opening Balance entry date must equal the fiscal year start date (2026-01-01).", result.Errors!.Select(e => e.MessageError));
        repository.Verify(r => r.CreateAsync(It.IsAny<Journal>()), Times.Never);
    }

    [Fact]
    public async Task Handle_DuplicateOpeningBalanceForFiscalYear_RejectsAndDoesNotCreate()
    {
        var year = Year();
        var period = Period();
        var accountingPeriodService = new Mock<IAccountingPeriodService>();
        accountingPeriodService.Setup(r => r.ResolveAndValidateAsync(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(AccountingPeriodResult.Ok(year, period));
        accountingPeriodService.Setup(r => r.ValidateOpeningBalanceAsync(It.IsAny<long>(), It.IsAny<DateTime>(), It.IsAny<FiscalYear>(), It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([new Error("An Opening Balance journal already exists for fiscal year 2026.")]);

        var repository = new Mock<IRepository<Journal>>();
        var unitOfWork = new Mock<IUnitOfWork>();

        var handler = new CreateCommandHandler(unitOfWork.Object, repository.Object, accountingPeriodService.Object, BuildMapper(), Microsoft.Extensions.Logging.Abstractions.NullLogger<CreateCommandHandler>.Instance);

        var result = await handler.Handle(ValidCommand(), CancellationToken.None);

        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        Assert.Contains("An Opening Balance journal already exists for fiscal year 2026.", result.Errors!.Select(e => e.MessageError));
        repository.Verify(r => r.CreateAsync(It.IsAny<Journal>()), Times.Never);
    }

    [Theory]
    [InlineData("No fiscal year is configured for journal date 2026-08-17.")]
    [InlineData("Fiscal year 2026 is closed.")]
    [InlineData("No fiscal period is configured for journal date 2026-08-17.")]
    [InlineData("Fiscal period August 2026 is closed.")]
    [InlineData("Fiscal period August 2026 is locked.")]
    public async Task Handle_ResolverFails_ReturnsBadRequestAndDoesNotCreate(string errorMessage)
    {
        var accountingPeriodService = new Mock<IAccountingPeriodService>();
        accountingPeriodService.Setup(r => r.ResolveAndValidateAsync(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(AccountingPeriodResult.Fail(errorMessage));

        var repository = new Mock<IRepository<Journal>>();
        var unitOfWork = new Mock<IUnitOfWork>();

        var handler = new CreateCommandHandler(unitOfWork.Object, repository.Object, accountingPeriodService.Object, BuildMapper(), Microsoft.Extensions.Logging.Abstractions.NullLogger<CreateCommandHandler>.Instance);

        var result = await handler.Handle(ValidCommand(), CancellationToken.None);

        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        Assert.Contains(errorMessage, result.Errors!.Select(e => e.MessageError));
        repository.Verify(r => r.CreateAsync(It.IsAny<Journal>()), Times.Never);
    }
}

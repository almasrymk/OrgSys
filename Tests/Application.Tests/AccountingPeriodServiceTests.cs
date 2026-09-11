using Accounting.Application;
using Domain.Abstraction;
using Domain.Entities;
using Domain.Enums;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace Application.Tests;

public class AccountingPeriodServiceTests
{
    private static Mock<IRepository<FiscalYear>> MockFiscalYearRepository(params FiscalYear[] fiscalYears)
    {
        var mock = new Mock<IRepository<FiscalYear>>();
        mock.Setup(r => r.GetListByFilterAsync(It.IsAny<Expression<Func<FiscalYear, bool>>>()))
            .Returns((Expression<Func<FiscalYear, bool>> filter) =>
                new ValueTask<IEnumerable<FiscalYear>?>(fiscalYears.Where(filter.Compile()).ToList()));
        return mock;
    }

    private static Mock<IRepository<FiscalPeriod>> MockFiscalPeriodRepository(params FiscalPeriod[] fiscalPeriods)
    {
        var mock = new Mock<IRepository<FiscalPeriod>>();
        mock.Setup(r => r.GetListByFilterAsync(It.IsAny<Expression<Func<FiscalPeriod, bool>>>()))
            .Returns((Expression<Func<FiscalPeriod, bool>> filter) =>
                new ValueTask<IEnumerable<FiscalPeriod>?>(fiscalPeriods.Where(filter.Compile()).ToList()));
        return mock;
    }

    private static Mock<IRepository<JournalType>> MockJournalTypeRepository(params JournalType[] journalTypes)
    {
        var mock = new Mock<IRepository<JournalType>>();
        mock.Setup(r => r.GetListByFilterAsync(It.IsAny<Expression<Func<JournalType, bool>>>()))
            .Returns((Expression<Func<JournalType, bool>> filter) =>
                new ValueTask<IEnumerable<JournalType>?>(journalTypes.Where(filter.Compile()).ToList()));
        return mock;
    }

    private static Mock<IRepository<Journal>> MockJournalRepository(params Journal[] journals)
    {
        var mock = new Mock<IRepository<Journal>>();
        mock.Setup(r => r.GetListByFilterAsync(It.IsAny<Expression<Func<Journal, bool>>>()))
            .Returns((Expression<Func<Journal, bool>> filter) =>
                new ValueTask<IEnumerable<Journal>?>(journals.Where(filter.Compile()).ToList()));
        return mock;
    }

    private static AccountingPeriodService BuildService(
        FiscalYear[]? fiscalYears = null, FiscalPeriod[]? fiscalPeriods = null,
        JournalType[]? journalTypes = null, Journal[]? journals = null) =>
        new(MockFiscalYearRepository(fiscalYears ?? []).Object,
            MockFiscalPeriodRepository(fiscalPeriods ?? []).Object,
            MockJournalTypeRepository(journalTypes ?? []).Object,
            MockJournalRepository(journals ?? []).Object);

    private static JournalType NormalType(long id = 1) => new() { Id = id, Name = "Normal", IsOpeningBlance = false, Status = Status.New };

    private static JournalType OpeningBalanceType(long id = 2) => new() { Id = id, Name = "Opening Balance", IsOpeningBlance = true, Status = Status.New };

    private static FiscalYear OpenYear(long id = 1) => new()
    {
        Id = id,
        Name = "2026",
        StartDate = new DateTime(2026, 1, 1),
        EndDate = new DateTime(2026, 12, 31),
        FiscalYearStatus = FiscalYearStatus.Open,
        Status = Status.New
    };

    private static FiscalPeriod OpenPeriod(long yearId = 1, long id = 1) => new()
    {
        Id = id,
        FiscalYearId = yearId,
        Name = "August 2026",
        PeriodNumber = 8,
        StartDate = new DateTime(2026, 8, 1),
        EndDate = new DateTime(2026, 8, 31),
        FiscalPeriodStatus = FiscalPeriodStatus.Open,
        Status = Status.New
    };

    [Fact]
    public async Task ResolveAndValidateAsync_OpenYearAndPeriod_ReturnsSuccessWithBoth()
    {
        var year = OpenYear();
        var period = OpenPeriod(year.Id);
        var service = BuildService(fiscalYears: [year], fiscalPeriods: [period]);

        var result = await service.ResolveAndValidateAsync(new DateTime(2026, 8, 17));

        Assert.True(result.Success);
        Assert.Equal(year.Id, result.FiscalYear!.Id);
        Assert.Equal(period.Id, result.FiscalPeriod!.Id);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public async Task ResolveAndValidateAsync_NoFiscalYearForDate_Fails()
    {
        var service = BuildService();

        var result = await service.ResolveAndValidateAsync(new DateTime(2026, 8, 17));

        Assert.False(result.Success);
        Assert.Contains("No fiscal year is configured for journal date 2026-08-17.", result.Errors.Select(e => e.MessageError));
    }

    [Fact]
    public async Task ResolveAndValidateAsync_FiscalYearClosed_Fails()
    {
        var year = OpenYear();
        year.FiscalYearStatus = FiscalYearStatus.Closed;
        var service = BuildService(fiscalYears: [year]);

        var result = await service.ResolveAndValidateAsync(new DateTime(2026, 8, 17));

        Assert.False(result.Success);
        Assert.Contains("Fiscal year 2026 is closed. Journal entries cannot be created or posted.", result.Errors.Select(e => e.MessageError));
    }

    [Fact]
    public async Task ResolveAndValidateAsync_NoFiscalPeriodForDate_Fails()
    {
        var year = OpenYear();
        var service = BuildService(fiscalYears: [year]);

        var result = await service.ResolveAndValidateAsync(new DateTime(2026, 8, 17));

        Assert.False(result.Success);
        Assert.Contains("No fiscal period is configured for journal date 2026-08-17.", result.Errors.Select(e => e.MessageError));
    }

    [Fact]
    public async Task ResolveAndValidateAsync_FiscalPeriodClosed_Fails()
    {
        var year = OpenYear();
        var period = OpenPeriod(year.Id);
        period.FiscalPeriodStatus = FiscalPeriodStatus.Closed;
        var service = BuildService(fiscalYears: [year], fiscalPeriods: [period]);

        var result = await service.ResolveAndValidateAsync(new DateTime(2026, 8, 17));

        Assert.False(result.Success);
        Assert.Contains("Fiscal period August 2026 is closed. Journal entries cannot be created or posted.", result.Errors.Select(e => e.MessageError));
    }

    [Fact]
    public async Task ResolveAndValidateAsync_FiscalPeriodLocked_Fails()
    {
        var year = OpenYear();
        var period = OpenPeriod(year.Id);
        period.FiscalPeriodStatus = FiscalPeriodStatus.Locked;
        var service = BuildService(fiscalYears: [year], fiscalPeriods: [period]);

        var result = await service.ResolveAndValidateAsync(new DateTime(2026, 8, 17));

        Assert.False(result.Success);
        Assert.Contains("Fiscal period August 2026 is locked. Journal entries cannot be created or posted.", result.Errors.Select(e => e.MessageError));
    }

    [Fact]
    public async Task GetFiscalYearAsync_ExistingId_ReturnsIt()
    {
        var year = OpenYear();
        var service = BuildService(fiscalYears: [year]);

        var result = await service.GetFiscalYearAsync(year.Id);

        Assert.NotNull(result);
        Assert.Equal(year.Id, result!.Id);
    }

    [Fact]
    public async Task ValidateOpeningBalanceAsync_NonOpeningBalanceType_ReturnsNoErrors()
    {
        var year = OpenYear();
        var normalType = NormalType();
        var service = BuildService(journalTypes: [normalType]);

        var errors = await service.ValidateOpeningBalanceAsync(normalType.Id, new DateTime(2026, 5, 15), year, journalId: 0);

        Assert.Empty(errors);
    }

    [Fact]
    public async Task ValidateOpeningBalanceAsync_DateEqualsFiscalYearStart_ReturnsNoErrors()
    {
        var year = OpenYear();
        var openingType = OpeningBalanceType();
        var service = BuildService(journalTypes: [openingType]);

        var errors = await service.ValidateOpeningBalanceAsync(openingType.Id, year.StartDate, year, journalId: 0);

        Assert.Empty(errors);
    }

    [Fact]
    public async Task ValidateOpeningBalanceAsync_DateNotEqualFiscalYearStart_Fails()
    {
        var year = OpenYear();
        var openingType = OpeningBalanceType();
        var service = BuildService(journalTypes: [openingType]);

        var errors = await service.ValidateOpeningBalanceAsync(openingType.Id, new DateTime(2026, 1, 2), year, journalId: 0);

        Assert.Contains($"Opening Balance entry date must equal the fiscal year start date ({year.StartDate:yyyy-MM-dd}).", errors.Select(e => e.MessageError));
    }

    [Fact]
    public async Task ValidateOpeningBalanceAsync_DuplicateOpeningBalanceInSameFiscalYear_Fails()
    {
        var year = OpenYear();
        var openingType = OpeningBalanceType();
        var existingOpeningBalance = new Journal { Id = 5, FiscalYearId = year.Id, JournalTypeId = openingType.Id, Status = Status.New, Hide = false };
        var service = BuildService(journalTypes: [openingType], journals: [existingOpeningBalance]);

        var errors = await service.ValidateOpeningBalanceAsync(openingType.Id, year.StartDate, year, journalId: 0);

        Assert.Contains($"An Opening Balance journal already exists for fiscal year {year.Name}.", errors.Select(e => e.MessageError));
    }

    [Fact]
    public async Task ValidateOpeningBalanceAsync_EditingTheSameOpeningBalanceJournal_DoesNotConflictWithItself()
    {
        var year = OpenYear();
        var openingType = OpeningBalanceType();
        var existingOpeningBalance = new Journal { Id = 5, FiscalYearId = year.Id, JournalTypeId = openingType.Id, Status = Status.New, Hide = false };
        var service = BuildService(journalTypes: [openingType], journals: [existingOpeningBalance]);

        var errors = await service.ValidateOpeningBalanceAsync(openingType.Id, year.StartDate, year, journalId: 5);

        Assert.Empty(errors);
    }

    [Fact]
    public async Task ValidateOpeningBalanceAsync_ExistingOpeningBalanceIsCancelled_DoesNotBlockANewOne()
    {
        var year = OpenYear();
        var openingType = OpeningBalanceType();
        var cancelledOpeningBalance = new Journal { Id = 5, FiscalYearId = year.Id, JournalTypeId = openingType.Id, Status = Status.Cancel, Hide = false };
        var service = BuildService(journalTypes: [openingType], journals: [cancelledOpeningBalance]);

        var errors = await service.ValidateOpeningBalanceAsync(openingType.Id, year.StartDate, year, journalId: 0);

        Assert.Empty(errors);
    }
}

using Application.Common.Services;
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
        var service = new AccountingPeriodService(MockFiscalYearRepository(year).Object, MockFiscalPeriodRepository(period).Object);

        var result = await service.ResolveAndValidateAsync(new DateTime(2026, 8, 17));

        Assert.True(result.Success);
        Assert.Equal(year.Id, result.FiscalYear!.Id);
        Assert.Equal(period.Id, result.FiscalPeriod!.Id);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public async Task ResolveAndValidateAsync_NoFiscalYearForDate_Fails()
    {
        var service = new AccountingPeriodService(MockFiscalYearRepository().Object, MockFiscalPeriodRepository().Object);

        var result = await service.ResolveAndValidateAsync(new DateTime(2026, 8, 17));

        Assert.False(result.Success);
        Assert.Contains("No fiscal year is configured for journal date 2026-08-17.", result.Errors.Select(e => e.MessageError));
    }

    [Fact]
    public async Task ResolveAndValidateAsync_FiscalYearClosed_Fails()
    {
        var year = OpenYear();
        year.FiscalYearStatus = FiscalYearStatus.Closed;
        var service = new AccountingPeriodService(MockFiscalYearRepository(year).Object, MockFiscalPeriodRepository().Object);

        var result = await service.ResolveAndValidateAsync(new DateTime(2026, 8, 17));

        Assert.False(result.Success);
        Assert.Contains("Fiscal year 2026 is closed. Journal entries cannot be created or posted.", result.Errors.Select(e => e.MessageError));
    }

    [Fact]
    public async Task ResolveAndValidateAsync_NoFiscalPeriodForDate_Fails()
    {
        var year = OpenYear();
        var service = new AccountingPeriodService(MockFiscalYearRepository(year).Object, MockFiscalPeriodRepository().Object);

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
        var service = new AccountingPeriodService(MockFiscalYearRepository(year).Object, MockFiscalPeriodRepository(period).Object);

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
        var service = new AccountingPeriodService(MockFiscalYearRepository(year).Object, MockFiscalPeriodRepository(period).Object);

        var result = await service.ResolveAndValidateAsync(new DateTime(2026, 8, 17));

        Assert.False(result.Success);
        Assert.Contains("Fiscal period August 2026 is locked. Journal entries cannot be created or posted.", result.Errors.Select(e => e.MessageError));
    }
}

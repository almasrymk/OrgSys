namespace Accounting.Domain.Tests;

public class FiscalPeriodTests
{
    private static FiscalPeriod OpenPeriod(long id = 20, long yearId = 10) => new()
    { Id = id, FiscalYearId = yearId, Name = "August 2026", PeriodNumber = 8, StartDate = new DateTime(2026, 8, 1), EndDate = new DateTime(2026, 8, 31), FiscalPeriodStatus = FiscalPeriodStatus.Open };

    [Fact]
    public void EnsureOpenForPosting_OpenPeriod_DoesNotThrow()
    {
        var period = OpenPeriod();

        var ex = Record.Exception(period.EnsureOpenForPosting);

        Assert.Null(ex);
    }

    [Fact]
    public void Close_OpenPeriod_TransitionsToClosedAndRaisesDomainEvent()
    {
        var period = OpenPeriod();

        period.Close();

        Assert.Equal(FiscalPeriodStatus.Closed, period.FiscalPeriodStatus);
        var raised = Assert.Single(period.DomainEvents.OfType<FiscalPeriodClosedDomainEvent>());
        Assert.Equal(period.Id, raised.FiscalPeriodId);
    }

    [Fact]
    public void Close_AlreadyClosed_IsIdempotent_NoDuplicateEvent()
    {
        var period = OpenPeriod();
        period.Close();
        period.ClearDomainEvents();

        period.Close();

        Assert.Empty(period.DomainEvents);
    }

    [Fact]
    public void EnsureOpenForPosting_ClosedPeriod_ThrowsAccountingPeriodClosed()
    {
        var period = OpenPeriod();
        period.Close();

        Assert.Throws<AccountingPeriodClosedException>(period.EnsureOpenForPosting);
    }

    [Fact]
    public void Reopen_ClosedPeriod_TransitionsToOpenAndRaisesDomainEvent()
    {
        var period = OpenPeriod();
        period.Close();

        period.Reopen();

        Assert.Equal(FiscalPeriodStatus.Open, period.FiscalPeriodStatus);
        Assert.Single(period.DomainEvents.OfType<FiscalPeriodReopenedDomainEvent>());
        var ex = Record.Exception(period.EnsureOpenForPosting);
        Assert.Null(ex);
    }

    [Fact]
    public void EnsureOpenForPosting_LockedPeriod_ThrowsAccountingPeriodClosed()
    {
        var period = OpenPeriod();
        period.FiscalPeriodStatus = FiscalPeriodStatus.Locked;

        Assert.Throws<AccountingPeriodClosedException>(period.EnsureOpenForPosting);
    }

    [Fact]
    public void Reopen_LockedPeriod_ThrowsAccountingPeriodClosed_LockedIsStrongerThanClosed()
    {
        var period = OpenPeriod();
        period.FiscalPeriodStatus = FiscalPeriodStatus.Locked;

        Assert.Throws<AccountingPeriodClosedException>(period.Reopen);
        Assert.Equal(FiscalPeriodStatus.Locked, period.FiscalPeriodStatus);
    }
}

using Moq;
using Receivables.Application.Balances.Queries;
using Receivables.Application.OpenItems.Queries;
using Receivables.Contracts.Receivables;
using Receivables.Domain;
using System.Linq.Expressions;
using Xunit;

namespace Application.Tests;

public class ReceivableReadModelQueryHandlerTests
{
    private static Receivable Make(long id, decimal original, decimal outstanding, DateTime dueDate, ReceivableStatus status, long customerId = 1)
    {
        var r = Receivable.Create(customerId, SourceDocumentType.SalesInvoice, id, $"INV-{id}", dueDate, dueDate, 1, 1, original, 1, dueDate);
        r.Id = id;
        if (status == ReceivableStatus.PartiallySettled || status == ReceivableStatus.Settled)
            r.Apply(original - outstanding);
        else if (status == ReceivableStatus.Cancelled)
            r.Cancel();
        else if (status == ReceivableStatus.WrittenOff)
            r.WriteOff(original, "test write-off");
        return r;
    }

    private static Mock<IRepository<Receivable>> MockRepository(List<Receivable> all)
    {
        var repository = new Mock<IRepository<Receivable>>();

        repository.Setup(r => r.GetListByFilterAsync(
                It.IsAny<Expression<Func<Receivable, bool>>>(),
                It.IsAny<Func<IQueryable<Receivable>, IOrderedQueryable<Receivable>>>()))
            .Returns<Expression<Func<Receivable, bool>>, Func<IQueryable<Receivable>, IOrderedQueryable<Receivable>>>((filter, orderBy) =>
                new ValueTask<IEnumerable<Receivable>?>(orderBy(all.AsQueryable().Where(filter)).ToList()));

        repository.Setup(r => r.GetListByFilterAsync(It.IsAny<Expression<Func<Receivable, bool>>>()))
            .Returns<Expression<Func<Receivable, bool>>>(filter =>
                new ValueTask<IEnumerable<Receivable>?>(all.AsQueryable().Where(filter).ToList()));

        return repository;
    }

    [Fact]
    public async Task GetOutstandingReceivables_ExcludesSettledCancelledAndWrittenOff()
    {
        var open = Make(1, 1000, 1000, new DateTime(2026, 9, 1), ReceivableStatus.Open);
        var partial = Make(2, 1000, 400, new DateTime(2026, 9, 1), ReceivableStatus.PartiallySettled);
        var settled = Make(3, 1000, 0, new DateTime(2026, 9, 1), ReceivableStatus.Settled);
        var cancelled = Make(4, 1000, 1000, new DateTime(2026, 9, 1), ReceivableStatus.Cancelled);
        var writtenOff = Make(5, 1000, 0, new DateTime(2026, 9, 1), ReceivableStatus.WrittenOff);
        var handler = new GetOutstandingReceivablesQueryHandler(MockRepository([open, partial, settled, cancelled, writtenOff]).Object);

        var result = await handler.Handle(new GetOutstandingReceivablesQuery(null), CancellationToken.None);

        var ids = result.Response!.Select(r => r.Id).ToList();
        Assert.Equal([1, 2], ids);
    }

    [Fact]
    public async Task GetCustomerSubledgerBalance_SumsOnlyOpenItems()
    {
        var open = Make(1, 1000, 1000, new DateTime(2026, 9, 1), ReceivableStatus.Open);
        var partial = Make(2, 1000, 400, new DateTime(2026, 9, 1), ReceivableStatus.PartiallySettled);
        var settled = Make(3, 1000, 0, new DateTime(2026, 9, 1), ReceivableStatus.Settled);
        var handler = new GetCustomerSubledgerBalanceQueryHandler(MockRepository([open, partial, settled]).Object);

        var result = await handler.Handle(new Receivables.Contracts.Balances.GetCustomerSubledgerBalanceQuery(1), CancellationToken.None);

        Assert.Equal(1400, result.Response);
    }

    [Fact]
    public async Task GetSubledgerAging_PlacesItemsInCorrectBuckets()
    {
        var asOf = new DateTime(2026, 9, 30);
        var current = Make(1, 100, 100, asOf.AddDays(-10), ReceivableStatus.Open);      // 10 days -> Current
        var bucket31 = Make(2, 100, 100, asOf.AddDays(-45), ReceivableStatus.Open);     // 45 days -> 31-60
        var bucket61 = Make(3, 100, 100, asOf.AddDays(-75), ReceivableStatus.Open);     // 75 days -> 61-90
        var bucket90 = Make(4, 100, 100, asOf.AddDays(-120), ReceivableStatus.Open);    // 120 days -> 90+
        var handler = new GetSubledgerAgingQueryHandler(MockRepository([current, bucket31, bucket61, bucket90]).Object);

        var result = await handler.Handle(new Receivables.Contracts.Balances.GetSubledgerAgingQuery(1, asOf), CancellationToken.None);

        Assert.Equal(100, result.Response!.Current);
        Assert.Equal(100, result.Response.Days31To60);
        Assert.Equal(100, result.Response.Days61To90);
        Assert.Equal(100, result.Response.Over90);
    }

    [Fact]
    public async Task GetOverdueReceivables_OnlyReturnsPastDueOpenItems()
    {
        var asOf = new DateTime(2026, 9, 30);
        var overdue = Make(1, 100, 100, asOf.AddDays(-5), ReceivableStatus.Open);
        var notYetDue = Make(2, 100, 100, asOf.AddDays(5), ReceivableStatus.Open);
        var handler = new GetOverdueReceivablesQueryHandler(MockRepository([overdue, notYetDue]).Object);

        var result = await handler.Handle(new GetOverdueReceivablesQuery(null, asOf), CancellationToken.None);

        var ids = result.Response!.Select(r => r.Id).ToList();
        Assert.Equal([1], ids);
    }
}

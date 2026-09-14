using Moq;
using Payables.Application.Balances.Queries;
using Payables.Application.OpenItems.Queries;
using Payables.Contracts.Payables;
using Payables.Domain;
using System.Linq.Expressions;
using Xunit;

namespace Application.Tests;

public class PayableReadModelQueryHandlerTests
{
    private static Payable Make(long id, decimal original, decimal outstanding, DateTime dueDate, PayableStatus status, long supplierId = 1)
    {
        var p = Payable.Create(supplierId, SourceDocumentType.PurchaseInvoice, id, $"PINV-{id}", dueDate, dueDate, 1, 1, original, 1, dueDate);
        p.Id = id;
        if (status == PayableStatus.PartiallySettled || status == PayableStatus.Settled)
            p.Apply(original - outstanding);
        else if (status == PayableStatus.Cancelled)
            p.Cancel();
        else if (status == PayableStatus.WrittenOff)
            p.WriteOff(original, "test write-off");
        return p;
    }

    private static Mock<IRepository<Payable>> MockRepository(List<Payable> all)
    {
        var repository = new Mock<IRepository<Payable>>();

        repository.Setup(r => r.GetListByFilterAsync(
                It.IsAny<Expression<Func<Payable, bool>>>(),
                It.IsAny<Func<IQueryable<Payable>, IOrderedQueryable<Payable>>>()))
            .Returns<Expression<Func<Payable, bool>>, Func<IQueryable<Payable>, IOrderedQueryable<Payable>>>((filter, orderBy) =>
                new ValueTask<IEnumerable<Payable>?>(orderBy(all.AsQueryable().Where(filter)).ToList()));

        repository.Setup(r => r.GetListByFilterAsync(It.IsAny<Expression<Func<Payable, bool>>>()))
            .Returns<Expression<Func<Payable, bool>>>(filter =>
                new ValueTask<IEnumerable<Payable>?>(all.AsQueryable().Where(filter).ToList()));

        return repository;
    }

    [Fact]
    public async Task GetOutstandingPayables_ExcludesSettledCancelledAndWrittenOff()
    {
        var open = Make(1, 1000, 1000, new DateTime(2026, 9, 1), PayableStatus.Open);
        var partial = Make(2, 1000, 400, new DateTime(2026, 9, 1), PayableStatus.PartiallySettled);
        var settled = Make(3, 1000, 0, new DateTime(2026, 9, 1), PayableStatus.Settled);
        var cancelled = Make(4, 1000, 1000, new DateTime(2026, 9, 1), PayableStatus.Cancelled);
        var writtenOff = Make(5, 1000, 0, new DateTime(2026, 9, 1), PayableStatus.WrittenOff);
        var handler = new GetOutstandingPayablesQueryHandler(MockRepository([open, partial, settled, cancelled, writtenOff]).Object);

        var result = await handler.Handle(new GetOutstandingPayablesQuery(null), CancellationToken.None);

        var ids = result.Response!.Select(p => p.Id).ToList();
        Assert.Equal([1, 2], ids);
    }

    [Fact]
    public async Task GetSupplierSubledgerBalance_SumsOnlyOpenItems()
    {
        var open = Make(1, 1000, 1000, new DateTime(2026, 9, 1), PayableStatus.Open);
        var partial = Make(2, 1000, 400, new DateTime(2026, 9, 1), PayableStatus.PartiallySettled);
        var settled = Make(3, 1000, 0, new DateTime(2026, 9, 1), PayableStatus.Settled);
        var handler = new GetSupplierSubledgerBalanceQueryHandler(MockRepository([open, partial, settled]).Object);

        var result = await handler.Handle(new Payables.Contracts.Balances.GetSupplierSubledgerBalanceQuery(1), CancellationToken.None);

        Assert.Equal(1400, result.Response);
    }

    [Fact]
    public async Task GetSubledgerAging_PlacesItemsInCorrectBuckets()
    {
        var asOf = new DateTime(2026, 9, 30);
        var current = Make(1, 100, 100, asOf.AddDays(-10), PayableStatus.Open);
        var bucket31 = Make(2, 100, 100, asOf.AddDays(-45), PayableStatus.Open);
        var bucket61 = Make(3, 100, 100, asOf.AddDays(-75), PayableStatus.Open);
        var bucket90 = Make(4, 100, 100, asOf.AddDays(-120), PayableStatus.Open);
        var handler = new GetSubledgerAgingQueryHandler(MockRepository([current, bucket31, bucket61, bucket90]).Object);

        var result = await handler.Handle(new Payables.Contracts.Balances.GetSubledgerAgingQuery(1, asOf), CancellationToken.None);

        Assert.Equal(100, result.Response!.Current);
        Assert.Equal(100, result.Response.Days31To60);
        Assert.Equal(100, result.Response.Days61To90);
        Assert.Equal(100, result.Response.Over90);
    }

    [Fact]
    public async Task GetOverduePayables_OnlyReturnsPastDueOpenItems()
    {
        var asOf = new DateTime(2026, 9, 30);
        var overdue = Make(1, 100, 100, asOf.AddDays(-5), PayableStatus.Open);
        var notYetDue = Make(2, 100, 100, asOf.AddDays(5), PayableStatus.Open);
        var handler = new GetOverduePayablesQueryHandler(MockRepository([overdue, notYetDue]).Object);

        var result = await handler.Handle(new GetOverduePayablesQuery(null, asOf), CancellationToken.None);

        var ids = result.Response!.Select(p => p.Id).ToList();
        Assert.Equal([1], ids);
    }
}

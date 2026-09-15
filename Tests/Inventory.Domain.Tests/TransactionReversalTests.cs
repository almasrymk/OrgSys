namespace Inventory.Domain.Tests;

public class TransactionReversalTests
{
    private static readonly DateTime Date = new(2026, 9, 1);

    [Fact]
    public void Reversal_links_back_to_original_and_flags_as_reversal_source()
    {
        var original = new Transaction { Id = 42, TypeId = 2, StockId = 1, Total = 100, Date = Date };

        var reversal = Transaction.CreateReversal(original, createUserId: 1, createDate: Date, reversalDate: Date, reason: "wrong quantity");

        Assert.Equal(42, reversal.ReversalOfMovementId);
        Assert.Equal(42, reversal.SourceDocumentId);
        Assert.Equal(SourceDocumentType.Reversal, reversal.SourceDocumentType);
        Assert.Equal(original.Total, reversal.Total);
        Assert.Contains("Reversal of Transaction 42", reversal.Notes);
    }

    [Fact]
    public void Cannot_reverse_an_unposted_transaction() =>
        Assert.Throws<MovementAlreadyReversedException>(() =>
            Transaction.CreateReversal(new Transaction { Id = 0 }, 1, Date, Date, null));
}

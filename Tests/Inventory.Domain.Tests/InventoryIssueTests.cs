namespace Inventory.Domain.Tests;

public class InventoryIssueTests
{
    private static readonly DateTime Date = new(2026, 9, 1);

    private static InventoryIssue NewIssue() =>
        InventoryIssue.Create(stockId: 1, locationId: null, dealerId: null, date: Date, createUserId: 1, createDate: Date, branchId: null, notes: null);

    [Fact]
    public void Serial_line_must_have_quantity_of_one()
    {
        var issue = NewIssue();
        Assert.Throws<InventoryDocumentLineRequiredException>(() => issue.AddLine(1, 1, 2, null, serialId: 5, null));
    }

    [Fact]
    public void Serial_line_with_quantity_one_is_valid()
    {
        var issue = NewIssue();
        var line = issue.AddLine(1, 1, 1, null, serialId: 5, null);

        Assert.Equal(5, line.SerialId);
        Assert.Equal(1, line.Quantity);
    }

    [Fact]
    public void Posting_raises_stock_issued_event_per_line()
    {
        var issue = NewIssue();
        issue.AddLine(1, 1, 3, null, null, null);

        issue.Post(Date);

        var evt = Assert.Single(issue.DomainEvents);
        var issued = Assert.IsType<StockIssuedDomainEvent>(evt);
        Assert.Equal(3, issued.Quantity);
        Assert.Equal(1, issued.StockId);
    }

    [Fact]
    public void Cannot_modify_posted_issue()
    {
        var issue = NewIssue();
        issue.AddLine(1, 1, 3, null, null, null);
        issue.Post(Date);

        Assert.Throws<CannotModifyPostedDocumentException>(() => issue.AddLine(2, 1, 1, null, null, null));
    }
}

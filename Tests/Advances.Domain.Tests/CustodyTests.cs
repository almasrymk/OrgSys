namespace Advances.Domain.Tests;

public class CustodyTests
{
    private static readonly DateTime CreateDate = new(2026, 9, 1);
    private static readonly DateTime DueDate = new(2026, 9, 30);

    private static Custody NewCustody(decimal issuedAmount = 10000) => Custody.Create(
        holderId: 1,
        purpose: "Travel expenses",
        currencyId: 1,
        rate: 1,
        issuedAmount: issuedAmount,
        dueDate: DueDate,
        createUserId: 1,
        createDate: CreateDate);

    private static Custody NewIssuedCustody(decimal issuedAmount = 10000, long financialTransactionId = 500)
    {
        var custody = NewCustody(issuedAmount);
        custody.Approve();
        custody.MarkIssued(financialTransactionId, CreateDate);
        return custody;
    }

    // ----- Creation -----

    [Fact]
    public void Create_custody_with_valid_data()
    {
        var custody = NewCustody(10000);

        Assert.Equal(1, custody.HolderId);
        Assert.Equal("Travel expenses", custody.Purpose);
        Assert.Equal(10000, custody.IssuedAmount);
        Assert.Equal(CustodyStatus.Draft, custody.LifecycleStatus);
    }

    [Fact]
    public void New_custody_outstanding_equals_issued_amount()
    {
        var custody = NewCustody(1234.56m);

        Assert.Equal(custody.IssuedAmount, custody.OutstandingAmount);
    }

    [Fact]
    public void Cannot_create_custody_with_zero_amount() =>
        Assert.Throws<InvalidCustodyAmountException>(() => NewCustody(0));

    [Fact]
    public void Cannot_create_custody_with_negative_amount() =>
        Assert.Throws<InvalidCustodyAmountException>(() => NewCustody(-1));

    [Fact]
    public void Cannot_create_custody_without_purpose() =>
        Assert.Throws<CustodyPurposeRequiredException>(() => Custody.Create(1, "  ", 1, 1, 1000, DueDate, 1, CreateDate));

    // ----- Approve -----

    [Fact]
    public void Approve_draft_custody()
    {
        var custody = NewCustody();

        custody.Approve();

        Assert.Equal(CustodyStatus.Approved, custody.LifecycleStatus);
        Assert.Contains(custody.DomainEvents, e => e is CustodyApprovedDomainEvent);
    }

    [Fact]
    public void Cannot_approve_already_approved_custody()
    {
        var custody = NewCustody();
        custody.Approve();

        Assert.Throws<CustodyNotApprovableException>(() => custody.Approve());
    }

    // ----- Issue -----

    [Fact]
    public void Issue_approved_custody()
    {
        var custody = NewCustody();
        custody.Approve();

        custody.MarkIssued(500, CreateDate);

        Assert.Equal(CustodyStatus.Issued, custody.LifecycleStatus);
        Assert.Equal(500, custody.IssuingFinancialTransactionId);
        Assert.Contains(custody.DomainEvents, e => e is CustodyIssuedDomainEvent);
    }

    [Fact]
    public void Cannot_issue_draft_custody_without_approval() =>
        Assert.Throws<CustodyNotIssuableException>(() => NewCustody().MarkIssued(500, CreateDate));

    [Fact]
    public void Cannot_issue_twice()
    {
        var custody = NewIssuedCustody();

        Assert.Throws<CustodyNotIssuableException>(() => custody.MarkIssued(501, CreateDate));
    }

    // ----- Settlement -----

    [Fact]
    public void Cannot_settle_zero_or_negative_amount()
    {
        var custody = NewIssuedCustody();

        Assert.Throws<InvalidCustodySettlementAmountException>(() => custody.Settle(0));
        Assert.Throws<InvalidCustodySettlementAmountException>(() => custody.Settle(-1));
    }

    [Fact]
    public void Partial_settlement_marks_correct_status()
    {
        var custody = NewIssuedCustody(10000);

        custody.Settle(7500);

        Assert.Equal(2500, custody.OutstandingAmount);
        Assert.Equal(CustodyStatus.PartiallySettled, custody.LifecycleStatus);
        Assert.Contains(custody.DomainEvents, e => e is CustodySettlementPostedDomainEvent);
    }

    [Fact]
    public void Full_settlement_settles_custody()
    {
        var custody = NewIssuedCustody(10000);

        custody.Settle(10000);

        Assert.Equal(0, custody.OutstandingAmount);
        Assert.Equal(CustodyStatus.Settled, custody.LifecycleStatus);
    }

    [Fact]
    public void Cannot_over_settle()
    {
        var custody = NewIssuedCustody(10000);

        Assert.Throws<InvalidCustodySettlementAmountException>(() => custody.Settle(10000.01m));
    }

    [Fact]
    public void Cannot_settle_draft_custody() =>
        Assert.Throws<CustodyNotOpenForSettlementException>(() => NewCustody().Settle(100));

    [Fact]
    public void Cannot_settle_closed_custody()
    {
        var custody = NewIssuedCustody(1000);
        custody.Settle(1000);
        custody.Close();

        Assert.Throws<CustodyNotOpenForSettlementException>(() => custody.Settle(1));
    }

    [Fact]
    public void Cannot_settle_cancelled_custody()
    {
        var custody = NewCustody();
        custody.Cancel();

        Assert.Throws<CustodyNotOpenForSettlementException>(() => custody.Settle(1));
    }

    // ----- Return -----

    [Fact]
    public void Cash_return_reduces_outstanding_and_records_transaction()
    {
        var custody = NewIssuedCustody(10000);

        custody.Return(2500, 900);

        Assert.Equal(7500, custody.OutstandingAmount);
        Assert.Equal(900, custody.ReturnFinancialTransactionId);
        Assert.Contains(custody.DomainEvents, e => e is CustodyAmountReturnedDomainEvent);
    }

    [Fact]
    public void Full_return_alone_settles_custody()
    {
        var custody = NewIssuedCustody(10000);

        custody.Return(10000, 900);

        Assert.Equal(0, custody.OutstandingAmount);
        Assert.Equal(CustodyStatus.Settled, custody.LifecycleStatus);
    }

    [Fact]
    public void Settlement_and_return_combine_toward_outstanding()
    {
        var custody = NewIssuedCustody(10000);

        custody.Settle(6000);
        custody.Return(4000, 900);

        Assert.Equal(0, custody.OutstandingAmount);
        Assert.Equal(CustodyStatus.Settled, custody.LifecycleStatus);
    }

    [Fact]
    public void Cannot_return_more_than_outstanding()
    {
        var custody = NewIssuedCustody(10000);

        Assert.Throws<InvalidCustodySettlementAmountException>(() => custody.Return(10000.01m, 900));
    }

    [Fact]
    public void Return_requires_a_valid_treasury_transaction_id()
    {
        var custody = NewIssuedCustody(10000);

        Assert.Throws<InvalidCustodyAmountException>(() => custody.Return(100, 0));
    }

    // ----- Close -----

    [Fact]
    public void Close_settled_custody()
    {
        var custody = NewIssuedCustody(1000);
        custody.Settle(1000);

        custody.Close();

        Assert.Equal(CustodyStatus.Closed, custody.LifecycleStatus);
        Assert.Contains(custody.DomainEvents, e => e is CustodyClosedDomainEvent);
    }

    [Fact]
    public void Cannot_close_custody_with_outstanding_balance()
    {
        var custody = NewIssuedCustody(1000);
        custody.Settle(400);

        Assert.Throws<CustodyCannotBeClosedException>(() => custody.Close());
    }

    [Fact]
    public void Cannot_close_draft_custody() =>
        Assert.Throws<CustodyCannotBeClosedException>(() => NewCustody().Close());

    // ----- Cancel -----

    [Fact]
    public void Cancel_draft_custody()
    {
        var custody = NewCustody();

        custody.Cancel();

        Assert.Equal(CustodyStatus.Cancelled, custody.LifecycleStatus);
        Assert.Contains(custody.DomainEvents, e => e is CustodyCancelledDomainEvent);
    }

    [Fact]
    public void Cancel_is_idempotent()
    {
        var custody = NewCustody();
        custody.Cancel();
        custody.ClearDomainEvents();

        custody.Cancel();

        Assert.Empty(custody.DomainEvents);
        Assert.Equal(CustodyStatus.Cancelled, custody.LifecycleStatus);
    }

    [Fact]
    public void Cannot_cancel_approved_custody()
    {
        var custody = NewCustody();
        custody.Approve();

        Assert.Throws<CustodyCannotBeCancelledException>(() => custody.Cancel());
    }

    [Fact]
    public void Cannot_cancel_issued_custody() =>
        Assert.Throws<CustodyCannotBeCancelledException>(() => NewIssuedCustody().Cancel());

    // ----- Handover / transfer -----

    [Fact]
    public void Transfer_holder_changes_holder_and_records_handover()
    {
        var custody = NewIssuedCustody(10000);

        custody.TransferHolder(toHolderId: 2, reason: "Reassigned to new branch manager", approvedByUserId: 9, transferDate: CreateDate);

        Assert.Equal(2, custody.HolderId);
        var handover = Assert.Single(custody.Handovers);
        Assert.Equal(1, handover.FromHolderId);
        Assert.Equal(2, handover.ToHolderId);
        Assert.Equal(10000, handover.TransferredAmount);
        Assert.Contains(custody.DomainEvents, e => e is CustodyTransferredDomainEvent);
    }

    [Fact]
    public void Holder_only_changes_through_transfer_not_directly()
    {
        var custody = NewIssuedCustody(10000);
        var originalHolderId = custody.HolderId;

        custody.Settle(1000);

        Assert.Equal(originalHolderId, custody.HolderId);
    }

    [Fact]
    public void Cannot_transfer_draft_custody() =>
        Assert.Throws<InvalidCustodyTransferException>(() => NewCustody().TransferHolder(2, "reason", 9, CreateDate));

    [Fact]
    public void Cannot_transfer_to_same_holder()
    {
        var custody = NewIssuedCustody();

        Assert.Throws<InvalidCustodyTransferException>(() => custody.TransferHolder(custody.HolderId, "reason", 9, CreateDate));
    }

    [Fact]
    public void Transfer_requires_a_reason()
    {
        var custody = NewIssuedCustody();

        Assert.Throws<InvalidCustodyTransferException>(() => custody.TransferHolder(2, "  ", 9, CreateDate));
    }

    [Fact]
    public void Transfer_after_partial_settlement_carries_remaining_outstanding()
    {
        var custody = NewIssuedCustody(10000);
        custody.Settle(4000);

        custody.TransferHolder(2, "Handover", 9, CreateDate);

        var handover = Assert.Single(custody.Handovers);
        Assert.Equal(6000, handover.TransferredAmount);
    }

    // ----- Overdue (derived) -----

    [Fact]
    public void IsOverdue_true_when_outstanding_issued_and_past_due_date()
    {
        var custody = NewIssuedCustody();

        Assert.True(custody.IsOverdue(DueDate.AddDays(1)));
    }

    [Fact]
    public void IsOverdue_false_when_not_yet_due()
    {
        var custody = NewIssuedCustody();

        Assert.False(custody.IsOverdue(DueDate.AddDays(-1)));
    }

    [Fact]
    public void IsOverdue_false_once_settled_even_past_due_date()
    {
        var custody = NewIssuedCustody(1000);
        custody.Settle(1000);

        Assert.False(custody.IsOverdue(DueDate.AddDays(10)));
    }

    [Fact]
    public void IsOverdue_false_while_still_draft()
    {
        var custody = NewCustody();

        Assert.False(custody.IsOverdue(DueDate.AddDays(10)));
    }
}

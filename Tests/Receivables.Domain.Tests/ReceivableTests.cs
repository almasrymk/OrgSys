namespace Receivables.Domain.Tests;

public class ReceivableTests
{
    private static readonly DateTime DocumentDate = new(2026, 9, 1);
    private static readonly DateTime DueDate = new(2026, 9, 30);

    private static Receivable NewReceivable(decimal originalAmount = 1000) => Receivable.Create(
        customerId: 1,
        sourceDocumentType: SourceDocumentType.SalesInvoice,
        sourceDocumentId: 100,
        sourceDocumentNumber: "INV-100",
        documentDate: DocumentDate,
        dueDate: DueDate,
        currencyId: 1,
        rate: 1,
        originalAmount: originalAmount,
        createUserId: 1,
        createDate: DocumentDate);

    // ----- Creation -----

    [Fact]
    public void Create_receivable_with_valid_data()
    {
        var receivable = NewReceivable(1000);

        Assert.Equal(1, receivable.CustomerId);
        Assert.Equal(SourceDocumentType.SalesInvoice, receivable.SourceDocumentType);
        Assert.Equal(100, receivable.SourceDocumentId);
        Assert.Equal("INV-100", receivable.SourceDocumentNumber);
        Assert.Equal(1000, receivable.OriginalAmount);
        Assert.Equal(ReceivableStatus.Open, receivable.LifecycleStatus);
    }

    [Fact]
    public void New_receivable_outstanding_equals_original()
    {
        var receivable = NewReceivable(1234.56m);

        Assert.Equal(receivable.OriginalAmount, receivable.OutstandingAmount);
    }

    [Fact]
    public void Cannot_create_receivable_with_zero_amount() =>
        Assert.Throws<InvalidReceivableAmountException>(() => NewReceivable(0));

    [Fact]
    public void Cannot_create_receivable_with_negative_amount() =>
        Assert.Throws<InvalidReceivableAmountException>(() => NewReceivable(-1));

    // ----- Apply (payment application) -----

    [Fact]
    public void Cannot_apply_zero_payment()
    {
        var receivable = NewReceivable(1000);
        Assert.Throws<InvalidAllocationAmountException>(() => receivable.Apply(0));
    }

    [Fact]
    public void Cannot_apply_negative_payment()
    {
        var receivable = NewReceivable(1000);
        Assert.Throws<InvalidAllocationAmountException>(() => receivable.Apply(-1));
    }

    [Fact]
    public void Cannot_apply_more_than_outstanding()
    {
        var receivable = NewReceivable(1000);
        Assert.Throws<InvalidAllocationAmountException>(() => receivable.Apply(1000.01m));
    }

    [Fact]
    public void Partial_application_marks_correct_status()
    {
        var receivable = NewReceivable(1000);

        receivable.Apply(400);

        Assert.Equal(600, receivable.OutstandingAmount);
        Assert.Equal(ReceivableStatus.PartiallySettled, receivable.LifecycleStatus);
    }

    [Fact]
    public void Full_application_settles_receivable()
    {
        var receivable = NewReceivable(1000);

        receivable.Apply(1000);

        Assert.Equal(0, receivable.OutstandingAmount);
        Assert.Equal(ReceivableStatus.Settled, receivable.LifecycleStatus);
        Assert.Contains(receivable.DomainEvents, e => e is ReceivableSettledDomainEvent);
    }

    [Fact]
    public void Applying_raises_PaymentAppliedDomainEvent()
    {
        var receivable = NewReceivable(1000);
        receivable.Id = 42;

        receivable.Apply(400);

        var raised = Assert.Single(receivable.DomainEvents.OfType<PaymentAppliedDomainEvent>());
        Assert.Equal(42, raised.ReceivableId);
        Assert.Equal(400, raised.AppliedAmount);
        Assert.Equal(600, raised.RemainingOutstandingAmount);
    }

    [Fact]
    public void Cannot_apply_to_cancelled_receivable()
    {
        var receivable = NewReceivable(1000);
        receivable.Cancel();

        Assert.Throws<ReceivableNotOpenForApplicationException>(() => receivable.Apply(100));
    }

    [Fact]
    public void Cannot_apply_to_settled_receivable()
    {
        var receivable = NewReceivable(1000);
        receivable.Apply(1000);

        Assert.Throws<ReceivableNotOpenForApplicationException>(() => receivable.Apply(1));
    }

    [Fact]
    public void Cannot_apply_to_written_off_receivable()
    {
        var receivable = NewReceivable(1000);
        receivable.WriteOff(1000, "Uncollectible");

        Assert.Throws<ReceivableNotOpenForApplicationException>(() => receivable.Apply(1));
    }

    // ----- Unapply -----

    [Fact]
    public void Unapply_restores_outstanding()
    {
        var receivable = NewReceivable(1000);
        receivable.Apply(400);

        receivable.Unapply(400);

        Assert.Equal(1000, receivable.OutstandingAmount);
        Assert.Equal(ReceivableStatus.Open, receivable.LifecycleStatus);
    }

    [Fact]
    public void Partial_unapply_keeps_receivable_partially_settled()
    {
        var receivable = NewReceivable(1000);
        receivable.Apply(1000);

        receivable.Unapply(400);

        Assert.Equal(400, receivable.OutstandingAmount);
        Assert.Equal(ReceivableStatus.PartiallySettled, receivable.LifecycleStatus);
    }

    [Fact]
    public void Cannot_unapply_zero_or_negative_amount()
    {
        var receivable = NewReceivable(1000);
        receivable.Apply(400);

        Assert.Throws<InvalidAllocationAmountException>(() => receivable.Unapply(0));
        Assert.Throws<InvalidAllocationAmountException>(() => receivable.Unapply(-1));
    }

    [Fact]
    public void Cannot_unapply_beyond_original_amount()
    {
        var receivable = NewReceivable(1000);
        receivable.Apply(400);

        Assert.Throws<InvalidAllocationAmountException>(() => receivable.Unapply(400.01m));
    }

    [Fact]
    public void Cannot_unapply_on_cancelled_receivable()
    {
        var receivable = NewReceivable(1000);
        receivable.Cancel();

        Assert.Throws<ReceivableNotOpenForApplicationException>(() => receivable.Unapply(1));
    }

    // ----- Cancellation -----

    [Fact]
    public void Cancellation_of_untouched_receivable_is_valid()
    {
        var receivable = NewReceivable(1000);

        receivable.Cancel();

        Assert.Equal(ReceivableStatus.Cancelled, receivable.LifecycleStatus);
        Assert.Contains(receivable.DomainEvents, e => e is ReceivableCancelledDomainEvent);
    }

    [Fact]
    public void Cancel_is_idempotent()
    {
        var receivable = NewReceivable(1000);
        receivable.Cancel();
        receivable.ClearDomainEvents();

        receivable.Cancel();

        Assert.Empty(receivable.DomainEvents);
        Assert.Equal(ReceivableStatus.Cancelled, receivable.LifecycleStatus);
    }

    [Fact]
    public void Cannot_cancel_receivable_with_a_payment_applied()
    {
        var receivable = NewReceivable(1000);
        receivable.Apply(1);

        Assert.Throws<ReceivableCannotBeCancelledException>(() => receivable.Cancel());
    }

    [Fact]
    public void Cannot_cancel_settled_receivable()
    {
        var receivable = NewReceivable(1000);
        receivable.Apply(1000);

        Assert.Throws<ReceivableCannotBeCancelledException>(() => receivable.Cancel());
    }

    // ----- Write-off -----

    [Fact]
    public void Full_write_off_settles_receivable_as_written_off()
    {
        var receivable = NewReceivable(1000);

        receivable.WriteOff(1000, "Customer bankrupt");

        Assert.Equal(0, receivable.OutstandingAmount);
        Assert.Equal(ReceivableStatus.WrittenOff, receivable.LifecycleStatus);
    }

    [Fact]
    public void Partial_write_off_leaves_receivable_open_for_remainder()
    {
        var receivable = NewReceivable(1000);

        receivable.WriteOff(300, "Partial settlement agreed");

        Assert.Equal(700, receivable.OutstandingAmount);
        Assert.Equal(ReceivableStatus.Open, receivable.LifecycleStatus);
    }

    [Fact]
    public void Write_off_requires_a_reason()
    {
        var receivable = NewReceivable(1000);

        Assert.Throws<WriteOffReasonRequiredException>(() => receivable.WriteOff(100, ""));
        Assert.Throws<WriteOffReasonRequiredException>(() => receivable.WriteOff(100, "   "));
    }

    [Fact]
    public void Cannot_write_off_more_than_outstanding()
    {
        var receivable = NewReceivable(1000);

        Assert.Throws<InvalidAllocationAmountException>(() => receivable.WriteOff(1000.01m, "Too much"));
    }

    // ----- Overdue (derived) -----

    [Fact]
    public void IsOverdue_true_when_outstanding_and_past_due_date()
    {
        var receivable = NewReceivable(1000);

        Assert.True(receivable.IsOverdue(DueDate.AddDays(1)));
    }

    [Fact]
    public void IsOverdue_false_when_not_yet_due()
    {
        var receivable = NewReceivable(1000);

        Assert.False(receivable.IsOverdue(DueDate.AddDays(-1)));
    }

    [Fact]
    public void IsOverdue_false_once_fully_settled_even_past_due_date()
    {
        var receivable = NewReceivable(1000);
        receivable.Apply(1000);

        Assert.False(receivable.IsOverdue(DueDate.AddDays(10)));
    }
}

namespace Payables.Domain.Tests;

public class PayableTests
{
    private static readonly DateTime DocumentDate = new(2026, 9, 1);
    private static readonly DateTime DueDate = new(2026, 9, 30);

    private static Payable NewPayable(decimal originalAmount = 1000) => Payable.Create(
        supplierId: 1,
        sourceDocumentType: SourceDocumentType.PurchaseInvoice,
        sourceDocumentId: 100,
        sourceDocumentNumber: "PINV-100",
        documentDate: DocumentDate,
        dueDate: DueDate,
        currencyId: 1,
        rate: 1,
        originalAmount: originalAmount,
        createUserId: 1,
        createDate: DocumentDate);

    // ----- Creation -----

    [Fact]
    public void Create_payable_with_valid_data()
    {
        var payable = NewPayable(1000);

        Assert.Equal(1, payable.SupplierId);
        Assert.Equal(SourceDocumentType.PurchaseInvoice, payable.SourceDocumentType);
        Assert.Equal(100, payable.SourceDocumentId);
        Assert.Equal("PINV-100", payable.SourceDocumentNumber);
        Assert.Equal(1000, payable.OriginalAmount);
        Assert.Equal(PayableStatus.Open, payable.LifecycleStatus);
    }

    [Fact]
    public void New_payable_outstanding_equals_original()
    {
        var payable = NewPayable(1234.56m);

        Assert.Equal(payable.OriginalAmount, payable.OutstandingAmount);
    }

    [Fact]
    public void Cannot_create_payable_with_zero_amount() =>
        Assert.Throws<InvalidPayableAmountException>(() => NewPayable(0));

    [Fact]
    public void Cannot_create_payable_with_negative_amount() =>
        Assert.Throws<InvalidPayableAmountException>(() => NewPayable(-1));

    // ----- Apply (payment application) -----

    [Fact]
    public void Cannot_apply_zero_payment()
    {
        var payable = NewPayable(1000);
        Assert.Throws<InvalidAllocationAmountException>(() => payable.Apply(0));
    }

    [Fact]
    public void Cannot_apply_negative_payment()
    {
        var payable = NewPayable(1000);
        Assert.Throws<InvalidAllocationAmountException>(() => payable.Apply(-1));
    }

    [Fact]
    public void Cannot_apply_more_than_outstanding()
    {
        var payable = NewPayable(1000);
        Assert.Throws<InvalidAllocationAmountException>(() => payable.Apply(1000.01m));
    }

    [Fact]
    public void Partial_application_marks_correct_status()
    {
        var payable = NewPayable(1000);

        payable.Apply(400);

        Assert.Equal(600, payable.OutstandingAmount);
        Assert.Equal(PayableStatus.PartiallySettled, payable.LifecycleStatus);
    }

    [Fact]
    public void Full_application_settles_payable()
    {
        var payable = NewPayable(1000);

        payable.Apply(1000);

        Assert.Equal(0, payable.OutstandingAmount);
        Assert.Equal(PayableStatus.Settled, payable.LifecycleStatus);
        Assert.Contains(payable.DomainEvents, e => e is PayableSettledDomainEvent);
    }

    [Fact]
    public void Applying_raises_PaymentAppliedDomainEvent()
    {
        var payable = NewPayable(1000);
        payable.Id = 42;

        payable.Apply(400);

        var raised = Assert.Single(payable.DomainEvents.OfType<PaymentAppliedDomainEvent>());
        Assert.Equal(42, raised.PayableId);
        Assert.Equal(400, raised.AppliedAmount);
        Assert.Equal(600, raised.RemainingOutstandingAmount);
    }

    [Fact]
    public void Cannot_apply_to_cancelled_payable()
    {
        var payable = NewPayable(1000);
        payable.Cancel();

        Assert.Throws<PayableNotOpenForApplicationException>(() => payable.Apply(100));
    }

    [Fact]
    public void Cannot_apply_to_settled_payable()
    {
        var payable = NewPayable(1000);
        payable.Apply(1000);

        Assert.Throws<PayableNotOpenForApplicationException>(() => payable.Apply(1));
    }

    [Fact]
    public void Cannot_apply_to_written_off_payable()
    {
        var payable = NewPayable(1000);
        payable.WriteOff(1000, "Written off by mutual agreement");

        Assert.Throws<PayableNotOpenForApplicationException>(() => payable.Apply(1));
    }

    // ----- Unapply -----

    [Fact]
    public void Unapply_restores_outstanding()
    {
        var payable = NewPayable(1000);
        payable.Apply(400);

        payable.Unapply(400);

        Assert.Equal(1000, payable.OutstandingAmount);
        Assert.Equal(PayableStatus.Open, payable.LifecycleStatus);
    }

    [Fact]
    public void Partial_unapply_keeps_payable_partially_settled()
    {
        var payable = NewPayable(1000);
        payable.Apply(1000);

        payable.Unapply(400);

        Assert.Equal(400, payable.OutstandingAmount);
        Assert.Equal(PayableStatus.PartiallySettled, payable.LifecycleStatus);
    }

    [Fact]
    public void Cannot_unapply_zero_or_negative_amount()
    {
        var payable = NewPayable(1000);
        payable.Apply(400);

        Assert.Throws<InvalidAllocationAmountException>(() => payable.Unapply(0));
        Assert.Throws<InvalidAllocationAmountException>(() => payable.Unapply(-1));
    }

    [Fact]
    public void Cannot_unapply_beyond_original_amount()
    {
        var payable = NewPayable(1000);
        payable.Apply(400);

        Assert.Throws<InvalidAllocationAmountException>(() => payable.Unapply(400.01m));
    }

    [Fact]
    public void Cannot_unapply_on_cancelled_payable()
    {
        var payable = NewPayable(1000);
        payable.Cancel();

        Assert.Throws<PayableNotOpenForApplicationException>(() => payable.Unapply(1));
    }

    // ----- Cancellation -----

    [Fact]
    public void Cancellation_of_untouched_payable_is_valid()
    {
        var payable = NewPayable(1000);

        payable.Cancel();

        Assert.Equal(PayableStatus.Cancelled, payable.LifecycleStatus);
        Assert.Contains(payable.DomainEvents, e => e is PayableCancelledDomainEvent);
    }

    [Fact]
    public void Cancel_is_idempotent()
    {
        var payable = NewPayable(1000);
        payable.Cancel();
        payable.ClearDomainEvents();

        payable.Cancel();

        Assert.Empty(payable.DomainEvents);
        Assert.Equal(PayableStatus.Cancelled, payable.LifecycleStatus);
    }

    [Fact]
    public void Cannot_cancel_payable_with_a_payment_applied()
    {
        var payable = NewPayable(1000);
        payable.Apply(1);

        Assert.Throws<PayableCannotBeCancelledException>(() => payable.Cancel());
    }

    [Fact]
    public void Cannot_cancel_settled_payable()
    {
        var payable = NewPayable(1000);
        payable.Apply(1000);

        Assert.Throws<PayableCannotBeCancelledException>(() => payable.Cancel());
    }

    // ----- Write-off -----

    [Fact]
    public void Full_write_off_settles_payable_as_written_off()
    {
        var payable = NewPayable(1000);

        payable.WriteOff(1000, "Supplier waived remaining balance");

        Assert.Equal(0, payable.OutstandingAmount);
        Assert.Equal(PayableStatus.WrittenOff, payable.LifecycleStatus);
    }

    [Fact]
    public void Partial_write_off_leaves_payable_open_for_remainder()
    {
        var payable = NewPayable(1000);

        payable.WriteOff(300, "Partial settlement agreed");

        Assert.Equal(700, payable.OutstandingAmount);
        Assert.Equal(PayableStatus.Open, payable.LifecycleStatus);
    }

    [Fact]
    public void Write_off_requires_a_reason()
    {
        var payable = NewPayable(1000);

        Assert.Throws<WriteOffReasonRequiredException>(() => payable.WriteOff(100, ""));
        Assert.Throws<WriteOffReasonRequiredException>(() => payable.WriteOff(100, "   "));
    }

    [Fact]
    public void Cannot_write_off_more_than_outstanding()
    {
        var payable = NewPayable(1000);

        Assert.Throws<InvalidAllocationAmountException>(() => payable.WriteOff(1000.01m, "Too much"));
    }

    // ----- Overdue (derived) -----

    [Fact]
    public void IsOverdue_true_when_outstanding_and_past_due_date()
    {
        var payable = NewPayable(1000);

        Assert.True(payable.IsOverdue(DueDate.AddDays(1)));
    }

    [Fact]
    public void IsOverdue_false_when_not_yet_due()
    {
        var payable = NewPayable(1000);

        Assert.False(payable.IsOverdue(DueDate.AddDays(-1)));
    }

    [Fact]
    public void IsOverdue_false_once_fully_settled_even_past_due_date()
    {
        var payable = NewPayable(1000);
        payable.Apply(1000);

        Assert.False(payable.IsOverdue(DueDate.AddDays(10)));
    }
}

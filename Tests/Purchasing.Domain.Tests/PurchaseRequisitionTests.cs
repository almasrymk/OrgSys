namespace Purchasing.Domain.Tests;

public class PurchaseRequisitionTests
{
    private static readonly DateTime CreateDate = new(2026, 9, 1);

    private static PurchaseRequisition NewRequisition() => PurchaseRequisition.Create(createUserId: 1, createDate: CreateDate);

    private static PurchaseRequisition NewRequisitionWithLine(decimal quantity = 10)
    {
        var requisition = NewRequisition();
        requisition.AddLine(productId: 5, unitId: 1, quantity: quantity);
        return requisition;
    }

    // ----- Creation -----

    [Fact]
    public void Create_requisition_with_valid_data()
    {
        var requisition = NewRequisition();

        Assert.Equal(Status.New, requisition.Status);
        Assert.Empty(requisition.PurchaseRequisitionProducts);
        Assert.Contains(requisition.DomainEvents, e => e is PurchaseRequisitionCreatedDomainEvent);
    }

    // ----- Lines -----

    [Fact]
    public void Add_remove_line()
    {
        var requisition = NewRequisition();
        var line = requisition.AddLine(5, 1, 10);
        requisition.AddLine(6, 1, 1);

        Assert.Equal(2, requisition.PurchaseRequisitionProducts.Count);

        requisition.RemoveLine(line.Id);

        Assert.Single(requisition.PurchaseRequisitionProducts);
    }

    [Fact]
    public void Cannot_add_line_with_zero_quantity()
    {
        var requisition = NewRequisition();
        Assert.Throws<InvalidPurchaseRequisitionLineException>(() => requisition.AddLine(5, 1, 0));
    }

    [Fact]
    public void Cannot_edit_lines_after_submit()
    {
        var requisition = NewRequisitionWithLine();
        requisition.Submit();

        Assert.Throws<PurchaseRequisitionNotEditableException>(() => requisition.AddLine(6, 1, 1));
        Assert.Throws<PurchaseRequisitionNotEditableException>(() => requisition.RemoveLine(requisition.PurchaseRequisitionProducts.First().Id));
    }

    // ----- Submit -----

    [Fact]
    public void Cannot_submit_empty_requisition() =>
        Assert.Throws<PurchaseRequisitionNotSubmittableException>(() => NewRequisition().Submit());

    [Fact]
    public void Submit_requisition_with_lines()
    {
        var requisition = NewRequisitionWithLine();

        requisition.Submit();

        Assert.Equal(Status.UnderReview, requisition.Status);
        Assert.Contains(requisition.DomainEvents, e => e is PurchaseRequisitionSubmittedDomainEvent);
    }

    [Fact]
    public void Cannot_submit_twice()
    {
        var requisition = NewRequisitionWithLine();
        requisition.Submit();

        Assert.Throws<PurchaseRequisitionNotSubmittableException>(() => requisition.Submit());
    }

    // ----- Approve (via RecordSourced) -----

    [Fact]
    public void Approve_via_record_sourced()
    {
        var requisition = NewRequisitionWithLine(10);
        requisition.Submit();
        var lineId = requisition.PurchaseRequisitionProducts.First().Id;

        requisition.RecordSourced(new Dictionary<long, decimal> { [lineId] = 10 });

        Assert.Equal(Status.Approved, requisition.Status);
        Assert.Equal(10, requisition.PurchaseRequisitionProducts.First().OrderedQuantity);
        Assert.Contains(requisition.DomainEvents, e => e is PurchaseRequisitionApprovedDomainEvent);
    }

    [Fact]
    public void Cannot_source_draft_requisition()
    {
        var requisition = NewRequisitionWithLine(10);
        var lineId = requisition.PurchaseRequisitionProducts.First().Id;

        Assert.Throws<PurchaseRequisitionCannotBeSourcedException>(() =>
            requisition.RecordSourced(new Dictionary<long, decimal> { [lineId] = 10 }));
    }

    [Fact]
    public void Cannot_source_twice()
    {
        var requisition = NewRequisitionWithLine(10);
        requisition.Submit();
        var lineId = requisition.PurchaseRequisitionProducts.First().Id;
        requisition.RecordSourced(new Dictionary<long, decimal> { [lineId] = 10 });

        Assert.Throws<PurchaseRequisitionCannotBeSourcedException>(() =>
            requisition.RecordSourced(new Dictionary<long, decimal> { [lineId] = 0 }));
    }

    // ----- Partial sourcing -----

    [Fact]
    public void Partial_sourcing_leaves_remaining_quantity()
    {
        var requisition = NewRequisitionWithLine(100);
        requisition.Submit();
        var lineId = requisition.PurchaseRequisitionProducts.First().Id;

        requisition.RecordSourced(new Dictionary<long, decimal> { [lineId] = 60 });

        var line = requisition.PurchaseRequisitionProducts.First();
        Assert.Equal(60, line.OrderedQuantity);
        Assert.Equal(40, line.RemainingQuantity);
    }

    [Fact]
    public void Cannot_source_more_than_requested()
    {
        var requisition = NewRequisitionWithLine(100);
        requisition.Submit();
        var lineId = requisition.PurchaseRequisitionProducts.First().Id;

        Assert.Throws<InvalidPurchaseRequisitionLineException>(() =>
            requisition.RecordSourced(new Dictionary<long, decimal> { [lineId] = 100.01m }));
    }

    // ----- Reject -----

    [Fact]
    public void Reject_underreview_requisition()
    {
        var requisition = NewRequisitionWithLine();
        requisition.Submit();

        requisition.Reject();

        Assert.Equal(Status.Rejected, requisition.Status);
        Assert.Contains(requisition.DomainEvents, e => e is PurchaseRequisitionRejectedDomainEvent);
    }

    [Fact]
    public void Cannot_reject_draft_requisition() =>
        Assert.Throws<PurchaseRequisitionNotOpenException>(() => NewRequisitionWithLine().Reject());

    // ----- Cancel -----

    [Fact]
    public void Cancel_draft_requisition()
    {
        var requisition = NewRequisitionWithLine();

        requisition.Cancel();

        Assert.Equal(Status.Cancel, requisition.Status);
        Assert.Contains(requisition.DomainEvents, e => e is PurchaseRequisitionCancelledDomainEvent);
    }

    [Fact]
    public void Cancel_underreview_requisition()
    {
        var requisition = NewRequisitionWithLine();
        requisition.Submit();

        requisition.Cancel();

        Assert.Equal(Status.Cancel, requisition.Status);
    }

    [Fact]
    public void Cancel_is_idempotent()
    {
        var requisition = NewRequisitionWithLine();
        requisition.Cancel();
        requisition.ClearDomainEvents();

        requisition.Cancel();

        Assert.Empty(requisition.DomainEvents);
    }

    [Fact]
    public void Cannot_cancel_approved_requisition()
    {
        var requisition = NewRequisitionWithLine(10);
        requisition.Submit();
        var lineId = requisition.PurchaseRequisitionProducts.First().Id;
        requisition.RecordSourced(new Dictionary<long, decimal> { [lineId] = 10 });

        Assert.Throws<PurchaseRequisitionCannotBeCancelledException>(() => requisition.Cancel());
    }
}

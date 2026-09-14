namespace Purchasing.Domain.Tests;

public class PurchaseOrderTests
{
    private static readonly DateTime CreateDate = new(2026, 9, 1);

    private static PurchaseOrder NewOrder() => PurchaseOrder.Create(dealerId: 1, createUserId: 1, createDate: CreateDate);

    private static (PurchaseOrder Order, PurchaseOrderProduct Line) NewOrderWithLine(decimal quantity = 100, decimal price = 10)
    {
        var order = NewOrder();
        var line = order.AddLine(productId: 5, unitId: 1, quantity: quantity, price: price);
        return (order, line);
    }

    // ----- Creation -----

    [Fact]
    public void Create_direct_purchase_order()
    {
        var order = NewOrder();

        Assert.Equal(1, order.DealerId);
        Assert.Equal(Status.New, order.Status);
        Assert.Null(order.PurchaseRequisitionId);
        Assert.Contains(order.DomainEvents, e => e is PurchaseOrderCreatedDomainEvent);
    }

    [Fact]
    public void Create_from_requisition_records_source()
    {
        var order = PurchaseOrder.Create(1, 1, CreateDate, purchaseRequisitionId: 77);

        Assert.Equal(77, order.PurchaseRequisitionId);
    }

    [Fact]
    public void Cannot_create_without_supplier() =>
        Assert.Throws<InvalidPurchaseOrderException>(() => PurchaseOrder.Create(0, 1, CreateDate));

    // ----- Lines / totals -----

    [Fact]
    public void Add_line_calculates_total()
    {
        var order = NewOrder();

        var line = order.AddLine(5, 1, quantity: 10, price: 25);

        Assert.Equal(250, line.Total);
    }

    [Fact]
    public void Totals_are_domain_calculated_not_trusted_from_caller()
    {
        var order = NewOrder();
        order.AddLine(5, 1, quantity: 10, price: 100);
        order.AddLine(6, 1, quantity: 2, price: 50);

        Assert.Equal(1000 + 100, order.Total);
        Assert.Equal(order.Total - order.Discount + order.Tax, order.Net);
    }

    [Fact]
    public void Cannot_issue_empty_purchase_order()
    {
        var order = NewOrder();
        Assert.Empty(order.PurchaseOrderProducts);
    }

    [Fact]
    public void Cannot_add_line_with_zero_quantity()
    {
        var order = NewOrder();
        Assert.Throws<InvalidPurchaseOrderLineException>(() => order.AddLine(5, 1, quantity: 0, price: 10));
    }

    [Fact]
    public void Cannot_add_line_with_negative_price()
    {
        var order = NewOrder();
        Assert.Throws<InvalidPurchaseOrderLineException>(() => order.AddLine(5, 1, quantity: 1, price: -1));
    }

    [Fact]
    public void Cannot_edit_lines_after_linking_invoice()
    {
        var (order, line) = NewOrderWithLine();
        order.LinkInvoice(500);

        Assert.Throws<PurchaseOrderNotEditableException>(() => order.AddLine(6, 1, 1, 10));
        Assert.Throws<PurchaseOrderNotEditableException>(() => order.RemoveLine(line.Id));
        Assert.Throws<PurchaseOrderNotEditableException>(() => order.UpdateHeader(0, 0, 0, 0, null));
    }

    // ----- LinkInvoice -----

    [Fact]
    public void Link_invoice_approves_order()
    {
        var (order, _) = NewOrderWithLine();

        order.LinkInvoice(500);

        Assert.Equal(500, order.InvoiceId);
        Assert.Equal(Status.Approved, order.Status);
        Assert.Contains(order.DomainEvents, e => e is PurchaseOrderLinkedToInvoiceDomainEvent);
    }

    [Fact]
    public void Cannot_link_invoice_twice()
    {
        var (order, _) = NewOrderWithLine();
        order.LinkInvoice(500);

        Assert.Throws<PurchaseOrderAlreadyLinkedException>(() => order.LinkInvoice(501));
    }

    // ----- Receipt -----

    [Fact]
    public void Cannot_receive_against_cancelled_order()
    {
        var (order, line) = NewOrderWithLine();
        order.Cancel();

        Assert.Throws<PurchaseOrderNotOpenException>(() => order.RecordReceipt(line.Id, 1));
    }

    [Fact]
    public void Partial_receipt_reduces_remaining()
    {
        var (order, line) = NewOrderWithLine(quantity: 100);

        order.RecordReceipt(line.Id, 40);

        Assert.Equal(40, line.ReceivedQuantity);
        Assert.Equal(60, line.RemainingQuantity);
        Assert.Contains(order.DomainEvents, e => e is PurchaseOrderPartiallyReceivedDomainEvent);
    }

    [Fact]
    public void Multiple_receipts_accumulate()
    {
        var (order, line) = NewOrderWithLine(quantity: 100);

        order.RecordReceipt(line.Id, 40);
        order.RecordReceipt(line.Id, 30);

        Assert.Equal(70, line.ReceivedQuantity);
        Assert.Equal(30, line.RemainingQuantity);
    }

    [Fact]
    public void Full_receipt_completes_order()
    {
        var (order, line) = NewOrderWithLine(quantity: 100);

        order.RecordReceipt(line.Id, 100);

        Assert.Equal(0, line.RemainingQuantity);
        Assert.Contains(order.DomainEvents, e => e is PurchaseOrderCompletedDomainEvent);
    }

    [Fact]
    public void Over_receipt_is_blocked()
    {
        var (order, line) = NewOrderWithLine(quantity: 100);
        order.RecordReceipt(line.Id, 90);

        Assert.Throws<InvalidPurchaseOrderReceiptException>(() => order.RecordReceipt(line.Id, 10.01m));
    }

    [Fact]
    public void Cannot_record_zero_or_negative_receipt()
    {
        var (order, line) = NewOrderWithLine();

        Assert.Throws<InvalidPurchaseOrderReceiptException>(() => order.RecordReceipt(line.Id, 0));
        Assert.Throws<InvalidPurchaseOrderReceiptException>(() => order.RecordReceipt(line.Id, -1));
    }

    // ----- Return -----

    [Fact]
    public void Partial_return_reduces_returnable_quantity()
    {
        var (order, line) = NewOrderWithLine(quantity: 100);
        order.RecordReceipt(line.Id, 80);

        order.RecordReturn(line.Id, 15);

        Assert.Equal(15, line.ReturnedQuantity);
        Assert.Equal(65, line.ReturnableQuantity);
    }

    [Fact]
    public void Cannot_return_more_than_received_minus_already_returned()
    {
        var (order, line) = NewOrderWithLine(quantity: 100);
        order.RecordReceipt(line.Id, 80);
        order.RecordReturn(line.Id, 60);

        Assert.Throws<InvalidPurchaseOrderReturnException>(() => order.RecordReturn(line.Id, 20.01m));
    }

    [Fact]
    public void Cannot_return_item_that_was_never_received()
    {
        var (order, line) = NewOrderWithLine(quantity: 100);

        Assert.Throws<InvalidPurchaseOrderReturnException>(() => order.RecordReturn(line.Id, 1));
    }

    // ----- Cancellation -----

    [Fact]
    public void Cancel_untouched_order()
    {
        var (order, _) = NewOrderWithLine();

        order.Cancel();

        Assert.Equal(Status.Cancel, order.Status);
        Assert.Contains(order.DomainEvents, e => e is PurchaseOrderCancelledDomainEvent);
    }

    [Fact]
    public void Cancel_is_idempotent()
    {
        var (order, _) = NewOrderWithLine();
        order.Cancel();
        order.ClearDomainEvents();

        order.Cancel();

        Assert.Empty(order.DomainEvents);
    }

    [Fact]
    public void Cannot_cancel_order_with_receipt_history()
    {
        var (order, line) = NewOrderWithLine();
        order.RecordReceipt(line.Id, 1);

        Assert.Throws<PurchaseOrderCannotBeCancelledException>(() => order.Cancel());
    }

    [Fact]
    public void Cannot_cancel_order_linked_to_invoice()
    {
        var (order, _) = NewOrderWithLine();
        order.LinkInvoice(500);

        Assert.Throws<PurchaseOrderCannotBeCancelledException>(() => order.Cancel());
    }

    [Fact]
    public void Partial_line_cancellation_leaves_remainder_receivable()
    {
        var (order, line) = NewOrderWithLine(quantity: 100);
        order.RecordReceipt(line.Id, 40);

        order.CancelLine(line.Id, 30);

        Assert.Equal(40, line.ReceivedQuantity);
        Assert.Equal(30, line.CancelledQuantity);
        Assert.Equal(30, line.RemainingQuantity);

        order.RecordReceipt(line.Id, 30);
        Assert.Equal(0, line.RemainingQuantity);
    }

    [Fact]
    public void Cannot_cancel_more_than_remaining_on_a_line()
    {
        var (order, line) = NewOrderWithLine(quantity: 100);
        order.RecordReceipt(line.Id, 40);

        Assert.Throws<InvalidPurchaseOrderLineException>(() => order.CancelLine(line.Id, 60.01m));
    }

    // ----- Fulfillment read-back (brief §86) -----

    [Fact]
    public void Fulfillment_quantities_reflect_ordered_received_returned_cancelled()
    {
        var (order, line) = NewOrderWithLine(quantity: 100);

        order.RecordReceipt(line.Id, 70);
        order.RecordReturn(line.Id, 10);
        order.CancelLine(line.Id, 20);

        Assert.Equal(100, line.Quantity);
        Assert.Equal(70, line.ReceivedQuantity);
        Assert.Equal(10, line.ReturnedQuantity);
        Assert.Equal(20, line.CancelledQuantity);
        Assert.Equal(10, line.RemainingQuantity);
    }
}

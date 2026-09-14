namespace Sales.Domain.Tests;

public class SalesOrderTests
{
    private static readonly DateTime OrderDate = new(2026, 9, 1);

    private static SalesOrder NewOrder() => SalesOrder.Create(
        customerId: 1,
        currencyId: 1,
        rate: 1,
        orderDate: OrderDate,
        createUserId: 1,
        createDate: OrderDate);

    private static (SalesOrder Order, SalesOrderLine Line) NewConfirmedOrder(decimal quantity = 100, decimal unitPrice = 10)
    {
        var order = NewOrder();
        var line = order.AddLine(productId: 5, productName: "Widget", unitId: 1, orderedQuantity: quantity, unitPrice: unitPrice);
        order.Confirm();
        return (order, line);
    }

    // ----- Creation -----

    [Fact]
    public void Create_sales_order_with_valid_data()
    {
        var order = NewOrder();

        Assert.Equal(1, order.CustomerId);
        Assert.Equal(SalesOrderStatus.Draft, order.LifecycleStatus);
        Assert.Null(order.SourceQuotationId);
        Assert.Contains(order.DomainEvents, e => e is SalesOrderCreatedDomainEvent);
    }

    [Fact]
    public void Create_from_quotation_records_source()
    {
        var order = SalesOrder.Create(1, 1, 1, OrderDate, 1, OrderDate, sourceQuotationId: 77);

        Assert.Equal(77, order.SourceQuotationId);
    }

    [Fact]
    public void Cannot_create_sales_order_with_zero_rate() =>
        Assert.Throws<InvalidSalesOrderLineException>(() => SalesOrder.Create(1, 1, 0, OrderDate, 1, OrderDate));

    // ----- Lines / totals -----

    [Fact]
    public void Add_remove_line_recalculates_totals()
    {
        var order = NewOrder();
        var line = order.AddLine(5, "Widget", 1, orderedQuantity: 10, unitPrice: 100, discountAmount: 20, taxAmount: 5);
        order.AddLine(6, "Gadget", 1, orderedQuantity: 1, unitPrice: 50);

        Assert.Equal(1000 + 50, order.Subtotal);

        order.RemoveLine(line.Id);

        Assert.Single(order.Lines);
        Assert.Equal(50, order.Subtotal);
    }

    [Fact]
    public void Cannot_add_line_with_zero_quantity()
    {
        var order = NewOrder();
        Assert.Throws<InvalidSalesOrderLineException>(() => order.AddLine(5, "Widget", 1, orderedQuantity: 0, unitPrice: 100));
    }

    // ----- Confirm -----

    [Fact]
    public void Cannot_confirm_empty_order() =>
        Assert.Throws<SalesOrderNotConfirmableException>(() => NewOrder().Confirm());

    [Fact]
    public void Confirm_order_with_lines()
    {
        var order = NewOrder();
        order.AddLine(5, "Widget", 1, 10, 100);

        order.Confirm();

        Assert.Equal(SalesOrderStatus.Confirmed, order.LifecycleStatus);
        Assert.Contains(order.DomainEvents, e => e is SalesOrderConfirmedDomainEvent);
    }

    [Fact]
    public void Cannot_edit_confirmed_order_lines()
    {
        var (order, line) = NewConfirmedOrder();

        Assert.Throws<SalesOrderNotEditableException>(() => order.AddLine(6, "Gadget", 1, 1, 50));
        Assert.Throws<SalesOrderNotEditableException>(() => order.RemoveLine(line.Id));
    }

    // ----- Delivery -----

    [Fact]
    public void Cannot_deliver_unconfirmed_order()
    {
        var order = NewOrder();
        var line = order.AddLine(5, "Widget", 1, 10, 100);

        Assert.Throws<InvalidSalesOrderDeliveryException>(() => order.RecordDelivery(line.Id, 5));
    }

    [Fact]
    public void Partial_delivery_marks_order_partially_delivered()
    {
        var (order, line) = NewConfirmedOrder(quantity: 100);

        order.RecordDelivery(line.Id, 40);

        Assert.Equal(40, line.DeliveredQuantity);
        Assert.Equal(60, line.RemainingQuantity);
        Assert.Equal(SalesOrderStatus.PartiallyDelivered, order.LifecycleStatus);
    }

    [Fact]
    public void Multiple_deliveries_accumulate()
    {
        var (order, line) = NewConfirmedOrder(quantity: 100);

        order.RecordDelivery(line.Id, 40);
        order.RecordDelivery(line.Id, 30);

        Assert.Equal(70, line.DeliveredQuantity);
        Assert.Equal(30, line.RemainingQuantity);
        Assert.Equal(SalesOrderStatus.PartiallyDelivered, order.LifecycleStatus);
    }

    [Fact]
    public void Full_delivery_completes_order()
    {
        var (order, line) = NewConfirmedOrder(quantity: 100);

        order.RecordDelivery(line.Id, 100);

        Assert.Equal(SalesOrderStatus.Delivered, order.LifecycleStatus);
        Assert.Contains(order.DomainEvents, e => e is SalesOrderCompletedDomainEvent);
    }

    [Fact]
    public void Cannot_exceed_ordered_quantity_on_delivery()
    {
        var (order, line) = NewConfirmedOrder(quantity: 100);
        order.RecordDelivery(line.Id, 90);

        Assert.Throws<InvalidSalesOrderDeliveryException>(() => order.RecordDelivery(line.Id, 10.01m));
    }

    [Fact]
    public void Cannot_deliver_cancelled_order()
    {
        var (order, line) = NewConfirmedOrder();
        order.Cancel();

        Assert.Throws<InvalidSalesOrderDeliveryException>(() => order.RecordDelivery(line.Id, 1));
    }

    [Fact]
    public void Cannot_deliver_already_delivered_order()
    {
        var (order, line) = NewConfirmedOrder(quantity: 10);
        order.RecordDelivery(line.Id, 10);

        Assert.Throws<InvalidSalesOrderDeliveryException>(() => order.RecordDelivery(line.Id, 1));
    }

    [Fact]
    public void Cannot_record_zero_or_negative_delivery()
    {
        var (order, line) = NewConfirmedOrder();

        Assert.Throws<InvalidSalesOrderDeliveryException>(() => order.RecordDelivery(line.Id, 0));
        Assert.Throws<InvalidSalesOrderDeliveryException>(() => order.RecordDelivery(line.Id, -1));
    }

    // ----- Return -----

    [Fact]
    public void Partial_return_reduces_returnable_quantity()
    {
        var (order, line) = NewConfirmedOrder(quantity: 100);
        order.RecordDelivery(line.Id, 80);

        order.RecordReturn(line.Id, 20);

        Assert.Equal(20, line.ReturnedQuantity);
        Assert.Equal(60, line.ReturnableQuantity);
    }

    [Fact]
    public void Multiple_returns_accumulate()
    {
        var (order, line) = NewConfirmedOrder(quantity: 100);
        order.RecordDelivery(line.Id, 80);

        order.RecordReturn(line.Id, 20);
        order.RecordReturn(line.Id, 10);

        Assert.Equal(30, line.ReturnedQuantity);
    }

    [Fact]
    public void Cannot_return_more_than_delivered_minus_already_returned()
    {
        var (order, line) = NewConfirmedOrder(quantity: 100);
        order.RecordDelivery(line.Id, 80);
        order.RecordReturn(line.Id, 60);

        Assert.Throws<InvalidSalesOrderReturnException>(() => order.RecordReturn(line.Id, 20.01m));
    }

    [Fact]
    public void Cannot_return_item_that_was_never_delivered()
    {
        var (order, line) = NewConfirmedOrder(quantity: 100);

        Assert.Throws<InvalidSalesOrderReturnException>(() => order.RecordReturn(line.Id, 1));
    }

    [Fact]
    public void Cannot_return_against_draft_order()
    {
        var order = NewOrder();
        var line = order.AddLine(5, "Widget", 1, 10, 100);

        Assert.Throws<InvalidSalesOrderReturnException>(() => order.RecordReturn(line.Id, 1));
    }

    // ----- Cancellation -----

    [Fact]
    public void Complete_cancellation_before_delivery_is_clean_cancel()
    {
        var (order, _) = NewConfirmedOrder();

        order.Cancel();

        Assert.Equal(SalesOrderStatus.Cancelled, order.LifecycleStatus);
        Assert.Contains(order.DomainEvents, e => e is SalesOrderCancelledDomainEvent);
    }

    [Fact]
    public void Cancelling_remainder_after_partial_delivery_preserves_delivered_history()
    {
        var (order, line) = NewConfirmedOrder(quantity: 100);
        order.RecordDelivery(line.Id, 40);

        order.Cancel();

        Assert.Equal(40, line.DeliveredQuantity);
        Assert.Equal(60, line.CancelledQuantity);
        Assert.Equal(0, line.RemainingQuantity);
        Assert.Equal(SalesOrderStatus.Delivered, order.LifecycleStatus);
        Assert.Contains(order.DomainEvents, e => e is SalesOrderCompletedDomainEvent);
        Assert.DoesNotContain(order.DomainEvents, e => e is SalesOrderCancelledDomainEvent);
    }

    [Fact]
    public void Cannot_cancel_delivered_order_silently()
    {
        var (order, line) = NewConfirmedOrder(quantity: 10);
        order.RecordDelivery(line.Id, 10);

        Assert.Throws<SalesOrderCannotBeCancelledException>(() => order.Cancel());
    }

    [Fact]
    public void Cannot_cancel_already_cancelled_order()
    {
        var (order, _) = NewConfirmedOrder();
        order.Cancel();

        Assert.Throws<SalesOrderCannotBeCancelledException>(() => order.Cancel());
    }

    [Fact]
    public void Partial_line_cancellation_leaves_remainder_deliverable()
    {
        var (order, line) = NewConfirmedOrder(quantity: 100);
        order.RecordDelivery(line.Id, 40);

        order.CancelLine(line.Id, 30);

        Assert.Equal(40, line.DeliveredQuantity);
        Assert.Equal(30, line.CancelledQuantity);
        Assert.Equal(30, line.RemainingQuantity);
        Assert.Equal(SalesOrderStatus.PartiallyDelivered, order.LifecycleStatus);

        order.RecordDelivery(line.Id, 30);
        Assert.Equal(SalesOrderStatus.Delivered, order.LifecycleStatus);
    }

    [Fact]
    public void Cannot_cancel_more_than_remaining_on_a_line()
    {
        var (order, line) = NewConfirmedOrder(quantity: 100);
        order.RecordDelivery(line.Id, 40);

        Assert.Throws<InvalidSalesOrderLineException>(() => order.CancelLine(line.Id, 60.01m));
    }

    // ----- Fulfillment read-back (brief §65) -----

    [Fact]
    public void Fulfillment_quantities_reflect_ordered_delivered_returned_cancelled()
    {
        var (order, line) = NewConfirmedOrder(quantity: 100);

        order.RecordDelivery(line.Id, 70);
        order.RecordReturn(line.Id, 10);
        order.CancelLine(line.Id, 20);

        Assert.Equal(100, line.OrderedQuantity);
        Assert.Equal(70, line.DeliveredQuantity);
        Assert.Equal(10, line.ReturnedQuantity);
        Assert.Equal(20, line.CancelledQuantity);
        Assert.Equal(10, line.RemainingQuantity);
    }
}

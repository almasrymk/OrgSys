namespace Sales.Domain.Tests;

public class QuotationTests
{
    private static readonly DateTime QuotationDate = new(2026, 9, 1);
    private static readonly DateTime ValidUntil = new(2026, 9, 30);

    private static Quotation NewQuotation() => Quotation.Create(
        customerId: 1,
        currencyId: 1,
        rate: 1,
        quotationDate: QuotationDate,
        validUntil: ValidUntil,
        createUserId: 1,
        createDate: QuotationDate);

    private static Quotation NewQuotationWithLine(decimal quantity = 10, decimal unitPrice = 100)
    {
        var quotation = NewQuotation();
        quotation.AddLine(productId: 5, productName: "Widget", unitId: 1, quantity: quantity, unitPrice: unitPrice);
        return quotation;
    }

    // ----- Creation -----

    [Fact]
    public void Create_quotation_with_valid_data()
    {
        var quotation = NewQuotation();

        Assert.Equal(1, quotation.CustomerId);
        Assert.Equal(QuotationStatus.Draft, quotation.LifecycleStatus);
        Assert.Empty(quotation.Lines);
        Assert.Contains(quotation.DomainEvents, e => e is QuotationCreatedDomainEvent);
    }

    [Fact]
    public void Cannot_create_quotation_with_valid_until_before_quotation_date() =>
        Assert.Throws<InvalidQuotationLineException>(() =>
            Quotation.Create(1, 1, 1, QuotationDate, QuotationDate.AddDays(-1), 1, QuotationDate));

    [Fact]
    public void Cannot_create_quotation_with_zero_rate() =>
        Assert.Throws<InvalidQuotationLineException>(() =>
            Quotation.Create(1, 1, 0, QuotationDate, ValidUntil, 1, QuotationDate));

    // ----- Lines / totals -----

    [Fact]
    public void Add_line_calculates_line_total()
    {
        var quotation = NewQuotation();

        var line = quotation.AddLine(5, "Widget", 1, quantity: 10, unitPrice: 100, discountAmount: 50, taxAmount: 20);

        Assert.Equal(10 * 100m - 50 + 20, line.LineTotal);
    }

    [Fact]
    public void Add_line_recalculates_quotation_totals()
    {
        var quotation = NewQuotation();

        quotation.AddLine(5, "Widget", 1, quantity: 10, unitPrice: 100, discountAmount: 50, taxAmount: 20);
        quotation.AddLine(6, "Gadget", 1, quantity: 2, unitPrice: 200, discountAmount: 0, taxAmount: 10);

        Assert.Equal(1000 + 400, quotation.Subtotal);
        Assert.Equal(50, quotation.DiscountAmount);
        Assert.Equal(30, quotation.TaxAmount);
        Assert.Equal(1400 - 50 + 30, quotation.TotalAmount);
    }

    [Fact]
    public void Remove_line_recalculates_totals()
    {
        var quotation = NewQuotation();
        var line = quotation.AddLine(5, "Widget", 1, quantity: 10, unitPrice: 100);
        quotation.AddLine(6, "Gadget", 1, quantity: 1, unitPrice: 50);

        quotation.RemoveLine(line.Id);

        Assert.Single(quotation.Lines);
        Assert.Equal(50, quotation.Subtotal);
    }

    [Fact]
    public void Cannot_add_line_with_zero_quantity()
    {
        var quotation = NewQuotation();
        Assert.Throws<InvalidQuotationLineException>(() => quotation.AddLine(5, "Widget", 1, quantity: 0, unitPrice: 100));
    }

    [Fact]
    public void Cannot_add_line_with_negative_unit_price()
    {
        var quotation = NewQuotation();
        Assert.Throws<InvalidQuotationLineException>(() => quotation.AddLine(5, "Widget", 1, quantity: 1, unitPrice: -1));
    }

    [Fact]
    public void Cannot_edit_lines_after_sending()
    {
        var quotation = NewQuotationWithLine();
        quotation.Send();

        Assert.Throws<QuotationNotEditableException>(() => quotation.AddLine(6, "Gadget", 1, 1, 50));
        Assert.Throws<QuotationNotEditableException>(() => quotation.RemoveLine(quotation.Lines.First().Id));
    }

    // ----- Send -----

    [Fact]
    public void Cannot_send_empty_quotation() =>
        Assert.Throws<QuotationNotSendableException>(() => NewQuotation().Send());

    [Fact]
    public void Send_quotation_with_lines()
    {
        var quotation = NewQuotationWithLine();

        quotation.Send();

        Assert.Equal(QuotationStatus.Sent, quotation.LifecycleStatus);
        Assert.Contains(quotation.DomainEvents, e => e is QuotationSentDomainEvent);
    }

    [Fact]
    public void Cannot_send_already_sent_quotation()
    {
        var quotation = NewQuotationWithLine();
        quotation.Send();

        Assert.Throws<QuotationNotSendableException>(() => quotation.Send());
    }

    // ----- Accept / Reject -----

    [Fact]
    public void Accept_sent_quotation()
    {
        var quotation = NewQuotationWithLine();
        quotation.Send();

        quotation.Accept();

        Assert.Equal(QuotationStatus.Accepted, quotation.LifecycleStatus);
        Assert.Contains(quotation.DomainEvents, e => e is QuotationAcceptedDomainEvent);
    }

    [Fact]
    public void Cannot_accept_draft_quotation() =>
        Assert.Throws<QuotationNotOpenException>(() => NewQuotationWithLine().Accept());

    [Fact]
    public void Reject_sent_quotation()
    {
        var quotation = NewQuotationWithLine();
        quotation.Send();

        quotation.Reject();

        Assert.Equal(QuotationStatus.Rejected, quotation.LifecycleStatus);
        Assert.Contains(quotation.DomainEvents, e => e is QuotationRejectedDomainEvent);
    }

    // ----- Expire -----

    [Fact]
    public void Expire_sent_quotation_past_valid_until()
    {
        var quotation = NewQuotationWithLine();
        quotation.Send();

        quotation.Expire(ValidUntil.AddDays(1));

        Assert.Equal(QuotationStatus.Expired, quotation.LifecycleStatus);
        Assert.Contains(quotation.DomainEvents, e => e is QuotationExpiredDomainEvent);
    }

    [Fact]
    public void Cannot_expire_before_valid_until_date()
    {
        var quotation = NewQuotationWithLine();
        quotation.Send();

        Assert.Throws<QuotationNotYetExpiredException>(() => quotation.Expire(ValidUntil.AddDays(-1)));
    }

    [Fact]
    public void IsPastValidUntil_reflects_expiry_before_Expire_is_called()
    {
        var quotation = NewQuotationWithLine();
        quotation.Send();

        Assert.True(quotation.IsPastValidUntil(ValidUntil.AddDays(1)));
        Assert.False(quotation.IsPastValidUntil(ValidUntil.AddDays(-1)));
    }

    // ----- Cancel -----

    [Fact]
    public void Cancel_draft_quotation()
    {
        var quotation = NewQuotationWithLine();

        quotation.Cancel();

        Assert.Equal(QuotationStatus.Cancelled, quotation.LifecycleStatus);
        Assert.Contains(quotation.DomainEvents, e => e is QuotationCancelledDomainEvent);
    }

    [Fact]
    public void Cancel_sent_quotation()
    {
        var quotation = NewQuotationWithLine();
        quotation.Send();

        quotation.Cancel();

        Assert.Equal(QuotationStatus.Cancelled, quotation.LifecycleStatus);
    }

    [Fact]
    public void Cancel_is_idempotent()
    {
        var quotation = NewQuotationWithLine();
        quotation.Cancel();
        quotation.ClearDomainEvents();

        quotation.Cancel();

        Assert.Empty(quotation.DomainEvents);
    }

    [Fact]
    public void Cannot_cancel_accepted_quotation()
    {
        var quotation = NewQuotationWithLine();
        quotation.Send();
        quotation.Accept();

        Assert.Throws<QuotationCannotBeCancelledException>(() => quotation.Cancel());
    }

    // ----- Convert -----

    [Fact]
    public void Convert_accepted_quotation()
    {
        var quotation = NewQuotationWithLine();
        quotation.Send();
        quotation.Accept();

        quotation.MarkConverted(salesOrderId: 900);

        Assert.Equal(QuotationStatus.Converted, quotation.LifecycleStatus);
        Assert.Equal(900, quotation.ConvertedToSalesOrderId);
        Assert.Contains(quotation.DomainEvents, e => e is QuotationConvertedDomainEvent);
    }

    [Fact]
    public void Cannot_convert_quotation_that_was_not_accepted() =>
        Assert.Throws<QuotationCannotBeConvertedException>(() => NewQuotationWithLine().MarkConverted(900));

    [Fact]
    public void Cannot_convert_twice()
    {
        var quotation = NewQuotationWithLine();
        quotation.Send();
        quotation.Accept();
        quotation.MarkConverted(900);

        Assert.Throws<QuotationCannotBeConvertedException>(() => quotation.MarkConverted(901));
    }
}

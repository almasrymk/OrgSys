namespace Payables.Domain.Tests;

public class SupplierPaymentApplicationTests
{
    private static readonly DateTime PaymentDate = new(2026, 9, 10);

    private static SupplierPaymentApplication NewApplication(decimal amount = 1000) =>
        SupplierPaymentApplication.Create(sourceFinancialId: 500, supplierId: 1, amount: amount, createUserId: 1, createDate: PaymentDate);

    [Fact]
    public void Create_payment_application_with_valid_data()
    {
        var application = NewApplication(1000);

        Assert.Equal(500, application.SourceFinancialId);
        Assert.Equal(1, application.SupplierId);
        Assert.Equal(1000, application.Amount);
        Assert.Equal(1000, application.UnappliedAmount);
        Assert.Empty(application.Lines);
    }

    [Fact]
    public void Cannot_create_payment_application_with_zero_or_negative_amount()
    {
        Assert.Throws<InvalidAllocationAmountException>(() => NewApplication(0));
        Assert.Throws<InvalidAllocationAmountException>(() => NewApplication(-1));
    }

    [Fact]
    public void RecordLine_reduces_unapplied_amount()
    {
        var application = NewApplication(1000);

        application.RecordLine(payableId: 10, appliedAmount: 400);

        Assert.Equal(600, application.UnappliedAmount);
        var line = Assert.Single(application.Lines);
        Assert.Equal(10, line.PayableId);
        Assert.Equal(400, line.Amount);
    }

    [Fact]
    public void RecordLine_can_split_across_multiple_payables()
    {
        var application = NewApplication(1000);

        application.RecordLine(10, 400);
        application.RecordLine(20, 600);

        Assert.Equal(0, application.UnappliedAmount);
        Assert.Equal(2, application.Lines.Count);
    }

    [Fact]
    public void Cannot_record_zero_or_negative_line_amount()
    {
        var application = NewApplication(1000);

        Assert.Throws<InvalidAllocationAmountException>(() => application.RecordLine(10, 0));
        Assert.Throws<InvalidAllocationAmountException>(() => application.RecordLine(10, -1));
    }

    [Fact]
    public void Cannot_record_line_beyond_unapplied_amount()
    {
        var application = NewApplication(1000);
        application.RecordLine(10, 700);

        Assert.Throws<InvalidAllocationAmountException>(() => application.RecordLine(20, 300.01m));
    }
}

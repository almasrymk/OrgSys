namespace Application.DTOs;

/// <summary>Request shape for posting money paid to a Supplier. Mirrors <c>PostCustomerReceiptDto</c>
/// on the AR side — same fields, opposite cash direction.</summary>
public sealed class PostSupplierPaymentDto
{
    public long DealerId { get; set; }
    public long FinancialAccountId { get; set; }
    public decimal Amount { get; set; }
    public long CurrencyId { get; set; }
    public decimal ExchangeRate { get; set; } = 1;
    public DateTime Date { get; set; }
    public string? ReferenceNumber { get; set; }
    public string? Notes { get; set; }
    public long CreateUserId { get; set; }
    public long? BranchId { get; set; }
    public long? ShiftId { get; set; }
}

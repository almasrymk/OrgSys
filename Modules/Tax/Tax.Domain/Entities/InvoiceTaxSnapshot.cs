namespace Tax.Domain;

[Table("InvoiceTaxSnapshot")]
public class InvoiceTaxSnapshot : BaseModel
{
    private readonly List<InvoiceTaxSnapshotLine> _lines = [];

    public virtual long InvoiceId { get; private set; }

    [StringLength(50)]
    public virtual string? InvoiceNumber { get; private set; }

    public virtual long InvoiceTypeId { get; private set; }

    public virtual int TaxType { get; private set; }

    [Column(TypeName = "decimal(18,2)")]
    public virtual decimal TaxInput { get; private set; }

    [Column(TypeName = "decimal(18,2)")]
    public virtual decimal TaxAmount { get; private set; }

    public virtual int DiscountType { get; private set; }

    [Column(TypeName = "decimal(18,2)")]
    public virtual decimal Discount { get; private set; }

    [Column(TypeName = "decimal(18,2)")]
    public virtual decimal Total { get; private set; }

    [Column(TypeName = "decimal(18,2)")]
    public virtual decimal Net { get; private set; }

    public virtual long CurrencyId { get; private set; }

    public virtual DateTime CapturedAt { get; private set; }

    public virtual ElectronicInvoiceChannel SubmissionChannel { get; private set; }

    public virtual ElectronicInvoiceSubmissionStatus SubmissionStatus { get; private set; }

    [StringLength(100)]
    public virtual string? ExternalReference { get; private set; }

    [StringLength(500)]
    public virtual string? SubmissionError { get; private set; }

    public virtual IReadOnlyCollection<InvoiceTaxSnapshotLine> Lines => _lines.AsReadOnly();

    protected InvoiceTaxSnapshot() { }

    public static InvoiceTaxSnapshot Capture(
        long invoiceId,
        string? invoiceNumber,
        long invoiceTypeId,
        int taxType,
        decimal taxInput,
        int discountType,
        decimal discount,
        decimal total,
        decimal net,
        long currencyId,
        DateTime capturedAt)
    {
        if (invoiceId <= 0)
            throw new Exceptions.TaxDomainException("An invoice is required.");
        if (currencyId <= 0)
            throw new Exceptions.TaxDomainException("A currency is required.");
        if (total < 0)
            throw new Exceptions.TaxDomainException("Invoice total cannot be negative.");

        return new InvoiceTaxSnapshot
        {
            InvoiceId = invoiceId,
            InvoiceNumber = invoiceNumber,
            InvoiceTypeId = invoiceTypeId,
            TaxType = taxType,
            TaxInput = taxInput,
            TaxAmount = CalculatePostedTaxAmount(taxType, taxInput, discountType, discount, total),
            DiscountType = discountType,
            Discount = discount,
            Total = total,
            Net = net,
            CurrencyId = currencyId,
            CapturedAt = capturedAt,
            SubmissionChannel = ElectronicInvoiceChannel.None,
            SubmissionStatus = ElectronicInvoiceSubmissionStatus.NotSubmitted
        };
    }

    public InvoiceTaxSnapshotLine AddLine(long productId, decimal quantity, decimal price, decimal tax, decimal lineNet, decimal lineTotal)
    {
        var line = new InvoiceTaxSnapshotLine(Id, productId, quantity, price, tax, lineNet, lineTotal);
        _lines.Add(line);
        return line;
    }

    public void RecordSubmission(
        ElectronicInvoiceChannel channel,
        ElectronicInvoiceSubmissionStatus status,
        string? externalReference,
        string? error)
    {
        SubmissionChannel = channel;
        SubmissionStatus = status;
        ExternalReference = externalReference;
        SubmissionError = error is { Length: > 500 } ? error[..500] : error;
    }

    /// <summary>
    /// Mirrors CommercialDocuments invoice posting: TaxType 1 = amount, 2 = percent of taxable total.
    /// This is a posted-invoice snapshot formula, not a TaxCode calculation engine.
    /// </summary>
    internal static decimal CalculatePostedTaxAmount(
        int taxType,
        decimal taxInput,
        int discountType,
        decimal discount,
        decimal total)
    {
        if (taxInput == 0)
            return 0;
        if (taxType == 1)
            return taxInput;
        if (taxType != 2)
            return 0;

        var discountAmount = discountType == 2
            ? total * discount / 100
            : discountType == 1 ? discount : 0;
        return (total - discountAmount) * taxInput / 100;
    }
}

namespace Tax.Domain;

[Table("InvoiceTaxSnapshotLine")]
public class InvoiceTaxSnapshotLine : BaseModel
{
    public virtual long SnapshotId { get; private set; }

    public virtual long ProductId { get; private set; }

    [Column(TypeName = "decimal(18,2)")]
    public virtual decimal Quantity { get; private set; }

    [Column(TypeName = "decimal(18,2)")]
    public virtual decimal Price { get; private set; }

    [Column(TypeName = "decimal(18,2)")]
    public virtual decimal Tax { get; private set; }

    [Column(TypeName = "decimal(18,2)")]
    public virtual decimal Net { get; private set; }

    [Column(TypeName = "decimal(18,2)")]
    public virtual decimal Total { get; private set; }

    public virtual InvoiceTaxSnapshot? Snapshot { get; private set; }

    protected InvoiceTaxSnapshotLine() { }

    internal InvoiceTaxSnapshotLine(
        long snapshotId,
        long productId,
        decimal quantity,
        decimal price,
        decimal tax,
        decimal net,
        decimal total)
    {
        SnapshotId = snapshotId;
        ProductId = productId;
        Quantity = quantity;
        Price = price;
        Tax = tax;
        Net = net;
        Total = total;
    }
}

namespace CommercialDocuments.Domain
{
    [Table("InvoiceProduct")]
    public class InvoiceProduct : BaseModel
    {
        [Required]
        public virtual long RowNumber { get; set; }

        [ForeignKey("Invoice")]
        public virtual long InvoiceId { get; set; }

        public virtual Invoice? Invoice { get; set; }

        // Product/Stock navigations dropped — Sales/Inventory module boundary (Fluent
        // "no navigation" config in OrgContext preserves both FK constraints exactly) — see
        // docs/modular-monolith-analysis.md §21.
        public virtual long ProductId { get; set; }

        /// <summary>Scalar-only Catalog Unit reference. FK preserved in OrgContext.</summary>
        public virtual long UnitId { get; set; }

        public virtual long? StockId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Price { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Total { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Discount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Tax { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Service { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Net { get; set; }

        public virtual string? Notes { get; set; }                       
    }
}
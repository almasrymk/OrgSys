namespace Sales.Domain
{
    [Table("Invoice")]
    public class Invoice : MovementModel
    {
        [ForeignKey("Dealer")]
        public virtual long DealerId { get; set; }

        public virtual Dealer? Dealer { get; set; } 

        [ForeignKey("PaymentType")]
        public virtual long PaymentTypeId { get; set; }

        public virtual PaymentType? PaymentType { get; set; }

        // Stock/Transaction navigations dropped — Sales/Inventory module boundary. StockId stays
        // a plain scalar FK (Fluent "no navigation" config in OrgContext preserves the DB
        // constraint); Transaction's Include was confirmed dead (loaded, never read) so
        // TransactionId keeps no relationship configuration at all — see
        // docs/modular-monolith-analysis.md §21.
        public virtual long? StockId { get; set; }

        public virtual long? TransactionId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Total { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Discount { get; set; }

        public virtual int DiscountType { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Tax { get; set; }

        public virtual int TaxType { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Service { get; set; }

        public virtual int ServiceType { get; set; }

        [ForeignKey("Currency")]
        public virtual long CurrencyId { get; set; }

        public virtual Currency? Currency { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Rate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Net { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal NetByDefaultCurrency { get; set; }

        [StringLength(500)]
        public virtual string? Notes { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Remaining { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Paid { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Credit { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal CreditByDefaultCurrency { get; set; }          

        public virtual ICollection<InvoiceProduct>? InvoiceProducts { get; set; }
    }
}
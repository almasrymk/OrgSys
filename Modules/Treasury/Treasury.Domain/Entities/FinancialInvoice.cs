namespace Treasury.Domain
{
    [Table("FinancialInvoice")]
    public class FinancialInvoice : BaseModel
    {
        [Required]
        public virtual long RowNumber { get; set; }

        [ForeignKey("Financial")]
        public virtual long FinancialId { get; set; }

        public virtual Financial? Financial { get; set; }

        /// <summary>Scalar-only reference into CommercialDocuments.Domain.Invoice — no EF
        /// navigation. FK preserved via Fluent HasOne(typeof(Invoice)) in OrgContext.</summary>
        public virtual long? InvoiceId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Amount { get; set; }        
    }
}
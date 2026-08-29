namespace Domain.Entities
{
    [Table("Financial")]
    public class Financial : MovementModel
    {
        [ForeignKey("Dealer")]
        public virtual long? DealerId { get; set; }

        public virtual Dealer? Dealer { get; set; }

        [ForeignKey("PaymentType")]
        public virtual long PaymentTypeId { get; set; }

        public virtual PaymentType? PaymentType { get; set; }

        [ForeignKey("Outlay")]
        public virtual long? OutlayId { get; set; }

        public virtual Outlay? Outlay { get; set; }

        [ForeignKey("CurrencyId")]
        public long CurrencyId { get; set; }

        public Currency? Currency { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Rate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal AmountByDefaultCurrency { get; set; }

        [ForeignKey(nameof(FinancialAccount))]
        public long? FinancialAccountId { get; set; }
        public virtual FinancialAccount? FinancialAccount { get; set; }

        // Numerically identical to FinancialTransactionType (Domain/Enums/FinancialTransactionType.cs) —
        // that enum and the FinancialType table share one Id space (see InitialData.cs), so callers can
        // freely cast between them. Kept as long? rather than the enum itself: EF Core cannot map an
        // enum-typed FK against a long-typed principal key, even with a value converter.
        [ForeignKey(nameof(FinancialType))]
        public long? FinancialTypeId { get; set; }
        public virtual FinancialType? FinancialType { get; set; }

        public FinancialTransactionDirection? Direction { get; set; }
        public FinancialReferenceType ReferenceType { get; set; } = FinancialReferenceType.Other;
        public long? ReferenceId { get; set; }

        [StringLength(100)]
        public string? ReferenceNumber { get; set; }

        [ForeignKey(nameof(ContraFinancialAccount))]
        public long? ContraFinancialAccountId { get; set; }
        public virtual FinancialAccount? ContraFinancialAccount { get; set; }

        [ForeignKey(nameof(FinancialTransfer))]
        public long? FinancialTransferId { get; set; }
        public virtual FinancialTransfer? FinancialTransfer { get; set; }

        [ForeignKey(nameof(Journal))]
        public long? JournalId { get; set; }
        public virtual Journal? Journal { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }

        public ICollection<FinancialInvoice>? FinancialInvoices { get; set; }
    }
}

namespace Domain.Entities
{
    [Table("TransactionProduct")]
    public class TransactionProduct : BaseModel
    {
        [Required]
        public virtual long RowNumber { get; set; }

        [ForeignKey("Transaction")]
        public virtual long TransactionId { get; set; }

        public virtual Transaction? Transaction { get; set; }

        [ForeignKey("Product")]
        public virtual long ProductId { get; set; }

        public virtual Product? Product { get; set; }

        [ForeignKey("Unit")]
        public virtual long UnitId { get; set; }

        public virtual Unit? Unit { get; set; }

        [ForeignKey("Stock")]
        public virtual long? StockId { get; set; }

        public virtual Stock? Stock { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Cost { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Total { get; set; }
        
        public virtual string? Notes { get; set; }                       
    }
}
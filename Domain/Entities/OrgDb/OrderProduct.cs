namespace Domain.Entities
{
    [Table("OrderProduct")]
    public class OrderProduct : BaseModel
    {
        [Required]
        public virtual long RowNumber { get; set; }

        [ForeignKey("Order")]
        public virtual long OrderId { get; set; }

        public virtual Order Order { get; set; }

        [ForeignKey("Product")]
        public virtual long ProductId { get; set; }

        public virtual Product Product { get; set; }

        [ForeignKey("Unit")]
        public virtual long UnitId { get; set; }

        public virtual Unit Unit { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Price { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Total { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Discount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Service { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Tax { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Net { get; set; }
       
        public virtual string Notes { get; set; }                       
    }
}
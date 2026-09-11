namespace Sales.Domain
{
    [Table("OrderProduct")]
    public class OrderProduct : BaseModel
    {
        [Required]
        public virtual long RowNumber { get; set; }

        [ForeignKey("Order")]
        public virtual long OrderId { get; set; }

        public virtual Order? Order { get; set; }

        // Product navigation dropped — Sales/Inventory module boundary; never actually
        // dereferenced anywhere (Order has no CQRS/API surface yet — see
        // docs/modular-monolith-analysis.md §3/§21). Fluent "no navigation" config in
        // OrgContext preserves the FK constraint.
        public virtual long ProductId { get; set; }

        [ForeignKey("Unit")]
        public virtual long UnitId { get; set; }

        public virtual Unit? Unit { get; set; }

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
       
        public virtual string? Notes { get; set; }                       
    }
}
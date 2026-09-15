namespace Catalog.Domain
{
    [Table("PriceListEntry")]
    public class PriceListEntry : BaseModel
    {
        [ForeignKey("PriceList")]
        public virtual long PriceListId { get; set; }

        public virtual PriceList? PriceList { get; set; }

        [ForeignKey("Product")]
        public virtual long ProductId { get; set; }

        public virtual Product? Product { get; set; }

        [ForeignKey("Unit")]
        public virtual long? UnitId { get; set; }

        public virtual Unit? Unit { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Price { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal? MinQuantity { get; set; }

        public virtual DateTime? ValidFrom { get; set; }

        public virtual DateTime? ValidTo { get; set; }

        public virtual bool IsActive { get; set; } = true;
    }
}

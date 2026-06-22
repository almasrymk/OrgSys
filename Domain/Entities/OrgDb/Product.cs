namespace Domain.Entities
{
    [Table("Product")]
    public class Product : BaseModel
    {       
        [Required]
        public virtual string Name { get; set; }

        public virtual string Nickname { get; set; }
       
        public virtual string Barcode { get; set; }

        public virtual string Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Price { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Cost { get; set; }

        [ForeignKey("Classification")]
        public virtual long ClassificationId { get; set; }

        public virtual Classification Classification { get; set; }

        [ForeignKey("Dealer")]
        public virtual long? DealerId { get; set; }

        public virtual Dealer Dealer { get; set; }

        public virtual string Recipe { get; set; }

        public virtual ICollection<ProductUnit> ProductUnits { get; set; }

        public virtual ICollection<ProductRecipe> ProductRecipes { get; set; }

        public virtual ICollection<ProductPropertyElement> ProductPropertyElements { get; set; }
    }
}
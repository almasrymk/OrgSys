namespace Domain.Entities
{
    [Table("GeneralProductPropertyElement", Schema = "admin")]
   public class GeneralProductPropertyElement : BaseModel
    {
        [ForeignKey("GeneralProduct")]
        public virtual long? GeneralProductId { get; set; }

        public virtual GeneralProduct? GeneralProduct { get; set; }

        [ForeignKey("GeneralProperty")]
        public virtual long? GeneralPropertyId { get; set; }

        public virtual GeneralProperty? GeneralProperty { get; set; }

        [ForeignKey("GeneralPropertyElement")]
        public virtual long? GeneralPropertyElementId { get; set; }

        public virtual GeneralPropertyElement? GeneralPropertyElement { get; set; }

        public virtual bool IsChecked { get; set; }
    }
}
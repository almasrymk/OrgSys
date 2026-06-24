namespace Domain.Entities
{
    [Table("Property")]
   public class Property : BaseModel
    {       
        [StringLength(50, MinimumLength = 3)]
        public virtual string Name { get; set; }

        public virtual ICollection<PropertyElement> PropertyElements { get; set; }
    }
}
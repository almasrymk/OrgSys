namespace Domain.Entities
{
    [Table("Classification")]
    public class Classification : BaseModel
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public virtual string Name { get; set; }

        public virtual bool BePurchased { get; set; }

        public virtual bool BeSold { get; set; }

        public virtual bool BeManufactured { get; set; }
    }
}
namespace Domain.Entities
{
    [Table("CompanyProfile")]
    public class CompanyProfile : BaseLockupEntity
    {
        [Required]
        public override string? Name { get; set; }

        public string? Description { get; set; }

        [StringLength(30, MinimumLength = 3)]
        public virtual string CommercialRegister { get; set; }

        [StringLength(30, MinimumLength = 3)]
        public virtual string TaxCard { get; set; }

        public virtual string Website { get; set; }

        public virtual string Watsapp { get; set; }        

        public virtual DateTime DateCreated { get; set; }

        public virtual long TypeActivity { get; set; }

        public virtual long NationalityId { get; set; }

        public virtual long SizeOfCompany { get; set; }

        public virtual long ClientId { get; set; }
    }
}
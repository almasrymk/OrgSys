using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("CompanyProfile")]
    public class CompanyProfile : BaseModel
    {
        [Required]
        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        [StringLength(25, MinimumLength = 8)]        
        public virtual string Phone1 { get; set; }

        [StringLength(25, MinimumLength = 8)]
        public virtual string Phone2 { get; set; }

        [StringLength(25, MinimumLength = 8)]
        public virtual string Mobile1 { get; set; }

        [StringLength(25, MinimumLength = 8)]
        public virtual string Mobile2 { get; set; }

        [StringLength(25, MinimumLength = 8)]
        public virtual string Fax1 { get; set; }

        [StringLength(25, MinimumLength = 8)]
        public virtual string Fax2 { get; set; }

        [StringLength(30, MinimumLength = 3)]        
        public virtual string Email1 { get; set; }

        [StringLength(30, MinimumLength = 3)]
        public virtual string Email2 { get; set; }

        [StringLength(500, MinimumLength = 3)]        
        public virtual string Address1 { get; set; }

        [StringLength(500, MinimumLength = 3)]
        public virtual string Address2 { get; set; }

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
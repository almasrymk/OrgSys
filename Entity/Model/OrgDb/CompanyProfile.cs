using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("CompanyProfile")]
    public class CompanyProfile : BaseModel
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [StringLength(25, MinimumLength = 8)]        
        public string Phone1 { get; set; }

        [StringLength(25, MinimumLength = 8)]
        public string Phone2 { get; set; }

        [StringLength(25, MinimumLength = 8)]
        public string Mobile1 { get; set; }

        [StringLength(25, MinimumLength = 8)]
        public string Mobile2 { get; set; }

        [StringLength(25, MinimumLength = 8)]
        public string Fax1 { get; set; }

        [StringLength(25, MinimumLength = 8)]
        public string Fax2 { get; set; }

        [StringLength(30, MinimumLength = 3)]        
        public string Email1 { get; set; }

        [StringLength(30, MinimumLength = 3)]
        public string Email2 { get; set; }

        [StringLength(500, MinimumLength = 3)]        
        public string Address1 { get; set; }

        [StringLength(500, MinimumLength = 3)]
        public string Address2 { get; set; }

        [StringLength(30, MinimumLength = 3)]
        public string CommercialRegister { get; set; }

        [StringLength(30, MinimumLength = 3)]
        public string TaxCard { get; set; }

        public string Website { get; set; }

        public string Watsapp { get; set; }        

        public DateTime DateCreated { get; set; }

        public long TypeActivity { get; set; }

        public long NationalityId { get; set; }

        public long SizeOfCompany { get; set; }

        public long ClientId { get; set; }
    }
}
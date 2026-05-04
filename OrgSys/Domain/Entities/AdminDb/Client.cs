using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{    
    [Table("Client" , Schema = "admin")]
    public class Client : BaseEntity
    {
        [Required]
        public virtual string Name { get; set; }

        public virtual string CompanyName { get; set; }

        public virtual string Description { get; set; }

        [StringLength(25, MinimumLength = 8)]        
        public virtual string Phone { get; set; }
      
        [StringLength(25, MinimumLength = 8)]
        public virtual string Mobile { get; set; }       

        [StringLength(25, MinimumLength = 8)]
        public virtual string Fax { get; set; }
      
        [StringLength(30, MinimumLength = 3)]        
        public virtual string Email { get; set; }

        public virtual string DbSchema { get; set; }

        public virtual long TypeActivityId { get; set; }

        public virtual long NationalityId { get; set; }

        public virtual long SizeOfCompany { get; set; }

        public virtual long RequestId { get; set; }

        public virtual long VersionDb { get; set; }

        public virtual TypeActivity TypeActivity { get; set; }

        public virtual Nationality Nationality { get; set; }
    }
}
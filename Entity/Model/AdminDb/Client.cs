using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{    
    [Table("Client" , Schema = "admin")]
    public class Client : BaseModel
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [StringLength(25, MinimumLength = 8)]        
        public string Phone { get; set; }
      
        [StringLength(25, MinimumLength = 8)]
        public string Mobile { get; set; }       

        [StringLength(25, MinimumLength = 8)]
        public string Fax { get; set; }
      
        [StringLength(30, MinimumLength = 3)]        
        public string Email { get; set; }

        public string DbSchema { get; set; }

        public long TypeActivityId { get; set; }

        public long NationalityId { get; set; }

        public long SizeOfCompany { get; set; }

        public virtual TypeActivity TypeActivity { get; set; }
        public virtual Nationality Nationality { get; set; }
    }
}
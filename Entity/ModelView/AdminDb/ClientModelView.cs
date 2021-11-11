using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{    
    public class ClientModelView : BaseModel
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

        public string TypeActivityName { get; set; }

        public long NationalityId { get; set; }

        public string NationalityName { get; set; }

        public long SizeOfCompany { get; set; }

        public long RequestId { get; set; }    
    }
}
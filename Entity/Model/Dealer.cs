using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Utility;

namespace Entity.Model
{
    [Table("Dealer")]
    public class Dealer : BaseModel
    {
        public long CodeNumber { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 3)]        
        public string Code { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        [StringLength(25, MinimumLength = 8)]        
        public string Phone { get; set; }

        [StringLength(20, MinimumLength = 3)]        
        public string Email { get; set; }

        [StringLength(500, MinimumLength = 3)]        
        public string Address { get; set; }       
    }
}

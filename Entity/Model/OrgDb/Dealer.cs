using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("Dealer", Schema = "org")]
    public class Dealer : BaseModel
    {
        [Required]
        public string Name { get; set; }

        [StringLength(25, MinimumLength = 8)]        
        public string Phone { get; set; }

        [StringLength(30, MinimumLength = 3)]        
        public string Email { get; set; }

        [StringLength(500, MinimumLength = 3)]        
        public string Address { get; set; }       
    }
}
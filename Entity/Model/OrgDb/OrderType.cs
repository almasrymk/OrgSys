using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("OrderType")]
    public class OrderType : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }    
        
        public string Icon { get; set; }
    }
}
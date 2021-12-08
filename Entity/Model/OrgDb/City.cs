using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("City")]
    public class City : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        [ForeignKey("Country")]
        public long? CountryId { get; set; }
       
        public virtual Country Country { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("City")]
    public class City : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public virtual string Name { get; set; }

        [ForeignKey("Country")]
        public virtual long? CountryId { get; set; }
       
        public virtual Country Country { get; set; }
    }
}
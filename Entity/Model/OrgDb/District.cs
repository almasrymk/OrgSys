using Utility;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("District")]
    public class District : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        [ForeignKey("Country")]
        public long? CountryId { get; set; }

        public virtual Country Country { get; set; }

        [ForeignKey("City")]
        public long? CityId { get; set; }

        public virtual City City { get; set; }
    }
}
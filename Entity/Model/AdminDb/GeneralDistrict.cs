using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("GeneralDistrict", Schema = "admin")]
    public class GeneralDistrict : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        [ForeignKey("GeneralCountry")]
        public long GeneralCountryId { get; set; }

        public virtual GeneralCountry GeneralCountry { get; set; }

        [ForeignKey("GeneralCity")]
        public long GeneralCityId { get; set; }

        public virtual GeneralCity GeneralCity { get; set; }
    }
}
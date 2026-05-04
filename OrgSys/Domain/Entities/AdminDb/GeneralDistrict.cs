using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("GeneralDistrict", Schema = "admin")]
    public class GeneralDistrict : BaseEntity
    {
        [StringLength(50, MinimumLength = 3)]
        public virtual string Name { get; set; }

        [ForeignKey("GeneralCountry")]
        public virtual long GeneralCountryId { get; set; }

        public virtual GeneralCountry GeneralCountry { get; set; }

        [ForeignKey("GeneralCity")]
        public virtual long GeneralCityId { get; set; }

        public virtual GeneralCity GeneralCity { get; set; }
    }
}
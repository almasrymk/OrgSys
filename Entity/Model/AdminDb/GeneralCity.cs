using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("GeneralCity", Schema = "admin")]
    public class GeneralCity : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        [ForeignKey("GeneralCountry")]
        public long GeneralCountryId { get; set; }
       
        public virtual GeneralCountry GeneralCountry { get; set; }
    }
}
using Utility;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("BankBranch")]
    public class BankBranch : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        [ForeignKey("Bank")]
        public long BankId { get; set; }

        public virtual Bank Bank { get; set; }

        [ForeignKey("Country")]
        public long CountryId { get; set; }

        public virtual Country Country { get; set; }

        [ForeignKey("City")]
        public long CityId { get; set; }

        public virtual City City { get; set; }

        [ForeignKey("District")]
        public long DistrictId { get; set; }

        public virtual District District { get; set; }
    }
}
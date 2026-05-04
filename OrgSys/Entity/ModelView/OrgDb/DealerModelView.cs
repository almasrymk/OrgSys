using Entity.Model;
using System.ComponentModel.DataAnnotations;

namespace Entity.ModelView
{
    public class DealerModelView : BaseModel
    {
        public virtual string Name { get; set; }

        [StringLength(25, MinimumLength = 8)]
        public virtual string Phone { get; set; }

        [StringLength(30, MinimumLength = 3)]
        public virtual string Email { get; set; }

        [StringLength(500, MinimumLength = 3)]
        public virtual string Address { get; set; }

        public virtual long? DealerGroupId { get; set; }

        public virtual string DealerGroupName { get; set; }

        public virtual long? CountryId { get; set; }

        public virtual string CountryName { get; set; }

        public virtual long? CityId { get; set; }

        public virtual string CityName { get; set; }

        public virtual long? DistrictId { get; set; }

        public virtual string DistrictName { get; set; }
    }
}
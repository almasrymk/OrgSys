using Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.DTOs
{
    public class DistrictModelView : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public virtual string Name { get; set; }

        [ForeignKey("Country")]
        public virtual long? CountryId { get; set; }

        [ForeignKey("City")]
        public virtual long? CityId { get; set; }

        public string CountryName { get; set; }
        public string CityName { get; set; }

    }
}
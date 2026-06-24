using Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.DTOs
{
    public class CityDto : BaseModel
    {
        //[StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        public long? CountryId { get; set; }

        public string CountryName { get; set; }
    }
}
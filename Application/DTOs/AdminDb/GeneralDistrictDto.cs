using Domain.Entities;

namespace Application.DTOs
{
    public class GeneralDistrictDto : GeneralDistrict
    { 
        public string? GeneralCountryName { get; set; }

        public string? GeneralCityName { get; set; }
    }
}
using Domain.Entities;

namespace Application.DTOs
{
    public class GeneralDistrictModelView : GeneralDistrict
    { 
        public string GeneralCountryName { get; set; }

        public string GeneralCityName { get; set; }
    }
}
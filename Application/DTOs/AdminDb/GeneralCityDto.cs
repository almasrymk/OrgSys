using Domain.Entities;

namespace Application.DTOs
{
    public class GeneralCityDto : GeneralCity
    {       
        public string GeneralCountryName { get; set; }
    }
}
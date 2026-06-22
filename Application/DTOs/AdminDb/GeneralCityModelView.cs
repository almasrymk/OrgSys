using Domain.Entities;

namespace Application.DTOs
{
    public class GeneralCityModelView : GeneralCity
    {       
        public string GeneralCountryName { get; set; }
    }
}
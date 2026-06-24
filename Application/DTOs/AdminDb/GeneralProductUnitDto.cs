using Domain.Entities;

namespace Application.DTOs
{
    public class GeneralProductUnitDto : GeneralProductUnit
    {      
        public string GeneralProductName { get; set; }

        public string GeneralUnitName { get; set; }
    }
}
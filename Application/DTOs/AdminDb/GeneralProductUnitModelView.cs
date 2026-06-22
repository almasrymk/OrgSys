using Domain.Entities;

namespace Application.DTOs
{
    public class GeneralProductUnitModelView : GeneralProductUnit
    {      
        public string GeneralProductName { get; set; }

        public string GeneralUnitName { get; set; }
    }
}
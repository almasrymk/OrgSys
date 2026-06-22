using Domain.Entities;

namespace Application.DTOs
{
    public class InventoryProductModelView : InventoryProduct
    {       
        public string ProductName { get; set; }

        public string UnitName { get; set; }

        public List<UnitModelView> UnitList { get; set; }
    }
}
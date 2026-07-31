using Domain.Entities;

namespace Application.DTOs
{
    public class InventoryProductDto : InventoryProduct
    {       
        public string? ProductName { get; set; }

        public string? UnitName { get; set; }

        public List<UnitDto>? UnitList { get; set; }
    }
}
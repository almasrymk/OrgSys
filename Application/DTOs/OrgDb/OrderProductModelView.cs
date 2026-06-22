using Domain.Entities;

namespace Application.DTOs
{
    public class OrderProductModelView : OrderProduct
    {
        public string ProductName { get; set; }

        public string UnitName { get; set; }

        public List<UnitModelView> UnitList { get; set; }
    }
}
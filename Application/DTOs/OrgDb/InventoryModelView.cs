using Domain.Entities;

namespace Application.DTOs
{
    public class InventoryModelView : Inventory
    {
        public string StockName { get; set; }

        public string UserName { get; set; }

        public string BranchName { get; set; }

        public string CreateUserName { get; set; }

        public string ModifyUserName { get; set; }

        public string ShiftName { get; set; }

        public List<InventoryProductModelView> InventoryProductList { get; set; }
    }
}
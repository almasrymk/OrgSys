using Utility;
using System.Linq;
using Entity.Model;
using System.Collections.Generic;

namespace Entity.ModelView
{
    public class InventoryModelView : Inventory
    {
        public string StoreName { get; set; }

        public string UserName { get; set; }

        public string BranchName { get; set; }

        public string CreateUserName { get; set; }

        public string ModifyUserName { get; set; }

        public string ShiftName { get; set; }

        public List<InventoryProductModelView> InventoryProductList { get; set; }
    }
}
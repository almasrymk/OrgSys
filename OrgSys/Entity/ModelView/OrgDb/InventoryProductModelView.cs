using Entity.Model;
using System.Collections.Generic;


namespace Entity.ModelView
{
    public class InventoryProductModelView : InventoryProduct
    {       
        public string ProductName { get; set; }

        public string UnitName { get; set; }

        public List<UnitModelView> UnitList { get; set; }
    }
}
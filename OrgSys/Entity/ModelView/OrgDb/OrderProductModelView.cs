using Entity.Model;
using System.Collections.Generic;

namespace Entity.ModelView
{
    public class OrderProductModelView : OrderProduct
    {
        public string ProductName { get; set; }

        public string UnitName { get; set; }

        public List<UnitModelView> UnitList { get; set; }
    }
}
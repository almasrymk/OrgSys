using Entity.Model;
using System.Collections.Generic;

namespace Entity.ModelView
{
    public class TransactionProductModelView : TransactionProduct
    {       
        public string ProductName { get; set; }

        public string UnitName { get; set; }

        public string StoreName { get; set; }

        public List<UnitModelView> UnitList { get; set; }
    }
}
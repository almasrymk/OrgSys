using Entity.Model;
using System.Collections.Generic;
using System.Linq;

namespace Entity.ModelView
{
    public class InvoiceProductModelView : InvoiceProduct
    { 
        public string ProductName { get; set; }

        public string UnitName { get; set; }

        public string StockName { get; set; }
    }
}
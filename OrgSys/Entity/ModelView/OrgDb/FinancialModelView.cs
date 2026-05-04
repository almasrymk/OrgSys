using Entity.Model;
using System.Collections.Generic;

namespace Entity.ModelView
{
    public class FinancialModelView : Financial
    {      

        public string DealerName { get; set; }

        public string PaymentTypeName { get; set; }

        public string OutlayName { get; set; }

        public string SafeName { get; set; }

        public string CurrencyName { get; set; }

        public List<FinancialInvoiceModelView> FinancialInvoiceList { get; set; }
    }
}
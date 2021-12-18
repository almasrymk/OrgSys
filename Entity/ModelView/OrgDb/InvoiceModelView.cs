using Entity.Model;
using System.Collections.Generic;

namespace Entity.ModelView
{
    public class InvoiceModelView : Invoice
    {     
        public string DealerName { get; set; }

        public string PaymentTypeName { get; set; }

        public string StoreName { get; set; }

        public string ParentCode { get; set; }

        public string CurrencyName { get; set; }

        public string BranchName { get; set; }

        public string CreateUserName { get; set; }

        public string ModifyUserName { get; set; }

        public string ShiftName { get; set; }

        public bool Cash { get; set; }      

        public List<InvoiceProductModelView> InvoiceProductList { get; set; }
    }
}
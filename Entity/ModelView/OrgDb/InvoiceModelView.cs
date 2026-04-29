using Entity.Model;
using System.Collections.Generic;

namespace Entity.ModelView
{
    public class InvoiceModelView : MovementModel
    {     
        public virtual long DealerId { get; set; }

        public virtual long PaymentTypeId { get; set; }

        public virtual long? StockId { get; set; }

        public virtual long? TransactionId { get; set; }
        
        public string TransactionCode { get; set; }

        public virtual decimal Total { get; set; }

        public virtual decimal Discount { get; set; }

        public virtual int DiscountType { get; set; }

        public virtual decimal Tax { get; set; }

        public virtual int TaxType { get; set; }

        public virtual decimal Service { get; set; }

        public virtual int ServiceType { get; set; }

        public virtual long CurrencyId { get; set; }

        public virtual decimal Rate { get; set; }

        public virtual decimal Net { get; set; }

        public virtual decimal NetByDefaultCurrency { get; set; }

        public virtual string Notes { get; set; }

        public virtual decimal Remaining { get; set; }

        public virtual decimal Paid { get; set; }

        public virtual decimal Credit { get; set; }

        public virtual decimal CreditByDefaultCurrency { get; set; }          

        public string DealerName { get; set; }

        public string PaymentTypeName { get; set; }

        public string StockName { get; set; }

        public string ParentCode { get; set; }

        public string CurrencyName { get; set; }

        public string BranchName { get; set; }

        public string CreateUserName { get; set; }

        public string ModifyUserName { get; set; }

        public string ShiftName { get; set; }

        public bool Cash { get; set; }      

        public List<InvoiceProductModelView> InvoiceProducts { get; set; }
    }
}
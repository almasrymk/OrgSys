using Entity.Model;

namespace Entity.ModelView
{
    public class FinancialInvoiceModelView : FinancialInvoice
    {       
        public decimal? Net { get; set; }
    }
}
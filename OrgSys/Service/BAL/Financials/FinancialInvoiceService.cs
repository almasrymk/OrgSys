using Entity.Model;
using Entity.ModelView;

namespace Service
{
    public class FinancialInvoiceService : BaseOrgService<FinancialInvoiceModelView, FinancialInvoice>
    {
        public FinancialInvoiceService(string Schema) : base(Schema, "Financial,Invoice") { }
    }
}
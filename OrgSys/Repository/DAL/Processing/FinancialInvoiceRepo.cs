using Entity.Model;

namespace Repository
{
    public class FinancialInvoiceRepo : CurdOrg<FinancialInvoice>
    {
        public FinancialInvoiceRepo(string Schema) : base(Schema) { }
    }
}
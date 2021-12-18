using Entity.Model;
using Entity.ModelView;

namespace Service
{
    public class InvoiceService : BaseOrgService<InvoiceModelView, Invoice>
    {
        public InvoiceService(string Schema) : base(Schema, "Dealer,Transaction,InvoiceProducts,InvoiceProducts.Product,InvoiceProducts.Product.ProductUnits,,InvoiceProducts.Product.ProductUnits.Unit") { }
    }
}
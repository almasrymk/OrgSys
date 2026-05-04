using Entity.Model;
using Entity.ModelView;

namespace Service
{
    public class InvoiceProductService : BaseOrgService<InvoiceProductModelView, InvoiceProduct>
    {
        public InvoiceProductService(string Schema) : base(Schema, "Invoice,Product,Unit,Stock") { }
    }
}
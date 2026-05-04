using Entity.Model;
using Entity.ModelView;

namespace Service
{
    public class InvoiceTypeService : BaseOrgService<InvoiceTypeModelView, InvoiceType>
    {
        public InvoiceTypeService(string Schema) : base(Schema) { }
    }
}
using Entity.Model;

namespace Repository
{
    public class InvoiceProductRepo : CurdOrg<InvoiceProduct>
    {
        public InvoiceProductRepo(string Schema) : base(Schema) { }
    }
}
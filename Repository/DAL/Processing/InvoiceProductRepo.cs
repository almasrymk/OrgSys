using Domain.Entities;

namespace Repository
{
    public class InvoiceProductRepo : CurdOrg<InvoiceProduct>
    {
        public InvoiceProductRepo(string Schema) : base(Schema) { }
    }
}
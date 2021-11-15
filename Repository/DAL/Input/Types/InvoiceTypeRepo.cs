using Entity.Model;

namespace Repository
{
    public class InvoiceTypeRepo : CurdOrg<InvoiceType>
    {
        public InvoiceTypeRepo(string Schema) : base(Schema) { }
    }
}
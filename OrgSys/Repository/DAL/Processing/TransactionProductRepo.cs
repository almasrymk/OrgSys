using Entity.Model;

namespace Repository
{
    public class TransactionProductRepo : CurdOrg<TransactionProduct>
    {
        public TransactionProductRepo(string Schema) : base(Schema) { }
    }
}
using Entity.Model;

namespace Repository
{
    public class TransactionRepo : CurdOrg<Transaction>
    {
        public TransactionRepo(string Schema) : base(Schema) { }
    }
}
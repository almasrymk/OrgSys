using Domain.Entities;

namespace Repository
{
    public class TransactionTypeRepo : CurdOrg<TransactionType>
    {
        public TransactionTypeRepo(string Schema) : base(Schema) { }
    }
}
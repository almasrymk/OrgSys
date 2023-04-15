using Entity.Model;
using Entity.ModelView;

namespace Service
{
    public class TransactionProductService : BaseOrgService<TransactionProductModelView, TransactionProduct>
    {
        public TransactionProductService(string Schema) : base(Schema, "Transaction,Product,Unit,Stock") { }
    }
}
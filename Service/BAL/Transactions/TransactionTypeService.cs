using Entity.Model;
using Entity.ModelView;

namespace Service
{
    public class TransactionTypeService : BaseOrgService<TransactionTypeModelView, TransactionType>
    {
        public TransactionTypeService(string Schema) : base(Schema) { }
    }
}
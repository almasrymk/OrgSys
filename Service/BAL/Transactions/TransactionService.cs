using Entity.Model;
using Entity.ModelView;

namespace Service
{
    public class TransactionService : BaseOrgService<TransactionModelView, Transaction>
    {
        public TransactionService(string Schema) : base(Schema, "Dealer,Store,ToStore,TransactionProducts,TransactionProducts.Product,TransactionProducts.Product.ProductUnits,TransactionProducts.Product.ProductUnits.Unit") { }
    }
}
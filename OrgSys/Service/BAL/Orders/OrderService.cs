using Entity.Model;
using Entity.ModelView;

namespace Service
{
    public class OrderService : BaseOrgService<OrderModelView, Order>
    {
        public OrderService(string Schema) : base(Schema, "Dealer,Table,Invoice,OrderProducts,OrderProducts.Product,OrderProducts.Product.ProductUnits,,OrderProducts.Product.ProductUnits.Unit") { }

        public void Cancel(long Id)
        {
            var ob = repo.Get(e => e.Id == Id);
            ob.Status = Utility.Status.Cancel;
            ob.CloseTable = true;
            ob = repo.AddOrUpdate(ob);
        }

        public void Redo(long Id)
        {
            var ob = repo.Get(e => e.Id == Id);
            ob.Status = Utility.Status.All;
            ob.CloseTable = false;
            ob = repo.AddOrUpdate(ob);
        }
    }
}
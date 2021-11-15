using Entity.Model;

namespace Repository
{
    public class OrderRepo : CurdOrg<Order>
    {
        public OrderRepo(string Schema) : base(Schema) { }
    }
}
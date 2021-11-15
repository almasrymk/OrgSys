using Entity.Model;

namespace Repository
{
    public class OrderProductRepo : CurdOrg<OrderProduct>
    {
        public OrderProductRepo(string Schema) : base(Schema) { }
    }
}
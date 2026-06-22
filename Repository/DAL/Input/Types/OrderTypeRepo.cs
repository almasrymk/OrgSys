using Domain.Entities;

namespace Repository
{
    public class OrderTypeRepo : CurdOrg<OrderType>
    {
        public OrderTypeRepo(string Schema) : base(Schema) { }
    }
}
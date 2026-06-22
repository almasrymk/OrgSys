using Domain.Entities;

namespace Application.DTOs
{
    public class OrderModelView : Order
    {        
        public string TableName { get; set; }

        public string DealerName { get; set; }

        public string BranchName { get; set; }

        public string CreateUserName { get; set; }

        public string ModifyUserName { get; set; }

        public string ShiftName { get; set; }

        public List<OrderProductModelView> OrderProductList { get; set; }
    }
}
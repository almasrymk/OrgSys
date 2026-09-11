
namespace Sales.Application
{
    public class OrderDto : Order
    {        
        public string? TableName { get; set; }

        public string? DealerName { get; set; }

        public string? BranchName { get; set; }

        public string? CreateUserName { get; set; }

        public string? ModifyUserName { get; set; }

        public string? ShiftName { get; set; }

        public List<OrderProductDto>? OrderProductList { get; set; }
    }
}
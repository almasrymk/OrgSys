using Domain.Entities;

namespace Application.DTOs
{
    public class TransactionModelView : Transaction
    {
        public string DealerName { get; set; }

        public string StockName { get; set; }

        public string ToStockName { get; set; }

        public string ParentCode { get; set; }

        public string BranchName { get; set; }

        public string CreateUserName { get; set; }

        public string ModifyUserName { get; set; }

        public string ShiftName { get; set; }

        public List<TransactionProductModelView> TransactionProductList { get; set; }
    }
}
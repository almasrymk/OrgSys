using Domain.Entities;

namespace Application.DTOs
{
    public class InventoryDto : Inventory
    {
        public string? StockName { get; set; }

        public string? UserName { get; set; }

        public string? BranchName { get; set; }

        public string? CreateUserName { get; set; }

        public string? ModifyUserName { get; set; }

        public string? ShiftName { get; set; }

        public long? AdjustmentInTransactionId { get; set; }

        public string? AdjustmentInTransactionCode { get; set; }

        public long? AdjustmentOutTransactionId { get; set; }

        public string? AdjustmentOutTransactionCode { get; set; }

        public List<InventoryProductDto>? InventoryProductList { get; set; }
    }
}

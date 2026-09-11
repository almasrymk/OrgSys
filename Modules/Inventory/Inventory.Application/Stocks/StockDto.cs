using System.ComponentModel.DataAnnotations;

namespace Inventory.Application
{
    public class StockDto : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public  string? Name { get; set; }

        public  long BranchId { get; set; }

        public string? BranchName { get; set; }

        public long? AccountId { get; set; }

        public string? AccountName { get; set; }
    }
}

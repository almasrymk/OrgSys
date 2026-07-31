using Domain.Entities;

namespace Application.DTOs
{
    public class TransactionProductDto : TransactionProduct
    {       
        public string? ProductName { get; set; }

        public string? UnitName { get; set; }

        public string? StockName { get; set; }

        public List<UnitDto>? UnitList { get; set; }
    }
}
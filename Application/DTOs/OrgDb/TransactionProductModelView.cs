using Domain.Entities;

namespace Application.DTOs
{
    public class TransactionProductModelView : TransactionProduct
    {       
        public string ProductName { get; set; }

        public string UnitName { get; set; }

        public string StockName { get; set; }

        public List<UnitModelView> UnitList { get; set; }
    }
}
using Domain.Entities;

namespace Application.DTOs
{
    public class InvoiceProductModelView : InvoiceProduct
    { 
        public string ProductName { get; set; }

        public string UnitName { get; set; }

        public string StockName { get; set; }
    }
}
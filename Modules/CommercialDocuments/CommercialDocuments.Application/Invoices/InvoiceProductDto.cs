
namespace CommercialDocuments.Application
{
    public class InvoiceProductDto : InvoiceProduct
    { 
        public string? ProductName { get; set; }

        public string? UnitName { get; set; }

        public string? StockName { get; set; }
    }
}
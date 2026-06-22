using Domain.Entities;

namespace Application.DTOs
{
    public class FinancialInvoiceModelView : FinancialInvoice
    {       
        public decimal? Net { get; set; }
    }
}
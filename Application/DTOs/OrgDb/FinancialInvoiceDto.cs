using Domain.Entities;

namespace Application.DTOs
{
    public class FinancialInvoiceDto : FinancialInvoice
    {       
        public decimal? Net { get; set; }
    }
}
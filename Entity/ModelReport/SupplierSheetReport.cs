using System;

namespace Entity.ModelReport
{
    public class SupplierSheetReport
    {
        public int Id { get; set; }
        public int DealarId { get; set; }
        public string DealarName { get; set; }
        public string DealarCode { get; set; }
        public DateTime? GetDateTime { get; set; }
        public int? InvoiceCode { get; set; }
        public decimal? Debit { get; set; }
        public decimal? Credit { get; set; }
        public decimal? OpenBalnce { get; set; }
        public decimal? Balnce { get; set; }
        public string TypeInvoice { get; set; }
        public decimal BeginBalance { get; set; }
    }
}
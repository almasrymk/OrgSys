using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Report
{
    public class StockStatment
    {
        public virtual long Id { get; set; }

        public long? ReferenceId { get; set; }

        public string? ProductCode { get; set; }

        public string? TransactionCode { get; set; }

        public long? TypeId { get; set; }
        
        public string? TypeName { get; set; }

        public DateTime Date { get; set; }

        public long ProductId { get; set; }

        public string? ProductName { get; set; }

        public long StockId { get; set; }

        public string? StockName { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }

        public string? StockImgPath { get; set; }

        public string? ProductImgPath { get; set; }
    }

    public class StockStatmentData
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public StockBalance? StockBalance { get; set; }
    }
}
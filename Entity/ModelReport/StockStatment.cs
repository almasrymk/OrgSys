using System;

namespace Entity.ModelReport
{
    public class StockStatment
    {
        public virtual long Id { get; set; }

        public long? ReferenceId { get; set; }

        public string ProductCode { get; set; }
        public string TransactionCode { get; set; }

        public long? TypeId { get; set; }
        //public int OpenningBalance { get; set; }
        //public int Type { get; set; }
        public string TypeName { get; set; }
        public DateTime Date { get; set; }
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public long StockId { get; set; }
        public string StockName { get; set; }
        public decimal Quantity { get; set; }
        public string StockImgPath { get; set; }

        //public int InOut { get; set; }
        //public decimal Balance { get; set; }
    }

    public class StockStatmentData
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public StockBalance StockBalance { get; set; }
    }
}
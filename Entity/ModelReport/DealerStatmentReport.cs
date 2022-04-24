using System;

namespace Entity.ModelReport
{
    public class DealerStatmentReport 
    {
        public virtual long Id { get; set; }

        public virtual string Code { get; set; }

        public long TypeId { get; set; }

        public int Type { get; set; }
        public string TypeName { get; set; }
        public DateTime Date { get; set; }
        public long DealerId { get; set; }
        public string DealerName { get; set; }
        public decimal Amount { get; set; }
        public int InOut { get; set; }
        public decimal Balance { get; set; }
    }
}
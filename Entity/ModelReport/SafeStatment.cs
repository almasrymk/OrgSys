using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.ModelReport
{
    public class SafeStatment
    {
        public virtual long Id { get; set; }
        public long? ReferenceId { get; set; }
        public string Code { get; set; }
        public decimal Amount { get; set; }

        public long? TypeId { get; set; }
        public string TypeName { get; set; }
        public DateTime Date { get; set; }
        public long SafeId { get; set; }
        public string SafeName { get; set; }
        public long DealerId { get; set; }
        public string DealerName { get; set; }
        public long CurrencyId { get; set; }
        public string CurrencyName { get; set; }
    }
    public class SafeStatmentData
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public SafeBalance SafeBalance { get; set; }
    }
}

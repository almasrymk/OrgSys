using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Report
{
    public class SafeBalance
    {
        public long Id { get; set; }
        public long SafeId { get; set; }
        public string? SafeName { get; set; }
        public long DealerId { get; set; }
        public string? DealerName { get; set; }
        public long CurrencyId { get; set; }
        public string? CurrencyName { get; set; }
        public string? SafeImgPath { get; set; }

    }
}
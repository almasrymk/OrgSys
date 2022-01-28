using Entity.Model;
using System;

namespace Entity.ModelReport
{
    public class InvoiceSearchModelView
    {
        public long TypeId { get; set; }
        public long DealerId { get; set; }
        public int Period { get; set; }
        public long In { get; set; }
        public long Out { get; set; }
        public long BranchId { get; set; }
        public long ShiftId { get; set; }
        public long UserId { get; set; }
        public long PaidStatus { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }      
        public bool OrderByDealer { get; set; } = false;
        public bool OrderByBranch { get; set; } = false;
        public bool OrderByShift { get; set; } = false;
        public bool OrderByUser { get; set; } = false;
    }
}
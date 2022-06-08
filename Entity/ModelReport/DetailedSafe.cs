using System;

namespace Entity.ModelReport
{
    public class DetailedSafe
    {
        public long Id { get; set; }
        public long SafeId { get; set; }
        public string SafeName { get; set; }
        public long TypeId { get; set; }
        public string TypeName { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public long BranchId { get; set; }
        public string BranchName { get; set; }
        public long ShiftId { get; set; }
        public string ShiftName { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }

    }
}
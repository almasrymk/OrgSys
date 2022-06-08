using System;

namespace Entity.ModelReport
{
    public class UnreceivedTransfer
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public DateTime Date { get; set; }
        public long FromStoreId { get; set; }
        public string  FromStoreName { get; set; }
        public long ToStoreId { get; set; }
        public string ToStoreName { get; set; }
        public long BranchId { get; set; }
        public string BranchName { get; set; }
        public long ShiftId { get; set; }
        public string ShiftName { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }
    }
}
using System;

namespace Entity.ModelReport
{
    public class DetailedInvoiceMovement
    {
        public long Id { get; set; }
        public long InvTypeId { get; set; }
        public string InvCode { get; set; }
        public string  InvTypeName { get; set; }
        public long DealerId { get; set; }
        public string DealerName { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public decimal Paid { get; set; }
        public decimal Credit { get; set; }
        public long BranchId { get; set; }
        public string  BranchName { get; set; }
        public long ShiftId { get; set; }
        public string ShiftName { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }
    }
}
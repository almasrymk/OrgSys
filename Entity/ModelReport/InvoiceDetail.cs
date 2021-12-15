using System;
using Utility;
using Entity.Model;


namespace Entity.ModelReport
{
    public class InvoiceDetail : BaseModel
    {
        public InvoiceDetail(Invoice ob )
        {
            this.TypeId = ob.TypeId;
            this.Id = ob.Id;
            this.Code = ob.Code;
            this.Date = ob.Date;
            this.DealerId = ob.DealerId;
            this.DealerName = ob.Dealer?.Name;
            this.ShiftId = ob.ShiftId??0;
            this.ShiftName = ob.Shift?.Name;
            this.BranchId = ob.Store.BranchId;
            this.BranchName = ob.Store.Branch.Name;
            this.Total = ob.Total * ob.Rate;
            this.Paid = ob.Paid;
            this.Credit = ob.Credit;
            this.Net = ob.Net * ob.Rate;
        }

        public DateTime Date { get; set; }
        public long DealerId { get; set; }
        public string DealerName { get; set; }
        public long ShiftId { get; set; }
        public string ShiftName { get; set; }
        public long BranchId { get; set; }
        public string BranchName { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }
        public decimal Total { get; set; }
        public decimal Paid { get; set; }
        public decimal Credit { get; set; }
        public decimal Net { get; set; }
    }
}
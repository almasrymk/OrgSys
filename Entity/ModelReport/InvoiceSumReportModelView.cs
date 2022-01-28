namespace Entity.ModelReport
{
    public class InvoiceSumReportModelView
    {
        public string Period { get; set; }

        public long DealerId { get; set; }

        public string DealerName { get; set; }

        public long BranchId { get; set; }

        public string BranchName { get; set; }

        public long UserId { get; set; }

        public string UserName { get; set; }

        public long ShiftId { get; set; }

        public string ShiftName { get; set; }

        public  long CurrencyId { get; set; }

        public  string CurrencyName { get; set; }

        public decimal TotalRemaining { get; set; }

        public decimal TotalCredit { get; set; }

        public decimal TotalInvoice { get; set; }

        public decimal TotalReturnInvoice { get; set; }

        public decimal NetInvoice { get; set; }
    }
}
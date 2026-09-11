namespace Reporting.Application
{
    public class UnreceivedTransfer
    {
        public long Id { get; set; }

        public string? Code { get; set; }

        public DateTime Date { get; set; }

        public long FromStockId { get; set; }

        public string? FromStockName { get; set; }

        public long ToStockId { get; set; }

        public string? ToStockName { get; set; }

        public long BranchId { get; set; }

        public string? BranchName { get; set; }

        public long ShiftId { get; set; }

        public string? ShiftName { get; set; }

        public long UserId { get; set; }

        public string? UserName { get; set; }
    }
}
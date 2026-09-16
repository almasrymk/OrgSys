namespace Reporting.Application;

public interface IReportingProjectionStore
{
    Task ApplySalesInvoicePostedAsync(long customerId, long invoiceId, DateTime invoiceDate, decimal amount, long? branchId, CancellationToken cancellationToken);

    Task<IReadOnlyList<(long CustomerId, long InvoiceId, DateTime InvoiceDate, decimal OriginalAmount, decimal OutstandingAmount)>> GetAgingAsync(long? customerId, CancellationToken cancellationToken);

    Task<IReadOnlyList<(DateTime SummaryDate, long? BranchId, int InvoiceCount, decimal NetAmount)>> GetSalesSummaryAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken);
}

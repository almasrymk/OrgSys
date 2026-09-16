namespace Reporting.Contracts.Projections;

using OrgSys.SharedKernel;

public sealed record CustomerAgingRowDto(
    long CustomerId,
    long InvoiceId,
    DateTime InvoiceDate,
    decimal OriginalAmount,
    decimal OutstandingAmount,
    int DaysOutstanding);

public sealed record GetCustomerAgingProjectionQuery(long? CustomerId, DateTime AsOfDate)
    : IQuery<IReadOnlyList<CustomerAgingRowDto>>;

public sealed record SalesSummaryRowDto(DateTime SummaryDate, long? BranchId, int InvoiceCount, decimal NetAmount);

public sealed record GetSalesSummaryProjectionQuery(DateTime FromDate, DateTime ToDate)
    : IQuery<IReadOnlyList<SalesSummaryRowDto>>;

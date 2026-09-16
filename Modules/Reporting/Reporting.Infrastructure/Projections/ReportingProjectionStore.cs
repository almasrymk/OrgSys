namespace Reporting.Infrastructure.Projections;

using OrgSys.SharedKernel;
using Reporting.Application;

public sealed class ReportingProjectionStore(
    IRepository<CustomerAgingReadModel> agingRepository,
    IRepository<SalesSummaryReadModel> summaryRepository,
    IUnitOfWork unitOfWork) : IReportingProjectionStore
{
    public async Task ApplySalesInvoicePostedAsync(
        long customerId, long invoiceId, DateTime invoiceDate, decimal amount, long? branchId, CancellationToken cancellationToken)
    {
        var existing = await agingRepository.GetByFilterAsync(e => e.InvoiceId == invoiceId, string.Empty);
        if (existing is null || existing.Id == 0)
        {
            await agingRepository.CreateAsync(new CustomerAgingReadModel
            {
                CustomerId = customerId,
                InvoiceId = invoiceId,
                InvoiceDate = invoiceDate,
                OriginalAmount = amount,
                OutstandingAmount = amount
            });
        }

        var day = invoiceDate.Date;
        var summary = await summaryRepository.GetByFilterAsync(
            e => e.SummaryDate == day && e.BranchId == branchId, string.Empty);
        if (summary is null || summary.Id == 0)
        {
            await summaryRepository.CreateAsync(new SalesSummaryReadModel
            {
                SummaryDate = day,
                BranchId = branchId,
                InvoiceCount = 1,
                NetAmount = amount
            });
        }
        else
        {
            summary.InvoiceCount += 1;
            summary.NetAmount += amount;
            await summaryRepository.UpdateAsync(summary);
        }

        await unitOfWork.SaveChangeAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<(long CustomerId, long InvoiceId, DateTime InvoiceDate, decimal OriginalAmount, decimal OutstandingAmount)>> GetAgingAsync(long? customerId, CancellationToken cancellationToken)
    {
        var rows = customerId is > 0
            ? await agingRepository.GetListByFilterAsync(e => e.CustomerId == customerId.Value)
            : await agingRepository.GetListByFilterAsync(e => true);
        return (rows ?? []).Select(r => (r.CustomerId, r.InvoiceId, r.InvoiceDate, r.OriginalAmount, r.OutstandingAmount)).ToList();
    }

    public async Task<IReadOnlyList<(DateTime SummaryDate, long? BranchId, int InvoiceCount, decimal NetAmount)>> GetSalesSummaryAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken)
    {
        var rows = await summaryRepository.GetListByFilterAsync(e => e.SummaryDate >= fromDate.Date && e.SummaryDate <= toDate.Date);
        return (rows ?? []).Select(r => (r.SummaryDate, r.BranchId, r.InvoiceCount, r.NetAmount)).ToList();
    }
}

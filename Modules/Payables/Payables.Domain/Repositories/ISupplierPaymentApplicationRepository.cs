namespace Payables.Domain.Repositories;

public interface ISupplierPaymentApplicationRepository
{
    Task<bool> ExistsForSourceFinancialAsync(long sourceFinancialId, CancellationToken cancellationToken = default);

    Task AddAsync(SupplierPaymentApplication supplierPaymentApplication, CancellationToken cancellationToken = default);
}

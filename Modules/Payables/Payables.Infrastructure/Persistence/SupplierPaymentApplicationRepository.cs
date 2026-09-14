namespace Payables.Infrastructure.Persistence;

/// <summary>Composed on top of the generic OrgSys.SharedKernel.IRepository&lt;SupplierPaymentApplication&gt; — mirrors PayableRepository/Receivables' PaymentApplicationRepository.</summary>
public sealed class SupplierPaymentApplicationRepository(IRepository<SupplierPaymentApplication> repository) : ISupplierPaymentApplicationRepository
{
    public async Task<bool> ExistsForSourceFinancialAsync(long sourceFinancialId, CancellationToken cancellationToken = default) =>
        await repository.AnyAsync(p => p.SourceFinancialId == sourceFinancialId, cancellationToken);

    public async Task AddAsync(SupplierPaymentApplication supplierPaymentApplication, CancellationToken cancellationToken = default) =>
        await repository.CreateAsync(supplierPaymentApplication);
}

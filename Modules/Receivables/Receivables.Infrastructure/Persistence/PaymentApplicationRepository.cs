namespace Receivables.Infrastructure.Persistence;

/// <summary>Composed on top of the generic OrgSys.SharedKernel.IRepository&lt;PaymentApplication&gt; — mirrors ReceivableRepository/Accounting's JournalRepository.</summary>
public sealed class PaymentApplicationRepository(IRepository<PaymentApplication> repository) : IPaymentApplicationRepository
{
    public async Task<bool> ExistsForSourceFinancialAsync(long sourceFinancialId, CancellationToken cancellationToken = default) =>
        await repository.AnyAsync(p => p.SourceFinancialId == sourceFinancialId, cancellationToken);

    public async Task AddAsync(PaymentApplication paymentApplication, CancellationToken cancellationToken = default) =>
        await repository.CreateAsync(paymentApplication);
}

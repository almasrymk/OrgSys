namespace Receivables.Domain.Repositories;

public interface IPaymentApplicationRepository
{
    Task<bool> ExistsForSourceFinancialAsync(long sourceFinancialId, CancellationToken cancellationToken = default);

    Task AddAsync(PaymentApplication paymentApplication, CancellationToken cancellationToken = default);
}

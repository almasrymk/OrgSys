namespace Accounting.Domain.Repositories;

public interface IFiscalPeriodRepository
{
    Task<FiscalPeriod?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
}

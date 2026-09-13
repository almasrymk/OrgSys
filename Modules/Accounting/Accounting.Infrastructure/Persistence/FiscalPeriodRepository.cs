namespace Accounting.Infrastructure.Persistence;

public sealed class FiscalPeriodRepository(IRepository<FiscalPeriod> repository) : IFiscalPeriodRepository
{
    public async Task<FiscalPeriod?> GetByIdAsync(long id, CancellationToken cancellationToken = default) =>
        await repository.GetByFilterAsync(p => p.Id == id, string.Empty);
}

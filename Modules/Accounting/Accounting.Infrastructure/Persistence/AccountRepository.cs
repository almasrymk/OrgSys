namespace Accounting.Infrastructure.Persistence;

public sealed class AccountRepository(IRepository<Account> repository) : IAccountRepository
{
    public async Task<Account?> GetByIdAsync(long id, CancellationToken cancellationToken = default) =>
        await repository.GetByFilterAsync(a => a.Id == id, string.Empty);

    public async Task<IReadOnlyList<Account>> GetByIdsAsync(IReadOnlyCollection<long> ids, CancellationToken cancellationToken = default)
    {
        var accounts = await repository.GetListByFilterAsync(a => ids.Contains(a.Id));
        return (accounts ?? []).ToList();
    }
}

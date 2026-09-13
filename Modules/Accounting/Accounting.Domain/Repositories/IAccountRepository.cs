namespace Accounting.Domain.Repositories;

public interface IAccountRepository
{
    Task<Account?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    /// <summary>Batch lookup for every Account a Journal's lines reference — used by Post to validate each one is postable/active.</summary>
    Task<IReadOnlyList<Account>> GetByIdsAsync(IReadOnlyCollection<long> ids, CancellationToken cancellationToken = default);
}

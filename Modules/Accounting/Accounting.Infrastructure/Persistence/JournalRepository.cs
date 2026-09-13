namespace Accounting.Infrastructure.Persistence;

/// <summary>
/// Aggregate-shaped Journal persistence, composed on top of the existing generic
/// OrgSys.SharedKernel.IRepository&lt;Journal&gt; rather than reimplementing EF querying — see
/// docs on why the generic repository stays as an Infrastructure implementation helper
/// (Accounting.Domain.Repositories.IJournalRepository's own doc comment / the GeneralLedger
/// migration report).
/// </summary>
public sealed class JournalRepository(IRepository<Journal> repository) : IJournalRepository
{
    public async Task<Journal?> GetByIdAsync(long id, CancellationToken cancellationToken = default) =>
        await repository.GetByFilterAsync(j => j.Id == id, "JournalItems");

    public async Task<Journal?> GetBySourceDocumentAsync(string referenceTable, long sourceDocumentId, long sourceDocumentTypeId, CancellationToken cancellationToken = default) =>
        await repository.GetByFilterAsync(
            j => j.RefranceTable == referenceTable && j.RefranceId == sourceDocumentId && j.RefranceTypeId == sourceDocumentTypeId,
            "JournalItems");

    public async Task<IReadOnlyList<Journal>> GetAllBySourceDocumentAsync(string referenceTable, long sourceDocumentId, CancellationToken cancellationToken = default)
    {
        var journals = await repository.GetListByFilterAsync(j => j.RefranceTable == referenceTable && j.RefranceId == sourceDocumentId);
        return (journals ?? []).ToList();
    }

    public async Task<Journal?> GetOpeningBalanceJournalAsync(long fiscalYearId, long journalTypeId, CancellationToken cancellationToken = default) =>
        await repository.GetByFilterAsync(
            j => j.FiscalYearId == fiscalYearId && j.JournalTypeId == journalTypeId && j.Status != Status.Deleted && j.Status != Status.Cancel,
            "JournalItems");

    public async Task<long> GetNextCodeNumberAsync(long journalTypeId, CancellationToken cancellationToken = default) =>
        await repository.AnyAsync(j => j.TypeId == journalTypeId, cancellationToken)
            ? await repository.GetMaxByFilterAsync(j => j.TypeId == journalTypeId, j => j.CodeNumber) + 1
            : 1;

    public async Task AddAsync(Journal journal, CancellationToken cancellationToken = default) =>
        await repository.CreateAsync(journal);

    public async Task RemoveAsync(Journal journal, CancellationToken cancellationToken = default) =>
        await repository.ShiftDeleteAsync(j => j.Id == journal.Id);
}

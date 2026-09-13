namespace Accounting.Application.Postings;

using Accounting.Contracts.Postings;
using OrgSys.SharedKernel;
using System.Net;

public sealed class GetAccountingDocumentJournalQueryHandler(
    IRepository<Accounting.Domain.Journal> journalRepository)
    : IQueryHandler<GetAccountingDocumentJournalQuery, AccountingDocumentJournalDto?>
{
    public async Task<Result<AccountingDocumentJournalDto?>> Handle(GetAccountingDocumentJournalQuery request, CancellationToken cancellationToken)
    {
        var journal = await journalRepository.GetByFilterAsync(
            e => e.RefranceTable == request.ReferenceTable
                && e.RefranceId == request.SourceDocumentId
                && e.RefranceTypeId == request.SourceDocumentTypeId,
            string.Empty);

        var dto = journal is null ? null : new AccountingDocumentJournalDto(journal.Id, journal.Code);
        return new Result<AccountingDocumentJournalDto?>(HttpStatusCode.OK, dto, null);
    }
}

public sealed class GetAccountingDocumentJournalsQueryHandler(
    IRepository<Accounting.Domain.Journal> journalRepository)
    : IQueryHandler<GetAccountingDocumentJournalsQuery, Dictionary<long, AccountingDocumentJournalDto>>
{
    public async Task<Result<Dictionary<long, AccountingDocumentJournalDto>>> Handle(GetAccountingDocumentJournalsQuery request, CancellationToken cancellationToken)
    {
        if (request.SourceDocumentIds.Count == 0)
            return new Result<Dictionary<long, AccountingDocumentJournalDto>>(HttpStatusCode.OK, [], null);

        var ids = request.SourceDocumentIds.Distinct().ToList();
        var journals = await journalRepository.GetListByFilterAsync(e => e.RefranceTable == request.ReferenceTable && ids.Contains(e.RefranceId));

        var result = (journals ?? [])
            .GroupBy(e => e.RefranceId)
            .ToDictionary(g => g.Key, g => new AccountingDocumentJournalDto(g.First().Id, g.First().Code));

        return new Result<Dictionary<long, AccountingDocumentJournalDto>>(HttpStatusCode.OK, result, null);
    }
}

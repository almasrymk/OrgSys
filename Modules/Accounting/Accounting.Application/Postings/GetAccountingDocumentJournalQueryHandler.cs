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

        var dto = journal is null ? null : new AccountingDocumentJournalDto(journal.Id);
        return new Result<AccountingDocumentJournalDto?>(HttpStatusCode.OK, dto, null);
    }
}

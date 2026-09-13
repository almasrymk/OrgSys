namespace Accounting.Application.Postings;

using Accounting.Contracts.Postings;
using Accounting.Domain.Repositories;
using OrgSys.SharedKernel;
using System.Net;

public sealed class SetAccountingDocumentJournalStatusCommandHandler(
    IJournalRepository journalRepository)
    : ICommandHandler<SetAccountingDocumentJournalStatusCommand>
{
    public async Task<Result> Handle(SetAccountingDocumentJournalStatusCommand request, CancellationToken cancellationToken)
    {
        var journals = await journalRepository.GetAllBySourceDocumentAsync(request.ReferenceTable, request.SourceDocumentId, cancellationToken);

        foreach (var journal in journals)
            journal.SyncStatusFromSourceDocument(request.Status);

        return new Result(HttpStatusCode.OK, null);
    }
}

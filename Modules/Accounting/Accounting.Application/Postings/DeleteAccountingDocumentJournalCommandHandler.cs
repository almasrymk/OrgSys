namespace Accounting.Application.Postings;

using Accounting.Contracts.Postings;
using Accounting.Domain.Repositories;
using OrgSys.SharedKernel;
using System.Net;

public sealed class DeleteAccountingDocumentJournalCommandHandler(
    IJournalRepository journalRepository)
    : ICommandHandler<DeleteAccountingDocumentJournalCommand>
{
    public async Task<Result> Handle(DeleteAccountingDocumentJournalCommand request, CancellationToken cancellationToken)
    {
        var journals = await journalRepository.GetAllBySourceDocumentAsync(request.ReferenceTable, request.SourceDocumentId, cancellationToken);

        foreach (var journal in journals)
            await journalRepository.RemoveAsync(journal, cancellationToken);

        return new Result(HttpStatusCode.OK, null);
    }
}

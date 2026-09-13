namespace Accounting.Application.Postings;

using Accounting.Contracts.Postings;
using OrgSys.SharedKernel;
using System.Net;

public sealed class SetAccountingDocumentJournalStatusCommandHandler(
    IRepository<Accounting.Domain.Journal> journalRepository)
    : ICommandHandler<SetAccountingDocumentJournalStatusCommand>
{
    public async Task<Result> Handle(SetAccountingDocumentJournalStatusCommand request, CancellationToken cancellationToken)
    {
        var journals = await journalRepository.GetListByFilterAsync(
            e => e.RefranceTable == request.ReferenceTable && e.RefranceId == request.SourceDocumentId);

        foreach (var journal in journals ?? [])
        {
            journal.Status = request.Status;
            await journalRepository.UpdateAsync(journal);
        }

        return new Result(HttpStatusCode.OK, null);
    }
}

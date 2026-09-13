namespace Accounting.Application.Postings;

using Accounting.Contracts.Postings;
using OrgSys.SharedKernel;
using System.Net;

public sealed class DeleteAccountingDocumentJournalCommandHandler(
    IRepository<Accounting.Domain.Journal> journalRepository,
    IRepository<Accounting.Domain.JournalItem> journalItemRepository)
    : ICommandHandler<DeleteAccountingDocumentJournalCommand>
{
    public async Task<Result> Handle(DeleteAccountingDocumentJournalCommand request, CancellationToken cancellationToken)
    {
        var journals = await journalRepository.GetListByFilterAsync(
            e => e.RefranceTable == request.ReferenceTable && e.RefranceId == request.SourceDocumentId);

        foreach (var journal in journals ?? [])
        {
            await journalItemRepository.ShiftDeleteAsync(e => e.JournalId == journal.Id);
            await journalRepository.ShiftDeleteAsync(e => e.Id == journal.Id);
        }

        return new Result(HttpStatusCode.OK, null);
    }
}

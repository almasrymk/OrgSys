namespace Accounting.Application.Postings;

using Accounting.Contracts.Postings;
using Accounting.Domain.Repositories;
using OrgSys.SharedKernel;
using System.Net;

/// <summary>
/// Reverses the journal owned by a source document (e.g. Treasury's Financial/FinancialTransfer
/// Reverse handlers) — mirrors Accounting.Application.Journals.Commands.ReverseJournalCommandHandler
/// exactly (fail-fast pre-checks, then resolve today's fiscal period, then Journal.CreateReversal),
/// the resource-controlled equivalent. See the Accounting DDD cleanup report.
/// </summary>
public sealed class ReverseAccountingDocumentJournalCommandHandler(
    IUnitOfWork unitOfWork,
    IJournalRepository journalRepository,
    IAccountingPeriodService accountingPeriodService)
    : ICommandHandler<ReverseAccountingDocumentJournalCommand, ReverseAccountingDocumentJournalResult>
{
    public async Task<Result<ReverseAccountingDocumentJournalResult>> Handle(ReverseAccountingDocumentJournalCommand request, CancellationToken cancellationToken)
    {
        var journals = await journalRepository.GetAllBySourceDocumentAsync(request.ReferenceTable, request.SourceDocumentId, cancellationToken);
        var original = journals.FirstOrDefault();
        if (original is null)
            return new Result<ReverseAccountingDocumentJournalResult>(HttpStatusCode.NotFound, null, [new Error("No journal is linked to this source document.")]);

        if (!original.Posted)
            return new Result<ReverseAccountingDocumentJournalResult>(HttpStatusCode.BadRequest, null, [new Error("Only a Posted journal entry can be reversed.")]);

        if (original.Status == Status.Reversed || original.ReversalJournal is not null)
            return new Result<ReverseAccountingDocumentJournalResult>(HttpStatusCode.BadRequest, null, [new Error("This journal entry has already been reversed.")]);

        if (original.JournalItems.Count == 0)
            return new Result<ReverseAccountingDocumentJournalResult>(HttpStatusCode.BadRequest, null, [new Error("Journal entry has no lines to reverse.")]);

        var reversalDate = DateTime.Now.Date;
        var resolution = await accountingPeriodService.ResolveAndValidateAsync(reversalDate, cancellationToken);
        if (!resolution.Success)
            return new Result<ReverseAccountingDocumentJournalResult>(HttpStatusCode.BadRequest, null, resolution.Errors);

        var codeNumber = await journalRepository.GetNextCodeNumberAsync(original.TypeId, cancellationToken);
        var reversal = original.CreateReversalForSourceDocument(codeNumber, reversalDate, resolution.FiscalYear!, resolution.FiscalPeriod!);
        await journalRepository.AddAsync(reversal, cancellationToken);

        // Unlike the rest of Accounting.Contracts.Postings, this handler saves immediately: the
        // reversal Journal's Id is DB-generated and the result DTO must carry the real value, not
        // 0. The caller (e.g. Treasury) still wraps its own further changes (flipping the Financial
        // row's Status) and this call in one transaction, so atomicity is unaffected.
        if (await unitOfWork.SaveChangeAsync(cancellationToken) <= 0)
            return new Result<ReverseAccountingDocumentJournalResult>(HttpStatusCode.InternalServerError, null, [new Error("Error saving changes")]);

        return new Result<ReverseAccountingDocumentJournalResult>(
            HttpStatusCode.OK, new ReverseAccountingDocumentJournalResult(original.Id, reversal.Id), null);
    }
}

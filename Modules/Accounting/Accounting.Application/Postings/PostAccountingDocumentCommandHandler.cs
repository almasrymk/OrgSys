namespace Accounting.Application.Postings;

using Accounting.Contracts.Postings;
using Accounting.Domain.Exceptions;
using Accounting.Domain.Repositories;
using OrgSys.SharedKernel;
using System.Net;

/// <summary>
/// Owns Journal creation/update for the source-neutral posting bridge — see
/// PostAccountingDocumentCommand. The upsert-by-source-reference and header-field logic mirrors
/// the legacy root Application project's InvoiceJournalIntegration.SyncAsync/
/// TransactionJournalIntegration.SyncAsync exactly (see the legacy-Application-elimination
/// report); this pass additionally routes line replacement through
/// Journal.ReplaceLinesFromSourceDocument so a resource-controlled journal's lines are never
/// mutated by direct JournalItem repository access, and every referenced Account is validated
/// postable — the one gap those bridges never actually checked. See the GeneralLedger migration report.
/// </summary>
public sealed class PostAccountingDocumentCommandHandler(
    IJournalRepository journalRepository,
    IAccountRepository accountRepository)
    : ICommandHandler<PostAccountingDocumentCommand, PostAccountingDocumentResult>
{
    public async Task<Result<PostAccountingDocumentResult>> Handle(PostAccountingDocumentCommand request, CancellationToken cancellationToken)
    {
        var journal = await journalRepository.GetBySourceDocumentAsync(
            request.ReferenceTable, request.SourceDocumentId, request.SourceDocumentTypeId, cancellationToken);

        var accountIds = request.Lines.Select(l => l.AccountId).Distinct().ToList();
        var accounts = (await accountRepository.GetByIdsAsync(accountIds, cancellationToken)).ToDictionary(a => a.Id);

        var lines = request.Lines
            .Select(l => (
                Account: accounts.TryGetValue(l.AccountId, out var account)
                    ? account
                    : throw new AccountNotPostableException($"Account {l.AccountId} referenced by the posting bridge could not be resolved."),
                l.Debit,
                l.Credit,
                l.Note))
            .ToList();

        if (journal is null)
        {
            var codeNumber = await journalRepository.GetNextCodeNumberAsync(request.JournalTypeId, cancellationToken);

            journal = new Accounting.Domain.Journal
            {
                JournalTypeId = request.JournalTypeId,
                TypeId = request.JournalTypeId,
                CodeNumber = codeNumber,
                Code = codeNumber.ToString(),
                Date = request.Date,
                CreateDate = request.CreateDate,
                CreateUserId = request.CreateUserId,
                BranchId = request.BranchId,
                ShiftId = request.ShiftId,
                CurrencyId = request.CurrencyId,
                Rate = request.Rate,
                RefranceId = request.SourceDocumentId,
                RefranceCode = request.SourceDocumentCode,
                RefranceTypeId = request.SourceDocumentTypeId,
                RefranceTable = request.ReferenceTable,
                Note = request.Note
            };

            journal.ReplaceLinesFromSourceDocument(lines);
            await journalRepository.AddAsync(journal, cancellationToken);
        }
        else
        {
            journal.ReplaceLinesFromSourceDocument(lines);

            journal.Date = request.Date;
            journal.ModifyDate = request.ModifyDate;
            journal.ModifyUserId = request.ModifyUserId;
            journal.BranchId = request.BranchId;
            journal.ShiftId = request.ShiftId;
            journal.CurrencyId = request.CurrencyId;
            journal.Rate = request.Rate;
            journal.RefranceCode = request.SourceDocumentCode;
            journal.Note = request.Note;
        }

        // The caller's own UnitOfWork.SaveChangeAsync persists this together with its own
        // changes, exactly as the legacy integration bridges relied on — this handler only stages.
        return new Result<PostAccountingDocumentResult>(HttpStatusCode.OK, new PostAccountingDocumentResult(true), null);
    }
}

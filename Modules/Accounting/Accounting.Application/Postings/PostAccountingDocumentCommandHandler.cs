namespace Accounting.Application.Postings;

using Accounting.Contracts.Postings;
using OrgSys.SharedKernel;
using System.Net;

/// <summary>
/// Owns Journal creation/update for the source-neutral posting bridge — see
/// PostAccountingDocumentCommand. Moved (behavior preserved) from the legacy root Application
/// project's InvoiceJournalIntegration.SyncAsync/TransactionJournalIntegration.SyncAsync, which
/// used to build the Journal/JournalItems themselves inline; the account-resolution/business-rule
/// half of that logic stayed in CommercialDocuments.Application/Inventory.Application, which now
/// call this instead of touching Accounting.Domain.
/// </summary>
public sealed class PostAccountingDocumentCommandHandler(
    IRepository<Accounting.Domain.Journal> journalRepository,
    IRepository<Accounting.Domain.JournalItem> journalItemRepository)
    : ICommandHandler<PostAccountingDocumentCommand, PostAccountingDocumentResult>
{
    public async Task<Result<PostAccountingDocumentResult>> Handle(PostAccountingDocumentCommand request, CancellationToken cancellationToken)
    {
        var journal = await journalRepository.GetByFilterAsync(
            e => e.RefranceTable == request.ReferenceTable
                && e.RefranceId == request.SourceDocumentId
                && e.RefranceTypeId == request.SourceDocumentTypeId,
            "JournalItems");

        var items = request.Lines
            .Select(l => new Accounting.Domain.JournalItem { AccountId = l.AccountId, Debit = l.Debit, Credit = l.Credit, Note = l.Note })
            .ToList();

        if (journal is null)
        {
            var codeNumber = await journalRepository.AnyAsync(e => e.TypeId == request.JournalTypeId)
                ? await journalRepository.GetMaxByFilterAsync(e => e.TypeId == request.JournalTypeId, e => e.CodeNumber) + 1
                : 1;

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
                Note = request.Note,
                JournalItems = items
            };
            await journalRepository.CreateAsync(journal);
        }
        else
        {
            await journalItemRepository.ShiftDeleteAsync(e => e.JournalId == journal.Id);
            foreach (var item in items)
                item.JournalId = journal.Id;
            await journalItemRepository.CreateAsync(items);

            journal.Date = request.Date;
            journal.ModifyDate = request.ModifyDate;
            journal.ModifyUserId = request.ModifyUserId;
            journal.BranchId = request.BranchId;
            journal.ShiftId = request.ShiftId;
            journal.CurrencyId = request.CurrencyId;
            journal.Rate = request.Rate;
            journal.RefranceCode = request.SourceDocumentCode;
            journal.Note = request.Note;
            await journalRepository.UpdateAsync(journal);
        }

        // The caller's own UnitOfWork.SaveChangeAsync persists this together with its own
        // changes, exactly as the legacy integration bridges relied on — this handler only stages.
        return new Result<PostAccountingDocumentResult>(HttpStatusCode.OK, new PostAccountingDocumentResult(true), null);
    }
}

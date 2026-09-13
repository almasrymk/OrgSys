namespace Accounting.Application.Postings;

using Accounting.Contracts.Postings;
using Accounting.Domain.Exceptions;
using Accounting.Domain.Repositories;
using OrgSys.SharedKernel;
using System.Net;

/// <summary>
/// Owns Journal creation for PostAccountingEntryCommand — see that contract's own doc comment for
/// how this differs from PostAccountingDocumentCommandHandler (Draft, upserting): this always
/// creates a brand-new journal and Posts it immediately (Journal.PostForSourceDocument), resolving
/// the FiscalYear/FiscalPeriod the same way the manual Journal Create/Post commands do.
/// </summary>
public sealed class PostAccountingEntryCommandHandler(
    IUnitOfWork unitOfWork,
    IJournalRepository journalRepository,
    IAccountRepository accountRepository,
    IAccountingPeriodService accountingPeriodService)
    : ICommandHandler<PostAccountingEntryCommand, PostAccountingEntryResult>
{
    public async Task<Result<PostAccountingEntryResult>> Handle(PostAccountingEntryCommand request, CancellationToken cancellationToken)
    {
        var resolution = await accountingPeriodService.ResolveAndValidateAsync(request.Date, cancellationToken);
        if (!resolution.Success)
            return new Result<PostAccountingEntryResult>(HttpStatusCode.BadRequest, null, resolution.Errors);

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

        var codeNumber = await journalRepository.GetNextCodeNumberAsync(request.JournalTypeId, cancellationToken);

        var journal = Accounting.Domain.Journal.CreateForSourceDocument(
            request.ReferenceTable, request.SourceDocumentId, request.SourceDocumentTypeId, request.SourceDocumentCode,
            request.JournalTypeId, codeNumber, request.Date, request.CreateUserId, request.CreateDate,
            request.BranchId, request.ShiftId, request.CurrencyId, request.Rate, request.Note);

        journal.ReplaceLinesFromSourceDocument(lines);
        journal.PostForSourceDocument(resolution.FiscalYear!, resolution.FiscalPeriod!, accounts.Values.ToList());

        await journalRepository.AddAsync(journal, cancellationToken);

        // Unlike PostAccountingDocumentCommandHandler, this handler saves immediately: the caller
        // (e.g. Treasury) needs the real DB-generated JournalId back to store on its own source
        // document row within the same still-open transaction — same reasoning as
        // ReverseAccountingDocumentJournalCommandHandler.
        if (await unitOfWork.SaveChangeAsync(cancellationToken) <= 0)
            return new Result<PostAccountingEntryResult>(HttpStatusCode.InternalServerError, null, [new Error("Error saving changes")]);

        return new Result<PostAccountingEntryResult>(HttpStatusCode.OK, new PostAccountingEntryResult(journal.Id, journal.Code), null);
    }
}

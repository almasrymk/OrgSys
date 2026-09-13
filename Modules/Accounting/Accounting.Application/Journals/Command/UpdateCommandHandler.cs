namespace Accounting.Application.Journals.Commands
{
    using Accounting.Domain.Repositories;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Net;

    public sealed class UpdateJournalCommand : Accounting.Application.JournalDto, ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork,
        IRepository<Accounting.Domain.Journal> _Repository,
        IAccountRepository _AccountRepository,
        IAccountingPeriodService _AccountingPeriodService,
        IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateJournalCommand, Accounting.Domain.Journal>(_UnitOfWork, _Repository, mapper, _provider)
    {
        public override async Task<Result> Handle(UpdateJournalCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var journal = await _Repository.GetByFilterAsync(e => e.Id == request.Id, "JournalItems");
                if (journal is null)
                    return new Result(HttpStatusCode.NotFound, [new Error("Journal not found")]);

                // A posted journal is immutable — correct it via ReverseJournalCommand, not by editing.
                if (journal.Posted)
                    return new Result(HttpStatusCode.Forbidden, [new Error("A posted journal entry cannot be edited. Use Reverse instead.")]);

                if (!string.IsNullOrEmpty(journal.RefranceTable))
                    return new Result(HttpStatusCode.Forbidden, [new Error("A journal created from a resource is read-only")]);

                long fiscalYearId = journal.FiscalYearId;
                long fiscalPeriodId = journal.FiscalPeriodId;
                Accounting.Domain.FiscalYear? fiscalYear;

                // Only re-resolve the accounting period when the journal date itself changes —
                // editing other (non-accounting) fields on a Draft must not be blocked by a
                // period that has since closed around the entry's existing, unchanged date.
                if (request.Date.Date != journal.Date.Date)
                {
                    var resolution = await _AccountingPeriodService.ResolveAndValidateAsync(request.Date, cancellationToken);
                    if (!resolution.Success)
                        return new Result(HttpStatusCode.BadRequest, resolution.Errors);

                    fiscalYearId = resolution.FiscalYear!.Id;
                    fiscalPeriodId = resolution.FiscalPeriod!.Id;
                    fiscalYear = resolution.FiscalYear;
                }
                else
                {
                    fiscalYear = await _AccountingPeriodService.GetFiscalYearAsync(fiscalYearId, cancellationToken);
                }

                // Re-validated even when the date is unchanged: the JournalType itself may be switching to/from
                // Opening Balance, which is an accounting-relevant edit that the date-unchanged fast path above must not skip.
                var openingBalanceErrors = await _AccountingPeriodService.ValidateOpeningBalanceAsync(request.JournalTypeId, request.Date, fiscalYear!, journal.Id, cancellationToken);
                if (openingBalanceErrors is { Count: > 0 })
                    return new Result(HttpStatusCode.BadRequest, openingBalanceErrors);

                // Header (non-invariant) fields: a plain overwrite of the tracked entity, same as
                // every other module's generic Update — Draft journals have no header-field
                // invariant beyond the period/opening-balance checks already run above.
                var ob = mapper.Map<Accounting.Domain.Journal>(request);
                ob.FiscalYearId = fiscalYearId;
                ob.FiscalPeriodId = fiscalPeriodId;
                // Draft edits can never flip Posted — that only ever happens via PostJournalCommand.
                ob.Posted = false;

                var res = await _Repository.UpdateAsync(ob);

                // Lines are the protected part of the aggregate: mutated only through
                // AddLine/UpdateLine/RemoveLine, never via a JournalItem repository directly —
                // see Journal.JournalItems' doc comment and the GeneralLedger migration report.
                var resDetails = await ApplyLineChangesAsync(journal, request, cancellationToken);

                return res && resDetails && await _UnitOfWork.SaveChangeAsync(cancellationToken) > 0
                    ? new Result(HttpStatusCode.OK, null)
                    : new Result(HttpStatusCode.InternalServerError, [new Error("Error")]);
            }
            catch (Exception ex)
            {
                return new Result(HttpStatusCode.InternalServerError, [new Error(ex.Message)]);
            }
        }

        private async Task<bool> ApplyLineChangesAsync(Accounting.Domain.Journal journal, UpdateJournalCommand request, CancellationToken cancellationToken)
        {
            var accountIds = request.JournalItems.Select(i => i.AccountId).Distinct().ToList();
            var accounts = (await _AccountRepository.GetByIdsAsync(accountIds, cancellationToken)).ToDictionary(a => a.Id);

            var requestedIds = request.JournalItems.Where(i => i.Id > 0).Select(i => i.Id).ToHashSet();
            foreach (var existingLine in journal.JournalItems.Where(l => !requestedIds.Contains(l.Id)).ToList())
                journal.RemoveLine(existingLine.Id);

            foreach (var line in request.JournalItems)
            {
                if (!accounts.TryGetValue(line.AccountId, out var account))
                    return false;

                if (line.Id > 0)
                    journal.UpdateLine(line.Id, account, line.Debit, line.Credit, line.Note);
                else
                    journal.AddLine(account, line.Debit, line.Credit, line.Note);
            }

            return true;
        }
    }
}

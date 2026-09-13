namespace Accounting.Application.Journals.Commands
{
    using Accounting.Domain.Exceptions;
    using Accounting.Domain.Repositories;
    using OrgSys.SharedKernel;
    using System.Net;

    public sealed class UpdateJournalCommand : Accounting.Application.JournalDto, ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(
        IUnitOfWork _UnitOfWork,
        IJournalRepository _JournalRepository,
        IAccountRepository _AccountRepository,
        IAccountingPeriodService _AccountingPeriodService) : ICommandHandler<UpdateJournalCommand>
    {
        public async Task<Result> Handle(UpdateJournalCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var journal = await _JournalRepository.GetByIdAsync(request.Id, cancellationToken);
                if (journal is null)
                    return new Result(HttpStatusCode.NotFound, [new Error("Journal not found")]);

                // A posted journal is immutable — correct it via ReverseJournalCommand, not by editing.
                if (journal.Posted)
                    return new Result(HttpStatusCode.Forbidden, [new Error("A posted journal entry cannot be edited. Use Reverse instead.")]);

                if (!string.IsNullOrEmpty(journal.RefranceTable))
                    return new Result(HttpStatusCode.Forbidden, [new Error("A journal created from a resource is read-only")]);

                Accounting.Domain.FiscalYear fiscalYear;
                Accounting.Domain.FiscalPeriod? fiscalPeriod = null;

                // Only re-resolve the accounting period when the journal date itself changes —
                // editing other (non-accounting) fields on a Draft must not be blocked by a
                // period that has since closed around the entry's existing, unchanged date.
                if (request.Date.Date != journal.Date.Date)
                {
                    var resolution = await _AccountingPeriodService.ResolveAndValidateAsync(request.Date, cancellationToken);
                    if (!resolution.Success)
                        return new Result(HttpStatusCode.BadRequest, resolution.Errors);

                    fiscalYear = resolution.FiscalYear!;
                    fiscalPeriod = resolution.FiscalPeriod!;
                }
                else
                {
                    fiscalYear = (await _AccountingPeriodService.GetFiscalYearAsync(journal.FiscalYearId, cancellationToken))!;
                }

                // Re-validated even when the date is unchanged: the JournalType itself may be switching to/from
                // Opening Balance, which is an accounting-relevant edit that the date-unchanged fast path above must not skip.
                var openingBalanceErrors = await _AccountingPeriodService.ValidateOpeningBalanceAsync(request.JournalTypeId, request.Date, fiscalYear, journal.Id, cancellationToken);
                if (openingBalanceErrors is { Count: > 0 })
                    return new Result(HttpStatusCode.BadRequest, openingBalanceErrors);

                // Header (non-invariant) fields: Draft journals have no header-field invariant
                // beyond the period/opening-balance checks already run above.
                journal.UpdateHeader(request.JournalTypeId, request.Date, request.CurrencyId, request.Rate, request.Note, request.BranchId, request.ShiftId);

                if (fiscalPeriod is not null)
                    journal.AssignFiscalPeriod(fiscalYear, fiscalPeriod);

                // Lines are the protected part of the aggregate: mutated only through
                // AddLine/UpdateLine/RemoveLine, never via a JournalItem repository directly —
                // see Journal.JournalItems' doc comment and the Accounting DDD cleanup report.
                var resDetails = await ApplyLineChangesAsync(journal, request, cancellationToken);
                if (!resDetails)
                    return new Result(HttpStatusCode.BadRequest, [new Error("One or more journal lines reference an account that could not be resolved.")]);

                return await _UnitOfWork.SaveChangeAsync(cancellationToken) > 0
                    ? new Result(HttpStatusCode.OK, null)
                    : new Result(HttpStatusCode.InternalServerError, [new Error("Error")]);
            }
            catch (AccountingDomainException ex)
            {
                return new Result(HttpStatusCode.BadRequest, [new Error(ex.Message)]);
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

namespace Accounting.Application.Journals.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using Accounting.Application;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Net;

    public sealed class UpdateJournalCommand : Accounting.Application.JournalDto, ICommand, IUpdateCommand<Result>;
    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork,
        IRepository<Accounting.Domain.Journal> _Repository,
        IRepository<Accounting.Domain.JournalItem> _RepositoryJournalInvoice,
        IAccountingPeriodService _AccountingPeriodService,
        IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateJournalCommand, Accounting.Domain.Journal>(_UnitOfWork, _Repository, mapper, _provider)
    {
        public override async Task<Result> Handle(UpdateJournalCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var journal = await _Repository.GetByFilterAsync(e => e.Id == request.Id, string.Empty);
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

                var ob = mapper.Map<Accounting.Domain.Journal>(request);
                ob.FiscalYearId = fiscalYearId;
                ob.FiscalPeriodId = fiscalPeriodId;
                // Draft edits can never flip Posted — that only ever happens via PostJournalCommand.
                ob.Posted = false;

                var res = await _Repository.UpdateAsync(ob);
                var resDetails = await SaveDetials(request);

                return res && resDetails && await _UnitOfWork.SaveChangeAsync(cancellationToken) > 0
                    ? new Result(HttpStatusCode.OK, null)
                    : new Result(HttpStatusCode.InternalServerError, [new Error("Error")]);
            }
            catch (Exception ex)
            {
                return new Result(HttpStatusCode.InternalServerError, [new Error(ex.Message)]);
            }
        }

        override public async Task<bool> SaveDetials(UpdateJournalCommand request)
        {
            #region UpdateProduct
            var ids = request.JournalItems.Select(e => e.Id);
            var removeList = await _RepositoryJournalInvoice.GetListByFilterAsync(e => e.JournalId == request.Id && !ids.Contains(e.Id));

            var res = await RemoveDetails<JournalItem>(removeList!);
            if (!res) return false;
            var ob = mapper.Map<List<JournalItem>>(request.JournalItems);
            res = await UpdateDetails<JournalItem>(ob);
            #endregion

            return res;
        }
    }
}

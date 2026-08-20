namespace Application.Commands.Org.Financials.Journal.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Common.Services;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Domain.Entities;
    using System.Net;

    public sealed class UpdateJournalCommand : Application.DTOs.JournalDto, ICommand, IUpdateCommand<Result>;
    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork,
        IRepository<Domain.Entities.Journal> _Repository,
        IRepository<Domain.Entities.JournalItem> _RepositoryJournalInvoice,
        IAccountingPeriodService _AccountingPeriodService,
        IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateJournalCommand, Domain.Entities.Journal>(_UnitOfWork, _Repository, mapper, _provider)
    {
        public override async Task<Result> Handle(UpdateJournalCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var journal = await _Repository.GetByFilterAsync(e => e.Id == request.Id, string.Empty);
                if (journal is null)
                    return new Result(HttpStatusCode.NotFound, [new Error("Journal not found")]);

                // A posted journal is immutable — correct it via the existing Cancel/Redo workflow, not by editing.
                if (journal.Posted)
                    return new Result(HttpStatusCode.Forbidden, [new Error("A posted journal entry cannot be edited. Use Cancel to reverse it instead.")]);

                if (!string.IsNullOrEmpty(journal.RefranceTable))
                    return new Result(HttpStatusCode.Forbidden, [new Error("A journal created from a resource is read-only")]);

                long fiscalYearId = journal.FiscalYearId;
                long fiscalPeriodId = journal.FiscalPeriodId;

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
                }

                var ob = mapper.Map<Domain.Entities.Journal>(request);
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

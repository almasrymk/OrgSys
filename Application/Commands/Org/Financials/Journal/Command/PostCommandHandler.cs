using Application.Abstraction.Command;
using Application.Common.Commands;
using Application.Common.Services;
using Application.Interfaces.CQRS;
using AutoMapper;
using Domain.Abstraction;
using Domain.Shared;
using System.Net;

namespace Application.Commands.Org.Financials.Journal.Commands
{
    public record PostJournalCommand(long Id) : ICommand, IUpdateCommand<Result>;

    public class PostJournalCommandHandler(
        IUnitOfWork _UnitOfWork,
        IRepository<Domain.Entities.Journal> _Repository,
        IAccountingPeriodService _AccountingPeriodService,
        IMapper mapper, IServiceProvider _provider) :
        UpdateCommandHandler<PostJournalCommand, Domain.Entities.Journal>(_UnitOfWork, _Repository, mapper, _provider)
    {
        public override async Task<Result> Handle(PostJournalCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var journal = await _Repository.GetByFilterAsync(x => x.Id == request.Id, string.Empty);

                if (journal is null)
                    return new Result(HttpStatusCode.NotFound, [new Error("Journal not found")]);

                if (journal.Posted)
                    return new Result(HttpStatusCode.BadRequest, [new Error("Journal entry is already posted.")]);

                if (!string.IsNullOrEmpty(journal.RefranceTable))
                    return new Result(HttpStatusCode.Forbidden, [new Error("A journal created from a resource is controlled by that resource")]);

                await _UnitOfWork.BeginTransactionAsync();
                try
                {
                    // Re-resolve against current DB state inside the posting transaction — the entry
                    // may have been created while the period was open and closed/locked since.
                    var resolution = await _AccountingPeriodService.ResolveAndValidateAsync(journal.Date, cancellationToken);
                    if (!resolution.Success)
                    {
                        await _UnitOfWork.RollbackAsync();
                        return new Result(HttpStatusCode.BadRequest, resolution.Errors);
                    }

                    journal.FiscalYearId = resolution.FiscalYear!.Id;
                    journal.FiscalPeriodId = resolution.FiscalPeriod!.Id;
                    journal.Posted = true;

                    await _Repository.UpdateAsync(journal);

                    if (await _UnitOfWork.SaveChangeAsync(cancellationToken) <= 0)
                    {
                        await _UnitOfWork.RollbackAsync();
                        return new Result(HttpStatusCode.InternalServerError, [new Error("Error saving changes")]);
                    }

                    await _UnitOfWork.CommitAsync();
                    return new Result(HttpStatusCode.OK, null);
                }
                catch
                {
                    await _UnitOfWork.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                return new Result(HttpStatusCode.InternalServerError, [new Error(ex.Message)]);
            }
        }
    }
}

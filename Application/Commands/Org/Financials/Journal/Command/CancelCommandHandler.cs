using Application.Abstraction.Command;
using Application.Common.Commands;
using Application.Interfaces.CQRS;
using AutoMapper;
using Domain.Abstraction;
using Domain.Shared;
using System.Net;

namespace Application.Commands.Org.Financials.Journal.Commands
{
    public record CancelJournalCommand(long Id) : ICommand, IUpdateCommand<Result>;

    public class CancelJournalCommandHandler(IUnitOfWork _UnitOfWork,   IRepository<Domain.Entities.Journal> _Repository,  IMapper mapper, IServiceProvider _provider) :
        UpdateCommandHandler<CancelJournalCommand, Domain.Entities.Journal>(_UnitOfWork, _Repository, mapper, _provider)
    {

        public override async Task<Result> Handle(CancelJournalCommand request, CancellationToken cancellationToken)
        {

            try
            {
                var journal = await _Repository.GetByFilterAsync(x => x.Id == request.Id, await CreateInclude());


                if (journal is null)
                    return new Result(HttpStatusCode.NotFound, new List<Error> { new Error("Invoice not found") });

                journal.Status = Domain.Enums.Status.Cancel;
                var saved = await _UnitOfWork.SaveChangeAsync();

                return saved > 0 ? new Result(HttpStatusCode.OK, null) : new Result(HttpStatusCode.InternalServerError, new List<Error> { new Error("Error saving changes") });
            }
            catch (Exception ex)
            {

                return new Result(HttpStatusCode.InternalServerError, new List<Error> { new Error("Error") });

            }
        }
    }
}
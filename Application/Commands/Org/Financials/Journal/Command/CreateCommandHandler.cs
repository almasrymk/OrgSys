namespace Application.Commands.Org.Financials.Journal.Commands;

using Application.Abstraction.Command;
using Application.Common.Commands;
using Application.Common.Services;
using Application.Interfaces.CQRS;
using AutoMapper;
using Domain.Abstraction;
using Domain.Shared;
using System.Net;

public sealed class CreateJournalCommand : Application.DTOs.JournalDto, ICommand , ICreateCommand<Result>;

public sealed class CreateCommandHandler(
    IUnitOfWork _UnitOfWork,
    IRepository<Domain.Entities.Journal> _Repository,
    IAccountingPeriodService _AccountingPeriodService,
    IMapper mapper) : CreateCommandHandler<CreateJournalCommand, Domain.Entities.Journal>(_UnitOfWork, _Repository , mapper)
{
    public override async Task<Result> Handle(CreateJournalCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var resolution = await _AccountingPeriodService.ResolveAndValidateAsync(request.Date, cancellationToken);
            if (!resolution.Success)
                return new Result(HttpStatusCode.BadRequest, resolution.Errors);

            var ob = mapper.Map<Domain.Entities.Journal>(request);
            ob.FiscalYearId = resolution.FiscalYear!.Id;
            ob.FiscalPeriodId = resolution.FiscalPeriod!.Id;
            // Every journal is created as a Draft — the client cannot force Posted through Create.
            ob.Posted = false;

            await _Repository.CreateAsync(ob);

            return await _UnitOfWork.SaveChangeAsync(cancellationToken) > 0
                ? new Result(HttpStatusCode.OK, null)
                : new Result(HttpStatusCode.InternalServerError, [new Error("Error")]);
        }
        catch (Exception ex)
        {
            return new Result(HttpStatusCode.InternalServerError, [new Error(ex.Message)]);
        }
    }
}

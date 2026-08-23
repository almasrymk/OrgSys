namespace Application.Commands.Org.Financials.Journal.Commands;

using Application.Abstraction.Command;
using Application.Common.Commands;
using Application.Common.Services;
using Application.Interfaces.CQRS;
using AutoMapper;
using Domain.Abstraction;
using Domain.Shared;
using Microsoft.Extensions.Logging;
using System.Net;

public sealed class CreateJournalCommand : Application.DTOs.JournalDto, ICommand , ICreateCommand<Result>;

public sealed class CreateCommandHandler(
    IUnitOfWork _UnitOfWork,
    IRepository<Domain.Entities.Journal> _Repository,
    IAccountingPeriodService _AccountingPeriodService,
    IMapper mapper,
    ILogger<CreateCommandHandler> logger) : CreateCommandHandler<CreateJournalCommand, Domain.Entities.Journal>(_UnitOfWork, _Repository , mapper)
{
    public override async Task<Result> Handle(CreateJournalCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Creating Journal. Date: {Date}, JournalTypeId: {JournalTypeId}, Lines: {LinesCount}",
            request.Date, request.JournalTypeId, request.JournalItems?.Count ?? 0);

        try
        {
            var resolution = await _AccountingPeriodService.ResolveAndValidateAsync(request.Date, cancellationToken);
            if (!resolution.Success)
            {
                logger.LogWarning("Journal creation rejected: {Errors}", string.Join("; ", resolution.Errors.Select(e => e.MessageError)));
                return new Result(HttpStatusCode.BadRequest, resolution.Errors);
            }

            var openingBalanceErrors = await _AccountingPeriodService.ValidateOpeningBalanceAsync(request.JournalTypeId, request.Date, resolution.FiscalYear!, journalId: 0, cancellationToken);
            if (openingBalanceErrors is { Count: > 0 })
            {
                logger.LogWarning("Journal creation rejected: {Errors}", string.Join("; ", openingBalanceErrors.Select(e => e.MessageError)));
                return new Result(HttpStatusCode.BadRequest, openingBalanceErrors);
            }

            var ob = mapper.Map<Domain.Entities.Journal>(request);
            ob.FiscalYearId = resolution.FiscalYear!.Id;
            ob.FiscalPeriodId = resolution.FiscalPeriod!.Id;
            // Every journal is created as a Draft — the client cannot force Posted through Create.
            ob.Posted = false;

            await _Repository.CreateAsync(ob);

            if (await _UnitOfWork.SaveChangeAsync(cancellationToken) > 0)
            {
                logger.LogInformation("Journal {JournalId} saved successfully", ob.Id);
                return new Result(HttpStatusCode.OK, null);
            }

            logger.LogWarning("Journal creation did not persist any changes (SaveChangesAsync returned 0)");
            return new Result(HttpStatusCode.InternalServerError, [new Error("Error")]);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create Journal");
            return new Result(HttpStatusCode.InternalServerError, [new Error(ex.Message)]);
        }
    }
}

namespace Accounting.Application.Journals.Commands;

using Accounting.Domain.Repositories;
using OrgSys.SharedKernel;
using Microsoft.Extensions.Logging;
using System.Net;

public sealed class CreateJournalCommand : Accounting.Application.JournalDto, ICommand , ICreateCommand<Result>;

public sealed class CreateCommandHandler(
    IUnitOfWork _UnitOfWork,
    IJournalRepository _JournalRepository,
    IAccountRepository _AccountRepository,
    IAccountingPeriodService _AccountingPeriodService,
    ILogger<CreateCommandHandler> logger) : ICommandHandler<CreateJournalCommand>
{
    public async Task<Result> Handle(CreateJournalCommand request, CancellationToken cancellationToken)
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

            var lines = request.JournalItems ?? [];
            var accountIds = lines.Select(i => i.AccountId).Distinct().ToList();
            var accounts = (await _AccountRepository.GetByIdsAsync(accountIds, cancellationToken)).ToDictionary(a => a.Id);

            // Every journal is created as a Draft — the client cannot force Posted through Create.
            var journal = Accounting.Domain.Journal.CreateDraft(
                request.JournalTypeId, request.TypeId, request.CodeNumber, request.Code, request.Date,
                request.CreateUserId, request.CreateDate, request.BranchId, request.ShiftId,
                request.CurrencyId, request.Rate, request.Note);

            foreach (var line in lines)
            {
                if (!accounts.TryGetValue(line.AccountId, out var account))
                    return new Result(HttpStatusCode.BadRequest, [new Error($"Account {line.AccountId} could not be resolved.")]);

                journal.AddLine(account, line.Debit, line.Credit, line.Note);
            }

            journal.AssignFiscalPeriod(resolution.FiscalYear!, resolution.FiscalPeriod!);

            await _JournalRepository.AddAsync(journal, cancellationToken);

            if (await _UnitOfWork.SaveChangeAsync(cancellationToken) > 0)
            {
                logger.LogInformation("Journal {JournalId} saved successfully", journal.Id);
                return new Result(HttpStatusCode.OK, null);
            }

            logger.LogWarning("Journal creation did not persist any changes (SaveChangesAsync returned 0)");
            return new Result(HttpStatusCode.InternalServerError, [new Error("Error")]);
        }
        catch (Accounting.Domain.Exceptions.AccountingDomainException ex)
        {
            return new Result(HttpStatusCode.BadRequest, [new Error(ex.Message)]);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create Journal");
            return new Result(HttpStatusCode.InternalServerError, [new Error(ex.Message)]);
        }
    }
}

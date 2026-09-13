namespace Accounting.Application.Postings;

using Accounting.Contracts.Postings;
using Accounting.Domain.Repositories;
using MasterData.Contracts.Currencies;
using MediatR;
using OrgSys.SharedKernel;
using System.Net;

/// <summary>
/// Upserts a party's own line inside the single shared per-fiscal-year Opening Balance journal —
/// mirrors Receivables/Payables' pre-migration SetCustomerOpeningBalanceCommandHandler/
/// SetSupplierOpeningBalanceCommandHandler exactly (find-or-create the JournalType.IsOpeningBlance
/// journal for the fiscal year, then Journal.SetOpeningBalanceLine), now owned by Accounting so
/// Receivables/Payables never touch Journal/JournalItem directly. See the Accounting DDD cleanup report.
/// </summary>
public sealed class SetOpeningBalanceLineCommandHandler(
    IUnitOfWork unitOfWork,
    IJournalRepository journalRepository,
    IAccountRepository accountRepository,
    IRepository<Accounting.Domain.JournalType> journalTypeRepository,
    IRepository<Accounting.Domain.FiscalYear> fiscalYearRepository,
    IAccountingPeriodService accountingPeriodService,
    ISender sender) : ICommandHandler<SetOpeningBalanceLineCommand>
{
    public async Task<Result> Handle(SetOpeningBalanceLineCommand request, CancellationToken cancellationToken)
    {
        if (request.Debit < 0 || request.Credit < 0 || (request.Debit == 0 && request.Credit == 0))
            return new Result(HttpStatusCode.BadRequest, [new Error("Amount must be greater than zero.")]);

        var fiscalYear = await fiscalYearRepository.GetByFilterAsync(e => e.Id == request.FiscalYearId, string.Empty);
        if (fiscalYear is null)
            return new Result(HttpStatusCode.BadRequest, [new Error("Fiscal year not found.")]);

        var openingJournalType = (await journalTypeRepository.GetListByFilterAsync(e => e.IsOpeningBlance))?.FirstOrDefault();
        if (openingJournalType is null)
            return new Result(HttpStatusCode.BadRequest, [new Error("No Opening Balance journal type is configured.")]);

        var accounts = (await accountRepository.GetByIdsAsync([request.AccountId, request.ClearingAccountId], cancellationToken)).ToDictionary(a => a.Id);
        if (!accounts.TryGetValue(request.AccountId, out var account))
            return new Result(HttpStatusCode.BadRequest, [new Error("The account could not be resolved.")]);
        if (!accounts.TryGetValue(request.ClearingAccountId, out var clearingAccount))
            return new Result(HttpStatusCode.BadRequest, [new Error("The opening balance clearing account could not be resolved.")]);

        await unitOfWork.BeginTransactionAsync();
        try
        {
            var journal = await journalRepository.GetOpeningBalanceJournalAsync(fiscalYear.Id, openingJournalType.Id, cancellationToken);

            if (journal is not null && journal.Posted)
            {
                await unitOfWork.RollbackAsync();
                return new Result(HttpStatusCode.BadRequest, [new Error("The Opening Balance journal for this fiscal year is already Posted and immutable. Use Reverse to correct it.")]);
            }

            if (journal is null)
            {
                var resolution = await accountingPeriodService.ResolveAndValidateAsync(fiscalYear.StartDate, cancellationToken);
                if (!resolution.Success)
                {
                    await unitOfWork.RollbackAsync();
                    return new Result(HttpStatusCode.BadRequest, resolution.Errors);
                }

                var obErrors = await accountingPeriodService.ValidateOpeningBalanceAsync(openingJournalType.Id, fiscalYear.StartDate, fiscalYear, 0, cancellationToken);
                if (obErrors.Count > 0)
                {
                    await unitOfWork.RollbackAsync();
                    return new Result(HttpStatusCode.BadRequest, obErrors);
                }

                var currencyResult = await sender.Send(new GetDefaultCurrencyQuery(), cancellationToken);
                var currency = currencyResult.Response;
                if (currency is null)
                {
                    await unitOfWork.RollbackAsync();
                    return new Result(HttpStatusCode.BadRequest, [new Error("A default currency is required to create the Opening Balance journal.")]);
                }

                var codeNumber = await journalRepository.GetNextCodeNumberAsync(openingJournalType.Id, cancellationToken);
                journal = Accounting.Domain.Journal.CreateDraft(
                    openingJournalType.Id, openingJournalType.Id, codeNumber, codeNumber.ToString(), fiscalYear.StartDate,
                    request.CreateUserId, DateTime.Now, null, null, currency.Id, currency.Rate, "Opening Balances");
                journal.AssignFiscalPeriod(resolution.FiscalYear!, resolution.FiscalPeriod!);
                await journalRepository.AddAsync(journal, cancellationToken);
                await unitOfWork.SaveChangeAsync(cancellationToken);
            }

            journal.SetOpeningBalanceLine(account, request.Debit, request.Credit, request.Note, clearingAccount);

            if (await unitOfWork.SaveChangeAsync(cancellationToken) <= 0)
            {
                await unitOfWork.RollbackAsync();
                return new Result(HttpStatusCode.InternalServerError, [new Error("Error saving changes")]);
            }

            await unitOfWork.CommitAsync();
            return new Result(HttpStatusCode.OK, null);
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();
            return new Result(HttpStatusCode.InternalServerError, [new Error(ex.Message)]);
        }
    }
}

namespace Application.Commands.Org.Financials.Unified;

using Application.Abstraction.Command;
using Application.DTOs;
using Application.Interfaces.CQRS;
using Domain.Abstraction;
using Domain.Entities;
using Domain.Enums;
using Domain.Shared;
using MediatR;
using System.Net;

public sealed record SaveFinancialAccountCommand(FinancialAccountDto Account) : ICommand;
public sealed record GetFinancialAccountsQuery(FinancialAccountType? AccountType, bool IncludeInactive = false) : IRequest<ResultCollection<FinancialAccountDto>>;
public sealed record GetFinancialAccountBalanceQuery(long FinancialAccountId, DateTime? AsOfDate = null) : IRequest<Result<decimal>>;
public sealed record PostFinancialTransactionCommand(PostFinancialTransactionDto Transaction) : ICommand, ICreateCommand<Result>;

public sealed class SaveFinancialAccountCommandHandler(
    IRepository<FinancialAccount> repository, IRepository<Safe> safeRepository,
    IRepository<BankAccount> bankRepository, IUnitOfWork unitOfWork,
    Application.Common.Services.IReceivableAccountValidator accountValidator) : ICommandHandler<SaveFinancialAccountCommand>
{
    public async Task<Result> Handle(SaveFinancialAccountCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Account;
        if (string.IsNullOrWhiteSpace(dto.Name))
            return new Result(HttpStatusCode.BadRequest, [new Error("Financial account name is required.")]);
        if (dto.AccountId is > 0)
        {
            var (_, accountErrors) = await accountValidator.ValidateAccountAsync(dto.AccountId.Value, cancellationToken);
            if (accountErrors.Count > 0)
                return new Result(HttpStatusCode.BadRequest, accountErrors);
        }
        var entity = dto.Id > 0
            ? await repository.GetByFilterAsync(e => e.Id == dto.Id, string.Empty)
            : new FinancialAccount();
        if (entity is null)
            return new Result(HttpStatusCode.NotFound, [new Error("Financial account not found.")]);
        entity.Name = dto.Name.Trim();
        entity.Code = dto.Code;
        entity.FinancialAccountType = dto.FinancialAccountType;
        entity.AccountId = dto.AccountId;
        entity.CurrencyId = dto.CurrencyId;
        entity.IsActive = dto.IsActive;
        if (dto.Id == 0) await repository.CreateAsync(entity); else await repository.UpdateAsync(entity);
        await unitOfWork.SaveChangeAsync(cancellationToken);
        if (dto.FinancialAccountType == FinancialAccountType.CashBox)
        {
            var detail = await safeRepository.GetByFilterAsync(e => e.FinancialAccountId == entity.Id, string.Empty)
                ?? new Safe { FinancialAccountId = entity.Id };
            detail.Name = entity.Name;
            detail.AccountId = entity.AccountId;
            detail.BranchId = dto.BranchId;
            detail.KeeperUserId = dto.KeeperUserId;
            if (detail.Id == 0) await safeRepository.CreateAsync(detail); else await safeRepository.UpdateAsync(detail);
        }
        else
        {
            var detail = await bankRepository.GetByFilterAsync(e => e.FinancialAccountId == entity.Id, string.Empty)
                ?? new BankAccount { FinancialAccountId = entity.Id };
            detail.Name = entity.Name;
            detail.AccountId = entity.AccountId;
            detail.BankId = dto.BankId ?? 0;
            detail.BankBranchd = dto.BankBranchId;
            detail.AccountNumber = dto.AccountNumber;
            detail.IBAN = dto.IBAN;
            detail.SwiftCode = dto.SwiftCode;
            detail.BranchName = dto.BranchName;
            if (detail.Id == 0) await bankRepository.CreateAsync(detail); else await bankRepository.UpdateAsync(detail);
        }
        await unitOfWork.SaveChangeAsync(cancellationToken);
        return new Result(HttpStatusCode.OK, null);
    }
}

public sealed class GetFinancialAccountsQueryHandler(IRepository<FinancialAccount> repository)
    : IRequestHandler<GetFinancialAccountsQuery, ResultCollection<FinancialAccountDto>>
{
    public async Task<ResultCollection<FinancialAccountDto>> Handle(GetFinancialAccountsQuery request, CancellationToken cancellationToken)
    {
        var rows = await repository.GetListByFilterAsync(
            e => (request.IncludeInactive || e.IsActive) && (!request.AccountType.HasValue || e.FinancialAccountType == request.AccountType.Value),
            "CashBox,BankAccount,Account,Currency");
        var result = (rows ?? []).Select(e => new FinancialAccountDto
        {
            Id = e.Id, Code = e.Code, Name = e.Name, FinancialAccountType = e.FinancialAccountType,
            AccountId = e.AccountId, AccountName = e.Account?.Name, AccountCode = e.Account?.Code,
            CurrencyId = e.CurrencyId, CurrencyName = e.Currency?.Name, IsActive = e.IsActive,
            BranchId = e.CashBox?.BranchId, KeeperUserId = e.CashBox?.KeeperUserId,
            BankId = e.BankAccount?.BankId, BankBranchId = e.BankAccount?.BankBranchd,
            AccountNumber = e.BankAccount?.AccountNumber, IBAN = e.BankAccount?.IBAN,
            SwiftCode = e.BankAccount?.SwiftCode, BranchName = e.BankAccount?.BranchName
        }).ToList();
        return new ResultCollection<FinancialAccountDto>(HttpStatusCode.OK, result, null);
    }
}

public sealed class PostFinancialTransactionCommandHandler(
    IUnitOfWork unitOfWork,
    IRepository<FinancialAccount> accountRepository,
    IRepository<FinancialType> typeRepository,
    IRepository<Account> glRepository,
    IRepository<Financial> transactionRepository,
    IRepository<Journal> journalRepository,
    Application.Common.Services.IAccountingPeriodService accountingPeriodService) : ICommandHandler<PostFinancialTransactionCommand>
{
    public async Task<Result> Handle(PostFinancialTransactionCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Transaction;
        if (dto.Amount <= 0 || dto.ExchangeRate <= 0)
            return BadRequest("Amount and exchange rate must be greater than zero.");
        var account = await accountRepository.GetByFilterAsync(e => e.Id == dto.FinancialAccountId, string.Empty);
        var type = await typeRepository.GetByFilterAsync(e => e.Id == dto.FinancialTypeId, string.Empty);
        var counter = await glRepository.GetByFilterAsync(e => e.Id == dto.CounterAccountId, string.Empty);
        if (account is null || !account.IsActive || account.AccountId is not > 0 || type is null || counter is null)
            return BadRequest("Financial account, transaction type, and counter account must be valid.");

        var resolution = await accountingPeriodService.ResolveAndValidateAsync(dto.TransactionDate, cancellationToken);
        if (!resolution.Success)
            return new Result(HttpStatusCode.BadRequest, resolution.Errors);

        await unitOfWork.BeginTransactionAsync();
        try
        {
            var now = DateTime.UtcNow;
            var transaction = new Financial
            {
                FinancialAccountId = account.Id,
                FinancialTypeId = type.Id,
                FinancialTransactionType = dto.FinancialTransactionType,
                Direction = dto.Direction,
                ReferenceType = dto.ReferenceType,
                ReferenceId = dto.ReferenceId,
                ReferenceNumber = dto.ReferenceNumber,
                DealerId = dto.DealerId,
                Amount = dto.Amount,
                AmountByDefaultCurrency = dto.Amount * dto.ExchangeRate,
                CurrencyId = dto.CurrencyId,
                Rate = dto.ExchangeRate,
                Date = dto.TransactionDate,
                Notes = dto.Description,
                CreateDate = now,
                CreateUserId = dto.CreateUserId,
                BranchId = dto.BranchId,
                ShiftId = dto.ShiftId,
                Posted = true,
                Status = Status.Approved,
                TypeId = type.Id
            };
            await transactionRepository.CreateAsync(transaction);
            await unitOfWork.SaveChangeAsync(cancellationToken);

            var codeNumber = await journalRepository.AnyAsync(e => e.TypeId == 2, cancellationToken)
                ? await journalRepository.GetMaxByFilterAsync(e => e.TypeId == 2, e => e.CodeNumber) + 1 : 1;
            var financialGlId = account.AccountId.Value;
            var debitId = dto.Direction == FinancialTransactionDirection.In ? financialGlId : counter.Id;
            var creditId = dto.Direction == FinancialTransactionDirection.In ? counter.Id : financialGlId;
            var journal = new Journal
            {
                JournalTypeId = 2, TypeId = 2, CodeNumber = codeNumber, Code = codeNumber.ToString(),
                Date = dto.TransactionDate, CreateDate = now, CreateUserId = dto.CreateUserId,
                BranchId = dto.BranchId, ShiftId = dto.ShiftId, CurrencyId = dto.CurrencyId,
                Rate = dto.ExchangeRate, RefranceId = transaction.Id, RefranceCode = transaction.Code,
                RefranceTypeId = type.Id, RefranceTable = "financialtransaction", Note = dto.Description,
                FiscalYearId = resolution.FiscalYear!.Id, FiscalPeriodId = resolution.FiscalPeriod!.Id,
                Posted = true, Status = Status.Approved,
                JournalItems =
                [
                    new JournalItem { AccountId = debitId, Debit = dto.Amount, Note = dto.Description },
                    new JournalItem { AccountId = creditId, Credit = dto.Amount, Note = dto.Description }
                ]
            };
            await journalRepository.CreateAsync(journal);
            await unitOfWork.SaveChangeAsync(cancellationToken);
            transaction.JournalId = journal.Id;
            transaction.HasJournal = true;
            await transactionRepository.UpdateAsync(transaction);
            await unitOfWork.SaveChangeAsync(cancellationToken);
            await unitOfWork.CommitAsync();
            return new Result(HttpStatusCode.OK, null);
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();
            return new Result(HttpStatusCode.InternalServerError, [new Error(ex.Message)]);
        }
    }

    private static Result BadRequest(string message) => new(HttpStatusCode.BadRequest, [new Error(message)]);
}

public sealed class GetFinancialAccountBalanceQueryHandler(IRepository<Financial> repository)
    : IRequestHandler<GetFinancialAccountBalanceQuery, Result<decimal>>
{
    public async Task<Result<decimal>> Handle(GetFinancialAccountBalanceQuery request, CancellationToken cancellationToken)
    {
        var asOfDate = request.AsOfDate?.Date;
        var rows = await repository.GetListByFilterAsync(e =>
            e.FinancialAccountId == request.FinancialAccountId && e.Posted && e.Status == Status.Approved
            && (asOfDate == null || e.Date.Date <= asOfDate));
        var balance = (rows ?? []).Sum(e => e.Direction == FinancialTransactionDirection.In ? e.Amount : -e.Amount);
        return new Result<decimal>(HttpStatusCode.OK, balance, null);
    }
}

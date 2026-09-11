namespace Treasury.Application.FinancialTransfers.Commands;

using OrgSys.SharedKernel;
using OrgSys.SharedKernel;
using System.Net;
using MediatR;

public sealed record PostFinancialTransferCommand(FinancialTransferDto Transfer)
    : ICommand, ICreateCommand<Result>;
public sealed record GetFinancialTransfersQuery : IRequest<ResultCollection<FinancialTransferDto>>;
public sealed record GetFinancialTransferQuery(long Id) : IRequest<Result<FinancialTransferDto>>;

public sealed class GetFinancialTransfersQueryHandler(
    IRepository<Treasury.Domain.FinancialTransfer> repository,
    IRepository<Journal> journalRepository) : IRequestHandler<GetFinancialTransfersQuery, ResultCollection<FinancialTransferDto>>
{
    public async Task<ResultCollection<FinancialTransferDto>> Handle(GetFinancialTransfersQuery request, CancellationToken token)
    {
        var rows = await repository.GetListByFilterAsync(e => e.Status != Status.Deleted,
            e => e.OrderByDescending(x => x.Date), "FromFinancialAccount,ToFinancialAccount", 1, 1000);
        var result = new List<FinancialTransferDto>();
        foreach (var row in rows ?? [])
        {
            var journal = await journalRepository.GetByFilterAsync(e => e.RefranceTable == "financialtransfer" && e.RefranceId == row.Id, string.Empty);
            result.Add(Map(row, journal?.Id));
        }
        return new ResultCollection<FinancialTransferDto>(HttpStatusCode.OK, result, null);
    }

    internal static FinancialTransferDto Map(Treasury.Domain.FinancialTransfer row, long? journalId) => new()
    {
        Id = row.Id, FromFinancialAccountId = row.FromFinancialAccountId,
        ToFinancialAccountId = row.ToFinancialAccountId, Amount = row.Amount,
        CurrencyId = row.CurrencyId, ExchangeRate = row.ExchangeRate,
        TransactionDate = row.Date, Description = row.Description,
        CreateUserId = row.CreateUserId, BranchId = row.BranchId, ShiftId = row.ShiftId,
        FromFinancialAccountName = row.FromFinancialAccount?.Name,
        ToFinancialAccountName = row.ToFinancialAccount?.Name,
        Status = row.Status, JournalId = journalId
    };
}

public sealed class GetFinancialTransferQueryHandler(
    IRepository<Treasury.Domain.FinancialTransfer> repository,
    IRepository<Journal> journalRepository) : IRequestHandler<GetFinancialTransferQuery, Result<FinancialTransferDto>>
{
    public async Task<Result<FinancialTransferDto>> Handle(GetFinancialTransferQuery request, CancellationToken token)
    {
        var row = await repository.GetByFilterAsync(e => e.Id == request.Id, "FromFinancialAccount,ToFinancialAccount");
        if (row is null) return new Result<FinancialTransferDto>(HttpStatusCode.NotFound, null, [new Error("Transfer not found")]);
        var journal = await journalRepository.GetByFilterAsync(e => e.RefranceTable == "financialtransfer" && e.RefranceId == row.Id, string.Empty);
        return new Result<FinancialTransferDto>(HttpStatusCode.OK, GetFinancialTransfersQueryHandler.Map(row, journal?.Id), null);
    }
}

public sealed class PostFinancialTransferCommandHandler(
    IUnitOfWork unitOfWork,
    IRepository<Treasury.Domain.FinancialTransfer> transferRepository,
    IRepository<FinancialAccount> accountRepository,
    IRepository<Financial> transactionRepository,
    IRepository<Journal> journalRepository,
    Accounting.Application.IAccountingPeriodService accountingPeriodService) : ICommandHandler<PostFinancialTransferCommand>
{
    public async Task<Result> Handle(PostFinancialTransferCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Transfer;
        if (dto.FromFinancialAccountId == dto.ToFinancialAccountId)
            return BadRequest("Source and destination financial accounts must be different.");
        if (dto.Amount <= 0 || dto.ExchangeRate <= 0)
            return BadRequest("Amount and exchange rate must be greater than zero.");

        var source = await accountRepository.GetByFilterAsync(e => e.Id == dto.FromFinancialAccountId, "Account");
        var destination = await accountRepository.GetByFilterAsync(e => e.Id == dto.ToFinancialAccountId, "Account");
        if (source is null || destination is null || !source.IsActive || !destination.IsActive)
            return BadRequest("Both financial accounts must exist and be active.");
        if (source.AccountId is not > 0 || destination.AccountId is not > 0)
            return BadRequest("Both financial accounts must be linked to general-ledger accounts.");

        var resolution = await accountingPeriodService.ResolveAndValidateAsync(dto.TransactionDate, cancellationToken);
        if (!resolution.Success)
            return new Result(HttpStatusCode.BadRequest, resolution.Errors);

        await unitOfWork.BeginTransactionAsync();
        try
        {
            var now = DateTime.UtcNow;
            var transfer = new Treasury.Domain.FinancialTransfer
            {
                FromFinancialAccountId = source.Id,
                ToFinancialAccountId = destination.Id,
                Amount = dto.Amount,
                CurrencyId = dto.CurrencyId,
                ExchangeRate = dto.ExchangeRate,
                Date = dto.TransactionDate,
                Description = dto.Description,
                CreateDate = now,
                CreateUserId = dto.CreateUserId,
                BranchId = dto.BranchId,
                ShiftId = dto.ShiftId,
                Posted = true,
                Status = Status.Approved,
                TypeId = 3
            };
            await transferRepository.CreateAsync(transfer);
            await unitOfWork.SaveChangeAsync(cancellationToken);

            var codeNumber = await journalRepository.AnyAsync(e => e.TypeId == 2, cancellationToken)
                ? await journalRepository.GetMaxByFilterAsync(e => e.TypeId == 2, e => e.CodeNumber) + 1
                : 1;
            var journal = new Journal
            {
                JournalTypeId = 2,
                TypeId = 2,
                CodeNumber = codeNumber,
                Code = codeNumber.ToString(),
                Date = dto.TransactionDate,
                CreateDate = now,
                CreateUserId = dto.CreateUserId,
                BranchId = dto.BranchId,
                ShiftId = dto.ShiftId,
                CurrencyId = dto.CurrencyId,
                Rate = dto.ExchangeRate,
                RefranceId = transfer.Id,
                RefranceCode = transfer.Id.ToString(),
                RefranceTypeId = 3,
                RefranceTable = "financialtransfer",
                Note = dto.Description,
                FiscalYearId = resolution.FiscalYear!.Id,
                FiscalPeriodId = resolution.FiscalPeriod!.Id,
                Posted = true,
                Status = Status.Approved,
                JournalItems =
                [
                    new JournalItem { AccountId = destination.AccountId.Value, Debit = dto.Amount, Note = dto.Description },
                    new JournalItem { AccountId = source.AccountId.Value, Credit = dto.Amount, Note = dto.Description }
                ]
            };
            await journalRepository.CreateAsync(journal);
            await unitOfWork.SaveChangeAsync(cancellationToken);

            var common = new
            {
                dto.CurrencyId, dto.ExchangeRate, dto.Description, dto.CreateUserId,
                dto.BranchId, dto.ShiftId
            };
            var outgoing = CreateMovement(transfer, source.Id, destination.Id,
                FinancialTransactionDirection.Out, Treasury.Domain.FinancialTransactionType.TransferOut, journal.Id,
                common.CurrencyId, common.ExchangeRate,
                common.Description, common.CreateUserId, common.BranchId, common.ShiftId, now);
            var incoming = CreateMovement(transfer, destination.Id, source.Id,
                FinancialTransactionDirection.In, Treasury.Domain.FinancialTransactionType.TransferIn, journal.Id,
                common.CurrencyId, common.ExchangeRate,
                common.Description, common.CreateUserId, common.BranchId, common.ShiftId, now);
            await transactionRepository.CreateAsync([outgoing, incoming]);
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

    private static Financial CreateMovement(
        Treasury.Domain.FinancialTransfer transfer, long accountId, long contraId,
        FinancialTransactionDirection direction, Treasury.Domain.FinancialTransactionType transactionType,
        long journalId, long currencyId,
        decimal rate, string? description, long userId, long? branchId, long? shiftId, DateTime now) => new()
    {
        FinancialAccountId = accountId,
        ContraFinancialAccountId = contraId,
        FinancialTypeId = (long)transactionType,
        // Financial.PaymentTypeId is a required FK with no equivalent field here — default to Cash
        // (Id=1), same fix and same convention as PostTransactionCommandHandler (see that file).
        PaymentTypeId = 1,
        FinancialTransferId = transfer.Id,
        Direction = direction,
        ReferenceType = FinancialReferenceType.Transfer,
        ReferenceId = transfer.Id,
        JournalId = journalId,
        Amount = transfer.Amount,
        AmountByDefaultCurrency = transfer.Amount * rate,
        CurrencyId = currencyId,
        Rate = rate,
        Date = transfer.Date,
        Notes = description,
        CreateDate = now,
        CreateUserId = userId,
        BranchId = branchId,
        ShiftId = shiftId,
        HasJournal = true,
        Posted = true,
        Status = Status.Approved,
        TypeId = 3
    };

    private static Result BadRequest(string message) =>
        new(HttpStatusCode.BadRequest, [new Error(message)]);
}

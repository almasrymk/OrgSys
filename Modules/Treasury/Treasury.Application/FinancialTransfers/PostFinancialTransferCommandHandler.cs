namespace Treasury.Application.FinancialTransfers.Commands;

using Accounting.Contracts.Postings;
using OrgSys.SharedKernel;
using System.Net;
using MediatR;

public sealed record PostFinancialTransferCommand(FinancialTransferDto Transfer)
    : ICommand, ICreateCommand<Result>;
public sealed record GetFinancialTransfersQuery : IRequest<ResultCollection<FinancialTransferDto>>;
public sealed record GetFinancialTransferQuery(long Id) : IRequest<Result<FinancialTransferDto>>;

public sealed class GetFinancialTransfersQueryHandler(
    IRepository<Treasury.Domain.FinancialTransfer> repository,
    ISender sender) : IRequestHandler<GetFinancialTransfersQuery, ResultCollection<FinancialTransferDto>>
{
    public async Task<ResultCollection<FinancialTransferDto>> Handle(GetFinancialTransfersQuery request, CancellationToken token)
    {
        var rows = await repository.GetListByFilterAsync(e => e.Status != Status.Deleted,
            e => e.OrderByDescending(x => x.Date), "FromFinancialAccount,ToFinancialAccount", 1, 1000);
        rows ??= [];

        var journals = (await sender.Send(
            new GetAccountingDocumentJournalsQuery("financialtransfer", rows.Select(r => r.Id).ToList()), token)).Response ?? [];

        var result = rows.Select(row => Map(row, journals.GetValueOrDefault(row.Id)?.JournalId)).ToList();
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
    ISender sender) : IRequestHandler<GetFinancialTransferQuery, Result<FinancialTransferDto>>
{
    public async Task<Result<FinancialTransferDto>> Handle(GetFinancialTransferQuery request, CancellationToken token)
    {
        var row = await repository.GetByFilterAsync(e => e.Id == request.Id, "FromFinancialAccount,ToFinancialAccount");
        if (row is null) return new Result<FinancialTransferDto>(HttpStatusCode.NotFound, null, [new Error("Transfer not found")]);
        var journal = (await sender.Send(new GetAccountingDocumentJournalQuery("financialtransfer", row.Id, 3), token)).Response;
        return new Result<FinancialTransferDto>(HttpStatusCode.OK, GetFinancialTransfersQueryHandler.Map(row, journal?.JournalId), null);
    }
}

public sealed class PostFinancialTransferCommandHandler(
    IUnitOfWork unitOfWork,
    ISender sender,
    IRepository<Treasury.Domain.FinancialTransfer> transferRepository,
    IRepository<FinancialAccount> accountRepository,
    IRepository<Financial> transactionRepository) : ICommandHandler<PostFinancialTransferCommand>
{
    public async Task<Result> Handle(PostFinancialTransferCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Transfer;
        if (dto.FromFinancialAccountId == dto.ToFinancialAccountId)
            return BadRequest("Source and destination financial accounts must be different.");
        if (dto.Amount <= 0 || dto.ExchangeRate <= 0)
            return BadRequest("Amount and exchange rate must be greater than zero.");

        // "Account" (the linked Accounting.Domain.Account) was dropped from these includes — it was
        // never dereferenced here, only source.AccountId/destination.AccountId (a plain scalar FK)
        // are used below. See the GeneralLedger migration report on why FinancialAccount.Account
        // (the navigation) was removed.
        var source = await accountRepository.GetByFilterAsync(e => e.Id == dto.FromFinancialAccountId, string.Empty);
        var destination = await accountRepository.GetByFilterAsync(e => e.Id == dto.ToFinancialAccountId, string.Empty);
        if (source is null || destination is null || !source.IsActive || !destination.IsActive)
            return BadRequest("Both financial accounts must exist and be active.");
        if (source.AccountId is not > 0 || destination.AccountId is not > 0)
            return BadRequest("Both financial accounts must be linked to general-ledger accounts.");

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

            var postResult = await sender.Send(new PostAccountingEntryCommand(
                ReferenceTable: "financialtransfer",
                SourceDocumentId: transfer.Id,
                SourceDocumentTypeId: 3,
                SourceDocumentCode: transfer.Id.ToString(),
                JournalTypeId: 2,
                Date: dto.TransactionDate,
                CreateDate: now,
                CreateUserId: dto.CreateUserId,
                BranchId: dto.BranchId,
                ShiftId: dto.ShiftId,
                CurrencyId: dto.CurrencyId,
                Rate: dto.ExchangeRate,
                Note: dto.Description,
                Lines:
                [
                    new AccountingPostingLine(destination.AccountId!.Value, dto.Amount, 0, dto.Description),
                    new AccountingPostingLine(source.AccountId!.Value, 0, dto.Amount, dto.Description)
                ]), cancellationToken);

            if (postResult.Response is null)
            {
                await unitOfWork.RollbackAsync();
                return new Result(postResult.StatusCode, postResult.Errors);
            }

            var journalId = postResult.Response.JournalId;
            var common = new
            {
                dto.CurrencyId, dto.ExchangeRate, dto.Description, dto.CreateUserId,
                dto.BranchId, dto.ShiftId
            };
            var outgoing = CreateMovement(transfer, source.Id, destination.Id,
                FinancialTransactionDirection.Out, Treasury.Domain.FinancialTransactionType.TransferOut, journalId,
                common.CurrencyId, common.ExchangeRate,
                common.Description, common.CreateUserId, common.BranchId, common.ShiftId, now);
            var incoming = CreateMovement(transfer, destination.Id, source.Id,
                FinancialTransactionDirection.In, Treasury.Domain.FinancialTransactionType.TransferIn, journalId,
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

namespace Application.Commands.Org.Financials.Financial.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Services;
    using Application.Interfaces.CQRS;
    using Domain.Abstraction;
    using Domain.Entities;
    using Domain.Enums;
    using Domain.Shared;
    using Microsoft.Extensions.Logging;
    using System.Linq;
    using System.Net;

    public sealed record ReverseFinancialCommand(long Id) : ICommand, IUpdateCommand<Result>;

    /// <summary>
    /// Reverse for a single-leg Posted <see cref="Financial"/> row (Receipt/Payment/Deposit/.../Opening
    /// Balance): mirrors <c>Journal/Command/ReverseCommandHandler.cs</c> and
    /// <c>FinancialTransfer/ReverseFinancialTransferCommandHandler.cs</c> — the original Journal and its
    /// lines are never touched, a new Posted Journal is created with every line's Debit/Credit swapped,
    /// and the original's Status flips to Reversed. A Transfer leg (which shares ONE Journal between TWO
    /// Financial rows) is explicitly rejected here — use ReverseFinancialTransferCommand for that.
    /// </summary>
    public sealed class ReverseFinancialCommandHandler(
        IUnitOfWork unitOfWork,
        IRepository<Financial> financialRepository,
        IRepository<Journal> journalRepository,
        IAccountingPeriodService accountingPeriodService,
        ILogger<ReverseFinancialCommandHandler> logger) : ICommandHandler<ReverseFinancialCommand>
    {
        public async Task<Result> Handle(ReverseFinancialCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var financial = await financialRepository.GetByFilterAsync(e => e.Id == request.Id, string.Empty);
                if (financial is null)
                    return new Result(HttpStatusCode.NotFound, [new Error("Financial transaction not found.")]);

                if (!financial.Posted)
                    return new Result(HttpStatusCode.BadRequest, [new Error("Only a Posted financial transaction can be reversed.")]);

                if (financial.Status == Status.Reversed)
                    return new Result(HttpStatusCode.BadRequest, [new Error("This financial transaction has already been reversed.")]);

                if (financial.FinancialTransferId is not null)
                    return new Result(HttpStatusCode.BadRequest, [new Error("This is one leg of a Transfer — reverse the Transfer instead.")]);

                if (financial.JournalId is not > 0)
                    return new Result(HttpStatusCode.BadRequest, [new Error("This financial transaction has no linked Journal Entry to reverse.")]);

                var original = await journalRepository.GetByFilterAsync(e => e.Id == financial.JournalId, "JournalItems");
                if (original is null || original.JournalItems is null || original.JournalItems.Count == 0)
                    return new Result(HttpStatusCode.BadRequest, [new Error("The linked Journal Entry has no lines to reverse.")]);

                if (original.Status == Status.Reversed || original.ReversalJournal is not null)
                    return new Result(HttpStatusCode.BadRequest, [new Error("The linked Journal Entry has already been reversed.")]);

                await unitOfWork.BeginTransactionAsync();
                try
                {
                    var reversalDate = DateTime.Now.Date;
                    var resolution = await accountingPeriodService.ResolveAndValidateAsync(reversalDate, cancellationToken);
                    if (!resolution.Success)
                    {
                        await unitOfWork.RollbackAsync();
                        return new Result(HttpStatusCode.BadRequest, resolution.Errors);
                    }

                    var maxCodeNumber = await journalRepository.GetMaxByFilterAsync(e => e.TypeId == original.TypeId, e => e.CodeNumber);
                    var codeNumber = maxCodeNumber + 1;

                    var reversal = new Journal
                    {
                        JournalTypeId = original.JournalTypeId,
                        CurrencyId = original.CurrencyId,
                        Rate = original.Rate,
                        Date = reversalDate,
                        FiscalYearId = resolution.FiscalYear!.Id,
                        FiscalPeriodId = resolution.FiscalPeriod!.Id,
                        Note = $"Reversal of Journal Entry {original.Code}",
                        TypeId = original.TypeId,
                        CodeNumber = codeNumber,
                        Code = codeNumber.ToString(),
                        CreateUserId = original.CreateUserId,
                        CreateDate = DateTime.Now,
                        Posted = true,
                        Status = Status.New,
                        OriginalJournalId = original.Id,
                        // Kept resource-controlled, like the original, so it can't be re-reversed or
                        // manually Posted/Cancelled through the generic Journal endpoints.
                        RefranceTable = original.RefranceTable,
                        RefranceId = original.RefranceId,
                        RefranceCode = original.RefranceCode,
                        RefranceTypeId = original.RefranceTypeId,
                        JournalItems = original.JournalItems.Select(line => new JournalItem
                        {
                            AccountId = line.AccountId,
                            Debit = line.Credit,
                            Credit = line.Debit,
                            Note = line.Note,
                            Status = Status.New
                        }).ToList()
                    };

                    await journalRepository.CreateAsync(reversal);

                    original.Status = Status.Reversed;
                    await journalRepository.UpdateAsync(original);

                    financial.Status = Status.Reversed;
                    await financialRepository.UpdateAsync(financial);

                    if (await unitOfWork.SaveChangeAsync(cancellationToken) <= 0)
                    {
                        await unitOfWork.RollbackAsync();
                        return new Result(HttpStatusCode.InternalServerError, [new Error("Error saving changes")]);
                    }

                    await unitOfWork.CommitAsync();
                    logger.LogInformation("Financial {FinancialId} reversed by new Journal {ReversalId}", financial.Id, reversal.Id);
                    return new Result(HttpStatusCode.OK, null);
                }
                catch
                {
                    await unitOfWork.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to reverse Financial {Id}", request.Id);
                return new Result(HttpStatusCode.InternalServerError, [new Error(ex.Message)]);
            }
        }
    }
}

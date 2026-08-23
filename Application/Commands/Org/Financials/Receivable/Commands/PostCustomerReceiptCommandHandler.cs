namespace Application.Commands.Org.Financials.Receivable.Commands
{
    using Application.Abstraction.Command;
    using Application.Commands.Org.Financials.Unified;
    using Application.Common.Services;
    using Application.DTOs;
    using Application.Interfaces.CQRS;
    using Domain.Enums;
    using Domain.Shared;
    using MediatR;
    using System.Net;

    /// <summary>Money collected from a Customer. Books Dr Cash/Bank, Cr the Customer's receivable
    /// account by delegating to the existing unified <see cref="PostFinancialTransactionCommand"/>
    /// engine — a Customer Receipt is a <c>Financial</c> row (FinancialTypeId = Receipt), not a
    /// separate entity, per the existing FinancialTransactions module.</summary>
    public sealed record PostCustomerReceiptCommand(PostCustomerReceiptDto Receipt) : ICommand, ICreateCommand<Result>;

    public sealed class PostCustomerReceiptCommandHandler(
        IReceivableAccountValidator _Validator,
        ISender _Sender) : ICommandHandler<PostCustomerReceiptCommand>
    {
        /// <summary>Seeded <c>FinancialType</c> row (see <c>Infrastructure/Seed/InitialData.cs</c>, InitialFinancialType).</summary>
        private const long ReceiptFinancialTypeId = 2;

        public async Task<Result> Handle(PostCustomerReceiptCommand request, CancellationToken cancellationToken)
        {
            var receipt = request.Receipt;

            if (receipt.Amount <= 0)
                return new Result(HttpStatusCode.BadRequest, [new Error("Amount must be greater than zero.")]);

            if (receipt.ExchangeRate <= 0)
                return new Result(HttpStatusCode.BadRequest, [new Error("Exchange rate must be greater than zero.")]);

            var (_, account, errors) = await _Validator.ValidateCustomerAsync(receipt.DealerId, cancellationToken);
            if (errors.Count > 0)
                return new Result(HttpStatusCode.BadRequest, errors);

            var dto = new PostFinancialTransactionDto
            {
                FinancialAccountId = receipt.FinancialAccountId,
                FinancialTypeId = ReceiptFinancialTypeId,
                Direction = FinancialTransactionDirection.In,
                Amount = receipt.Amount,
                CurrencyId = receipt.CurrencyId,
                ExchangeRate = receipt.ExchangeRate,
                TransactionDate = receipt.Date,
                ReferenceType = FinancialReferenceType.Customer,
                ReferenceId = receipt.DealerId,
                CounterAccountId = account!.Id,
                DealerId = receipt.DealerId,
                Description = receipt.ReferenceNumber is { Length: > 0 }
                    ? $"{receipt.ReferenceNumber} — {receipt.Notes}"
                    : receipt.Notes,
                CreateUserId = receipt.CreateUserId,
                BranchId = receipt.BranchId,
                ShiftId = receipt.ShiftId
            };

            return await _Sender.Send(new PostFinancialTransactionCommand(dto), cancellationToken);
        }
    }
}

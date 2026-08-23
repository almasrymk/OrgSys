namespace Application.Commands.Org.Financials.Payable.Commands
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

    /// <summary>Money paid to a Supplier. Books Dr the Supplier's payable account, Cr Cash/Bank by
    /// delegating to the existing unified <see cref="PostFinancialTransactionCommand"/> engine — a
    /// Supplier Payment is a <c>Financial</c> row (FinancialTypeId = Payment), not a separate entity,
    /// per the existing FinancialTransactions module. Mirrors <c>PostCustomerReceiptCommandHandler</c>
    /// on the AR side.</summary>
    public sealed record PostSupplierPaymentCommand(PostSupplierPaymentDto Payment) : ICommand, ICreateCommand<Result>;

    public sealed class PostSupplierPaymentCommandHandler(
        IPayableAccountValidator _Validator,
        ISender _Sender) : ICommandHandler<PostSupplierPaymentCommand>
    {
        /// <summary>Seeded <c>FinancialType</c> row (see <c>Infrastructure/Seed/InitialData.cs</c>, InitialFinancialType).</summary>
        private const long PaymentFinancialTypeId = 3;

        public async Task<Result> Handle(PostSupplierPaymentCommand request, CancellationToken cancellationToken)
        {
            var payment = request.Payment;

            if (payment.Amount <= 0)
                return new Result(HttpStatusCode.BadRequest, [new Error("Amount must be greater than zero.")]);

            if (payment.ExchangeRate <= 0)
                return new Result(HttpStatusCode.BadRequest, [new Error("Exchange rate must be greater than zero.")]);

            var (_, account, errors) = await _Validator.ValidateSupplierAsync(payment.DealerId, cancellationToken);
            if (errors.Count > 0)
                return new Result(HttpStatusCode.BadRequest, errors);

            var dto = new PostFinancialTransactionDto
            {
                FinancialAccountId = payment.FinancialAccountId,
                FinancialTypeId = PaymentFinancialTypeId,
                Direction = FinancialTransactionDirection.Out,
                Amount = payment.Amount,
                CurrencyId = payment.CurrencyId,
                ExchangeRate = payment.ExchangeRate,
                TransactionDate = payment.Date,
                ReferenceType = FinancialReferenceType.Supplier,
                ReferenceId = payment.DealerId,
                CounterAccountId = account!.Id,
                DealerId = payment.DealerId,
                Description = payment.ReferenceNumber is { Length: > 0 }
                    ? $"{payment.ReferenceNumber} — {payment.Notes}"
                    : payment.Notes,
                CreateUserId = payment.CreateUserId,
                BranchId = payment.BranchId,
                ShiftId = payment.ShiftId
            };

            return await _Sender.Send(new PostFinancialTransactionCommand(dto), cancellationToken);
        }
    }
}

using Application.Commands.Org.Financials.Payable.Commands;
using Application.Commands.Org.Financials.Payable.Queries;
using Application.Commands.Org.Financials.Receivable.Commands;
using Application.Commands.Org.Setting.Dealer.Queries;
using Application.DTOs;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Financials;

/// <summary>Accounts Payable (Supplier) endpoints. New controller — deliberately does not touch
/// <c>FinancialController</c> — that lets this land independently of the concurrent AR/Receivable work
/// in that file. Money movement itself is still the existing <c>Financial</c>/<c>Journal</c> machinery;
/// this only adds the Supplier-shaped entry points over it.</summary>
[Route("[controller]")]
[ApiController]
public sealed class PayableController(ISender sender) : ControllerBase
{
    [HttpPost("Payment")]
    public Task<Result> PostSupplierPayment(
        [FromBody] PostSupplierPaymentDto payment,
        CancellationToken cancellationToken) =>
        sender.Send(new PostSupplierPaymentCommand(payment), cancellationToken);

    /// <summary>Reverses a Posted Supplier Payment. Delegates to the existing, dealer-agnostic
    /// <see cref="ReverseCustomerReceiptCommand"/> — it reverses any Posted <c>Financial</c> row's
    /// Journal by Id and has no Customer-specific logic, so reusing it here avoids duplicating the
    /// reversal engine. (Candidate for a neutral rename once AR and AP are reviewed together.)</summary>
    [HttpPut("Payment/Reverse")]
    public Task<Result> ReverseSupplierPayment(long id, CancellationToken cancellationToken) =>
        sender.Send(new ReverseCustomerReceiptCommand(id), cancellationToken);

    [HttpGet("Payments")]
    public Task<ResultCollection<FinancialDto>> SupplierPayments(long dealerId, CancellationToken cancellationToken) =>
        sender.Send(new GetSupplierPaymentsQuery(dealerId), cancellationToken);

    /// <summary>Supplier payable balance as of an optional date. Delegates to the existing, dealer-type
    /// agnostic <see cref="GetDealerBalanceQuery"/> (Debit − Credit on the dealer's GL account) and
    /// negates it, since a payable is naturally presented as a credit balance.</summary>
    [HttpGet("Balance")]
    public async Task<Result<decimal>> SupplierBalance(long dealerId, DateTime? asOfDate, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetDealerBalanceQuery(dealerId, asOfDate), cancellationToken);
        return new Result<decimal>(result.StatusCode, -result.Response, result.Errors);
    }

    [HttpPost("OpeningBalance")]
    public Task<Result> SetSupplierOpeningBalance(
        [FromBody] SetSupplierOpeningBalanceCommand command,
        CancellationToken cancellationToken) =>
        sender.Send(command, cancellationToken);
}

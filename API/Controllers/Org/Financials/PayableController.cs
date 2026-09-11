using Payables.Application.OpeningBalance.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Financials;

/// <summary>Accounts Payable (Supplier) opening balance. Supplier Payment posting/reversal/listing
/// lives on the unified <c>Financial</c>/<c>Journal</c> Receipt/Payment path instead of a dedicated
/// AP screen — see FinancialAccountArchitecture review.</summary>
[Route("[controller]")]
[ApiController]
public sealed class PayableController(ISender sender) : ControllerBase
{
    [HttpPost("OpeningBalance")]
    public Task<Result> SetSupplierOpeningBalance(
        [FromBody] SetSupplierOpeningBalanceCommand command,
        CancellationToken cancellationToken) =>
        sender.Send(command, cancellationToken);
}

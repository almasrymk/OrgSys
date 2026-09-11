namespace Treasury.Application.Financials.Command;

using OrgSys.SharedKernel;
using System.Net;
using Treasury.Contracts.Financials;

/// <summary>
/// Handles the Contracts-facing DeleteFinancialsByInvoiceCommand — deletes the Financial linked
/// to a Sales Invoice being deleted, via FinancialInvoices. No-op if none is linked. Mirrors the
/// logic Sales' Invoice DeleteCommandHandler/DeleteListCommandHandler used to run inline against
/// Treasury.Domain directly — see docs/dependency-rules.md.
/// </summary>
public sealed class DeleteFinancialsByInvoiceCommandHandler(IRepository<Financial> financialRepository)
    : ICommandHandler<DeleteFinancialsByInvoiceCommand>
{
    public async Task<Result> Handle(DeleteFinancialsByInvoiceCommand request, CancellationToken cancellationToken)
    {
        var financial = await financialRepository.GetByFilterAsync(
            e => e.FinancialInvoices!.Select(f => f.InvoiceId).Contains(request.InvoiceId),
            "FinancialInvoices");

        if (financial != null)
            await financialRepository.ShiftDeleteAsync(f => f.Id == financial.Id);

        return new Result(HttpStatusCode.OK, null);
    }
}

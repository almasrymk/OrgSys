namespace CommercialDocuments.Application.Invoices.Commands;

using OrgSys.SharedKernel;
using global::Application.Commands.Org.Financials.Integration.JournalInvoice;
using OrgSys.SharedKernel;
using System.Net;

public sealed record CreateJournalByInvoiceCommand(long InvoiceId) : ICommand;

public sealed class CreateJournalByInvoiceCommandHandler(
    IRepository<CommercialDocuments.Domain.Invoice> repository,
    IUnitOfWork unitOfWork,
    IServiceProvider provider) : ICommandHandler<CreateJournalByInvoiceCommand>
{
    public async Task<Result> Handle(CreateJournalByInvoiceCommand request, CancellationToken cancellationToken)
    {
        var invoice = await repository.GetByFilterAsync(e => e.Id == request.InvoiceId, string.Empty);
        if (invoice == null)
            return new Result(HttpStatusCode.NotFound, [new Error("Invoice not found")]);

        try
        {
            await new InvoiceJournalIntegration(provider).SyncAsync(invoice, force: true);
            await repository.UpdateAsync(invoice);
            await unitOfWork.SaveChangeAsync(cancellationToken);
            return new Result(HttpStatusCode.OK, null);
        }
        catch (Exception ex)
        {
            return new Result(HttpStatusCode.InternalServerError, [new Error(ex.Message)]);
        }
    }
}

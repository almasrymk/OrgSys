namespace CommercialDocuments.Application.Invoices.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Net;
    using global::Application.Commands.Org.Financials.Integration.JournalInvoice;
    using Inventory.Contracts.Transactions;
    using MediatR;

    public sealed class CreateInvoiceCommand : InvoiceDto, ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<CommercialDocuments.Domain.Invoice> _Repository,
        IRepository<Administration.Domain.Preference> preferenceRepository, IMapper mapper, IServiceProvider provider,
        ISender sender) : CreateCommandHandler<CreateInvoiceCommand, CommercialDocuments.Domain.Invoice>(_UnitOfWork, _Repository , mapper)
    {
        public override async Task<Result> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
        {
            var autoCreateTransaction = await preferenceRepository.GetByFilterAsync(
                e => e.Reference == "Invoice"
                    && e.TypeId == request.TypeId
                    && e.Key == "AutoCreateTransaction",
                "");

            CommercialDocuments.Domain.Invoice invoice;
            await _UnitOfWork.BeginTransactionAsync();
            try
            {
                invoice = mapper.Map<CommercialDocuments.Domain.Invoice>(request);
                await _Repository.CreateAsync(invoice);
                await _UnitOfWork.SaveChangeAsync(cancellationToken);

                await new InvoiceJournalIntegration(provider).SyncAsync(invoice);
                await _UnitOfWork.SaveChangeAsync(cancellationToken);
                await _UnitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                await _UnitOfWork.RollbackAsync();
                return new Result(HttpStatusCode.InternalServerError, [new Error(ex.Message)]);
            }

            if (autoCreateTransaction?.Value == "1")
                return await sender.Send(new CreateTransactionByInvoiceCommand(invoice.Id), cancellationToken);

            return new Result(HttpStatusCode.OK, null);
        }
    }
}

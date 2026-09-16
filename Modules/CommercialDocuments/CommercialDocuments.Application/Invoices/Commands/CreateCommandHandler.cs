namespace CommercialDocuments.Application.Invoices.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Net;
    using CommercialDocuments.Application.Invoices.Integration;
    using CommercialDocuments.Contracts.Invoices;
    using CommercialDocuments.Contracts.IntegrationEvents;
    using Inventory.Contracts.Transactions;
    using MediatR;
    using Tax.Contracts.Snapshots;

    public sealed class CreateInvoiceCommand : InvoiceDto, ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<CommercialDocuments.Domain.Invoice> _Repository,
        IMapper mapper,
        ISender sender, IIntegrationEventPublisher integrationEventPublisher) : CreateCommandHandler<CreateInvoiceCommand, CommercialDocuments.Domain.Invoice>(_UnitOfWork, _Repository , mapper)
    {
        public override async Task<Result> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
        {
            var autoCreateTransaction = (await sender.Send(
                new Administration.Contracts.Preferences.GetPreferenceValueQuery("Invoice", request.TypeId, "AutoCreateTransaction"),
                cancellationToken)).Response;

            CommercialDocuments.Domain.Invoice invoice;
            await _UnitOfWork.BeginTransactionAsync();
            try
            {
                invoice = mapper.Map<CommercialDocuments.Domain.Invoice>(request);
                await _Repository.CreateAsync(invoice);
                await _UnitOfWork.SaveChangeAsync(cancellationToken);

                await new InvoiceJournalPostingService(sender).SyncAsync(invoice);
                await _UnitOfWork.SaveChangeAsync(cancellationToken);

                // Notify Receivables that a Sales Invoice reached AR-integrated GL posting — see
                // docs/architecture/receivables-ddd-migration.md §9. Published before commit, inside
                // the same transaction (see OrgSys.EventBus.IIntegrationEvent's own doc comment on
                // why: publish/handle stays same-request/same-transaction until module DbContexts
                // require eventual consistency), so a failure in Receivables rolls back the whole
                // invoice creation instead of leaving GL posting committed with no AR open item. This
                // is a side-effect of the posting above, never a second posting decision;
                // Purchase/Return invoice types are not AR's concern (see SourceDocumentType's own
                // doc comment on scope).
                if (invoice.TypeId == (long)InvoiceTypeId.Sales && invoice.HasJournal)
                    await integrationEventPublisher.PublishAsync(
                        new SalesInvoicePostedIntegrationEvent(
                            InvoiceId: invoice.Id,
                            InvoiceNumber: invoice.Code,
                            CustomerId: invoice.DealerId,
                            InvoiceDate: invoice.Date,
                            DueDate: invoice.Date,
                            CurrencyId: invoice.CurrencyId,
                            Rate: invoice.Rate,
                            Amount: invoice.Net,
                            CreateUserId: invoice.CreateUserId,
                            CreateDate: invoice.CreateDate,
                            BranchId: invoice.BranchId),
                        cancellationToken);

                // Same reasoning, AP side: notify Payables that a Purchase Invoice reached
                // AP-integrated GL posting — see docs/architecture/payables-ddd-migration.md.
                // PurchaseReturn is not wired (deferred, same as SalesReturn on the AR side).
                if (invoice.TypeId == (long)InvoiceTypeId.Purchase && invoice.HasJournal)
                    await integrationEventPublisher.PublishAsync(
                        new PurchaseInvoicePostedIntegrationEvent(
                            InvoiceId: invoice.Id,
                            InvoiceNumber: invoice.Code,
                            SupplierId: invoice.DealerId,
                            InvoiceDate: invoice.Date,
                            DueDate: invoice.Date,
                            CurrencyId: invoice.CurrencyId,
                            Rate: invoice.Rate,
                            Amount: invoice.Net,
                            CreateUserId: invoice.CreateUserId,
                            CreateDate: invoice.CreateDate,
                            BranchId: invoice.BranchId),
                        cancellationToken);

                if (invoice.HasJournal)
                {
                    var taxSnapshot = await sender.Send(
                        new SnapshotInvoiceTaxCommand(
                            invoice.Id,
                            invoice.Code,
                            invoice.TypeId,
                            invoice.TaxType,
                            invoice.Tax,
                            invoice.DiscountType,
                            invoice.Discount,
                            invoice.Total,
                            invoice.Net,
                            invoice.CurrencyId,
                            invoice.InvoiceProducts?
                                .Select(p => new InvoiceTaxLineInput(
                                    p.ProductId, p.Quantity, p.Price, p.Tax, p.Net, p.Total))
                                .ToList() ?? [],
                            DateTime.UtcNow),
                        cancellationToken);
                    if (taxSnapshot.StatusCode != HttpStatusCode.OK)
                        throw new InvalidOperationException(taxSnapshot.Errors?[0].ToString() ?? "Tax snapshot failed.");
                }

                await _UnitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                await _UnitOfWork.RollbackAsync();
                return new Result(HttpStatusCode.InternalServerError, [new Error(ex.Message)]);
            }

            if (autoCreateTransaction == "1")
                return await sender.Send(new CreateTransactionByInvoiceCommand(invoice.Id), cancellationToken);

            return new Result(HttpStatusCode.OK, null);
        }
    }
}

namespace Application.Commands.Org.Invoices.Invoice.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Net;
    using Application.Commands.Org.Financials.Integration.JournalInvoice;
    using MediatR;

    public sealed class CreateInvoiceCommand : Application.DTOs.InvoiceDto, ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.Invoice> _Repository,
        IRepository<Domain.Entities.Preference> preferenceRepository, IMapper mapper, IServiceProvider provider,
        ISender sender) : CreateCommandHandler<CreateInvoiceCommand, Domain.Entities.Invoice>(_UnitOfWork, _Repository , mapper)
    {
        public override async Task<Result> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
        {
            var autoCreateTransaction = await preferenceRepository.GetByFilterAsync(
                e => e.Reference == "Invoice"
                    && e.TypeId == request.TypeId
                    && e.Key == "AutoCreateTransaction",
                "");

            Domain.Entities.Invoice invoice;
            await _UnitOfWork.BeginTransactionAsync();
            try
            {
                invoice = mapper.Map<Domain.Entities.Invoice>(request);
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

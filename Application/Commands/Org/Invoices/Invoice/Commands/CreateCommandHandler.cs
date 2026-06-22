namespace Application.Commands.Org.Invoices.Invoice.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;

    public sealed class CreateInvoiceCommand : Application.DTOs.InvoiceModelView, ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.Invoice> _Repository , IMapper mapper) : CreateCommandHandler<CreateInvoiceCommand, Domain.Entities.Invoice>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}
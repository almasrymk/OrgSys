namespace Application.Commands.Org.Invoices.Invoice.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;

    public sealed class CreateInvoiceCommand : Entity.ModelView.InvoiceModelView, ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Invoice> _Repository , IMapper mapper) : CreateCommandHandler<CreateInvoiceCommand, Entity.Model.Invoice>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}
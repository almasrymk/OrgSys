namespace Application.Commands.Org.Invoices.Invoice.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;

    public sealed class UpdateInvoiceCommand : Entity.ModelView.InvoiceModelView , ICommand, IUpdateCommand<Result>;
    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Invoice> _Repository , IMapper mapper) : UpdateCommandHandler<UpdateInvoiceCommand, Entity.Model.Invoice>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}
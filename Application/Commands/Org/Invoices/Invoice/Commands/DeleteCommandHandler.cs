namespace Application.Commands.Org.Invoices.Invoice.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;
    using System.Linq.Expressions;

    public sealed record DeleteInvoiceCommand(long Id) : ICommand, IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Invoice> _Repository, IMapper mapper) : DeleteCommandHandler<DeleteInvoiceCommand, Entity.Model.Invoice>(_UnitOfWork, _Repository, mapper)
    {
        public override Expression<Func<Entity.Model.Invoice, bool>> CreateFilter(DeleteInvoiceCommand request)
        {
            return e => e.Id == request.Id && e.Status != Utility.Status.Deleted && e.Hide != true;
        }
    }
}
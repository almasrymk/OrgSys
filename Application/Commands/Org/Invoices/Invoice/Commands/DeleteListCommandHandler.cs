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

    public sealed record DeleteListInvoiceCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Invoice> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteListInvoiceCommand, Entity.Model.Invoice>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Entity.Model.Invoice, bool>> CreateFilter(DeleteListInvoiceCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != Utility.Status.Deleted && e.Hide != true;
        }
    }
}
namespace Application.Commands.Org.Setting.InvoiceType.Commands
{
    using Application.Abstraction.Command;    
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;
    using System.Linq.Expressions;

    public sealed record DeleteInvoiceTypeCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.InvoiceType> _Repository , IMapper mapper) : DeleteCommandHandler<DeleteInvoiceTypeCommand, Entity.Model.InvoiceType>(_UnitOfWork, _Repository , mapper)
    {
        public override Expression<Func<Entity.Model.InvoiceType, bool>> CreateFilter(DeleteInvoiceTypeCommand request)
        {
            return e => e.Id == request.Id && e.Status != Utility.Status.Deleted && e.Hide != true;
        }
    }
}
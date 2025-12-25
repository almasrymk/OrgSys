namespace Application.Commands.Org.Setting.Stock.Commands
{
    using Application.Abstraction.Command;    
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;
    using System.Linq.Expressions;

    public sealed record DeleteStockCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Stock> _Repository , IMapper mapper) : DeleteCommandHandler<DeleteStockCommand, Entity.Model.Stock>(_UnitOfWork, _Repository , mapper)
    {
        public override Expression<Func<Entity.Model.Stock, bool>> CreateFilter(DeleteStockCommand request)
        {
            return e => e.Id == request.Id && e.Status != Utility.Status.Deleted && e.Hide != true;
        }
    }
}
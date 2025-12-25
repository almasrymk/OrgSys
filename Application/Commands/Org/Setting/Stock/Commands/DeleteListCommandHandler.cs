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

    public sealed record DeleteListStockCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Stock> _Repository , IMapper mapper) : DeleteCommandHandler<DeleteListStockCommand, Entity.Model.Stock>(_UnitOfWork, _Repository , mapper)
    {
        public override Expression<Func<Entity.Model.Stock, bool>> CreateFilter(DeleteListStockCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != Utility.Status.Deleted && e.Hide != true;
        }
    }
}
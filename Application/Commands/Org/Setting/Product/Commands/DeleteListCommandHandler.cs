namespace Application.Commands.Org.Setting.Product.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;
    using System.Linq.Expressions;

    public sealed record DeleteListProductCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Product> _Repository , IMapper mapper) : DeleteCommandHandler<DeleteListProductCommand, Entity.Model.Product>(_UnitOfWork, _Repository , mapper)
    {
        public override Expression<Func<Entity.Model.Product, bool>> CreateFilter(DeleteListProductCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != Utility.Status.Deleted && e.Hide != true;
        }
    }
}
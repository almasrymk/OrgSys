namespace Application.Commands.Org.Setting.Property.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;
    using System.Linq.Expressions;

    public sealed record DeleteListPropertyCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Property> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteListPropertyCommand, Entity.Model.Property>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Entity.Model.Property, bool>> CreateFilter(DeleteListPropertyCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != Utility.Status.Deleted && e.Hide != true;
        }
    }
}
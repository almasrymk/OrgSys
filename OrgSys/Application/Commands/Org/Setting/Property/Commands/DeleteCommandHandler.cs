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

    public sealed record DeletePropertyCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Property> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeletePropertyCommand, Entity.Model.Property>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Entity.Model.Property, bool>> CreateFilter(DeletePropertyCommand request)
        {
            return e => e.Id == request.Id && e.Status != Utility.Status.Deleted && e.Hide != true;
        }
    }
}
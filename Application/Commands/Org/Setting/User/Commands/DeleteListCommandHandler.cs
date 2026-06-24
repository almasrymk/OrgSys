namespace Application.Commands.Org.Setting.User.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;

    public sealed record DeleteListUserCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.User> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteListUserCommand, Domain.Entities.User>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Domain.Entities.User, bool>> CreateFilter(DeleteListUserCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
    }
}
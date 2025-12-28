namespace Application.Commands.Org.Setting.Safe.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;
    using System.Linq.Expressions;

    public sealed record DeleteListSafeCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Safe> _Repository , IMapper mapper) : DeleteCommandHandler<DeleteListSafeCommand, Entity.Model.Safe>(_UnitOfWork, _Repository , mapper)
    {
        public override Expression<Func<Entity.Model.Safe, bool>> CreateFilter(DeleteListSafeCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != Utility.Status.Deleted && e.Hide != true;
        }
    }
}
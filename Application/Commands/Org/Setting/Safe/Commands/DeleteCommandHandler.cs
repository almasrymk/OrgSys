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

    public sealed record DeleteSafeCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Safe> _Repository , IMapper mapper) : DeleteCommandHandler<DeleteSafeCommand, Entity.Model.Safe>(_UnitOfWork, _Repository , mapper)
    {
        public override Expression<Func<Entity.Model.Safe, bool>> CreateFilter(DeleteSafeCommand request)
        {
            return e => e.Id == request.Id && e.Status != Utility.Status.Deleted && e.Hide != true;
        }
    }
}
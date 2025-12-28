namespace Application.Commands.Org.Setting.Unit.Commands
{
    using Application.Abstraction.Command;    
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;
    using System.Linq.Expressions;

    public sealed record DeleteUnitCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Unit> _Repository , IMapper mapper) : DeleteCommandHandler<DeleteUnitCommand, Entity.Model.Unit>(_UnitOfWork, _Repository , mapper)
    {
        public override Expression<Func<Entity.Model.Unit, bool>> CreateFilter(DeleteUnitCommand request)
        {
            return e => e.Id == request.Id && e.Status != Utility.Status.Deleted && e.Hide != true;
        }
    }
}
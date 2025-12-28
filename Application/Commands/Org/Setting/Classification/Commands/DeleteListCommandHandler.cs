namespace Application.Commands.Org.Setting.Classification.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;
    using System.Linq.Expressions;

    public sealed record DeleteListClassificationCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Classification> _Repository , IMapper mapper) : DeleteCommandHandler<DeleteListClassificationCommand, Entity.Model.Classification>(_UnitOfWork, _Repository , mapper)
    {
        public override Expression<Func<Entity.Model.Classification, bool>> CreateFilter(DeleteListClassificationCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != Utility.Status.Deleted && e.Hide != true;
        }
    }
}
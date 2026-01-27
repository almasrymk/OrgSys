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

    public sealed record DeleteClassificationCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Classification> _Repository , IMapper mapper) : DeleteCommandHandler<DeleteClassificationCommand, Entity.Model.Classification>(_UnitOfWork, _Repository , mapper)
    {
        public override Expression<Func<Entity.Model.Classification, bool>> CreateFilter(DeleteClassificationCommand request)
        {
            return e => e.Id == request.Id && e.Status != Utility.Status.Deleted && e.Hide != true;
        }
    }
}
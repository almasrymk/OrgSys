namespace Application.Commands.Org.Setting.Outlay.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;
    using System.Linq.Expressions;

    public sealed record DeleteOutlayCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Outlay> _Repository , IMapper mapper) : DeleteCommandHandler<DeleteOutlayCommand, Entity.Model.Outlay>(_UnitOfWork, _Repository , mapper)
    {
        public override Expression<Func<Entity.Model.Outlay, bool>> CreateFilter(DeleteOutlayCommand request)
        {
            return e => e.Id == request.Id && e.Status != Utility.Status.Deleted && e.Hide != true;
        }
    }
}
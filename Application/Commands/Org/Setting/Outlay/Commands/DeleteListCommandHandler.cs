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

    public sealed record DeleteListOutlayCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Outlay> _Repository , IMapper mapper) : DeleteCommandHandler<DeleteListOutlayCommand, Entity.Model.Outlay>(_UnitOfWork, _Repository , mapper)
    {
        public override Expression<Func<Entity.Model.Outlay, bool>> CreateFilter(DeleteListOutlayCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != Utility.Status.Deleted && e.Hide != true;
        }
    }
}
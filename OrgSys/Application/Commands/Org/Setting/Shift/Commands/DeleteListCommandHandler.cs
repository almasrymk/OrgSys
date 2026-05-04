namespace Application.Commands.Org.Setting.Shift.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;
    using System.Linq.Expressions;

    public sealed record DeleteListShiftCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Shift> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteListShiftCommand, Entity.Model.Shift>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Entity.Model.Shift, bool>> CreateFilter(DeleteListShiftCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != Utility.Status.Deleted && e.Hide != true;
        }
    }
}
namespace Application.Commands.Org.Setting.District.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;
    using System.Linq.Expressions;

    public sealed record DeleteListDistrictCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.District> _Repository , IMapper mapper) : DeleteCommandHandler<DeleteListDistrictCommand, Entity.Model.District>(_UnitOfWork, _Repository , mapper)
    {
        public override Expression<Func<Entity.Model.District, bool>> CreateFilter(DeleteListDistrictCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != Utility.Status.Deleted && e.Hide != true;
        }
    }
}
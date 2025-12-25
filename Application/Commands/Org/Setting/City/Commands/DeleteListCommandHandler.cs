namespace Application.Commands.Org.Setting.City.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;
    using System.Linq.Expressions;

    public sealed record DeleteListCityCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.City> _Repository , IMapper mapper) : DeleteCommandHandler<DeleteListCityCommand, Entity.Model.City>(_UnitOfWork, _Repository , mapper)
    {
        public override Expression<Func<Entity.Model.City, bool>> CreateFilter(DeleteListCityCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != Utility.Status.Deleted && e.Hide != true;
        }
    }
}
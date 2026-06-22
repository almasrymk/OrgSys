namespace Application.Commands.Org.Setting.City.Commands
{
    using AutoMapper;
    using Domain.Shared;
    using Domain.Abstraction;
    using System.Linq.Expressions;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using Application.Abstraction.Command;

    public sealed record DeleteListCityCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.City> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteListCityCommand, Domain.Entities.City>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Domain.Entities.City, bool>> CreateFilter(DeleteListCityCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
    }
}
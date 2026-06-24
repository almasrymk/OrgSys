namespace Application.Commands.Org.Setting.City.Commands
{
    using AutoMapper;
    using Domain.Shared;
    using Domain.Abstraction;
    using System.Linq.Expressions;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using Application.Abstraction.Command;

    public sealed record DeleteCityCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.City> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteCityCommand, Domain.Entities.City>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Domain.Entities.City, bool>> CreateFilter(DeleteCityCommand request)
        {
            return e => e.Id == request.Id && e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
    }
}
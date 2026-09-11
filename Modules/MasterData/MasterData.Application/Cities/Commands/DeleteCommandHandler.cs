namespace MasterData.Application.Cities.Commands
{
    using AutoMapper;
    using System.Linq.Expressions;
    using OrgSys.SharedKernel;

    public sealed record DeleteCityCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<MasterData.Domain.City> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteCityCommand, MasterData.Domain.City>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<MasterData.Domain.City, bool>> CreateFilter(DeleteCityCommand request)
        {
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}

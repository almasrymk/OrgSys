namespace MasterData.Application.Cities.Commands
{
    using AutoMapper;
    using System.Linq.Expressions;
    using OrgSys.SharedKernel;

    public sealed record DeleteListCityCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<MasterData.Domain.City> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteListCityCommand, MasterData.Domain.City>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<MasterData.Domain.City, bool>> CreateFilter(DeleteListCityCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}

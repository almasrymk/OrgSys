namespace MasterData.Application.Districts.Commands
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record DeleteListDistrictCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<MasterData.Domain.District> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteListDistrictCommand, MasterData.Domain.District>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<MasterData.Domain.District, bool>> CreateFilter(DeleteListDistrictCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}

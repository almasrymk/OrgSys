namespace MasterData.Application.Districts.Commands
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record DeleteDistrictCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<MasterData.Domain.District> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteDistrictCommand, MasterData.Domain.District>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<MasterData.Domain.District, bool>> CreateFilter(DeleteDistrictCommand request)
        {
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}

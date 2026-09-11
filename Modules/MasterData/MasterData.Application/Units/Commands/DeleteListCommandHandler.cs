namespace MasterData.Application.Units.Commands
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record DeleteListUnitCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<MasterData.Domain.Unit> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteListUnitCommand, MasterData.Domain.Unit>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<MasterData.Domain.Unit, bool>> CreateFilter(DeleteListUnitCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}

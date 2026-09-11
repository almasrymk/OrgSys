namespace MasterData.Application.Units.Commands
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record DeleteUnitCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<MasterData.Domain.Unit> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteUnitCommand, MasterData.Domain.Unit>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<MasterData.Domain.Unit, bool>> CreateFilter(DeleteUnitCommand request)
        {
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}

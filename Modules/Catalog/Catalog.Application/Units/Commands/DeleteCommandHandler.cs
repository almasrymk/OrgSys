namespace Catalog.Application.Units.Commands
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record DeleteUnitCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Catalog.Domain.Unit> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteUnitCommand, Catalog.Domain.Unit>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Catalog.Domain.Unit, bool>> CreateFilter(DeleteUnitCommand request)
        {
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}

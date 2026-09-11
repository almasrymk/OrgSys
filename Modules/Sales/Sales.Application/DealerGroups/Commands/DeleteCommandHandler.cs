namespace Sales.Application.DealerGroups.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record DeleteDealerGroupCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Sales.Domain.DealerGroup> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteDealerGroupCommand, Sales.Domain.DealerGroup>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Sales.Domain.DealerGroup, bool>> CreateFilter(DeleteDealerGroupCommand request)
        {
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}
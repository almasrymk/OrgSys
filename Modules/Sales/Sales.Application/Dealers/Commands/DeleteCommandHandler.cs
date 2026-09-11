namespace Sales.Application.Dealers.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record DeleteDealerCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Sales.Domain.Dealer> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteDealerCommand, Sales.Domain.Dealer>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Sales.Domain.Dealer, bool>> CreateFilter(DeleteDealerCommand request)
        {
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}
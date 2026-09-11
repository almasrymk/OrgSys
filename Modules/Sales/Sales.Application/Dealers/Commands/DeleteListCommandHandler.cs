namespace Sales.Application.Dealers.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record DeleteListDealerCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Sales.Domain.Dealer> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteListDealerCommand, Sales.Domain.Dealer>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Sales.Domain.Dealer, bool>> CreateFilter(DeleteListDealerCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}
namespace Catalog.Application.PriceLists.Commands
{
    using OrgSys.SharedKernel;
    using System.Linq.Expressions;

    public sealed record DeleteListPriceListCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Catalog.Domain.PriceList> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteListPriceListCommand, Catalog.Domain.PriceList>(_UnitOfWork, _Repository, _provider)
    {
        public override Expression<Func<Catalog.Domain.PriceList, bool>> CreateFilter(DeleteListPriceListCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}

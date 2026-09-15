namespace Catalog.Application.PriceLists.Commands
{
    using OrgSys.SharedKernel;
    using System.Linq.Expressions;

    public sealed record DeletePriceListCommand(long Id) : ICommand, IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Catalog.Domain.PriceList> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeletePriceListCommand, Catalog.Domain.PriceList>(_UnitOfWork, _Repository, _provider)
    {
        public override Expression<Func<Catalog.Domain.PriceList, bool>> CreateFilter(DeletePriceListCommand request)
        {
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}

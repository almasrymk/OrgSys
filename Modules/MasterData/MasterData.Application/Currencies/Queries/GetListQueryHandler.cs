namespace MasterData.Application.Currencies.Queries
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetListCurrencyQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<CurrencyDto> , IListQuery<ResultCollection<CurrencyDto>>;

    public sealed class GetListQueryHandler(IRepository<MasterData.Domain.Currency> _Repository, IMapper mapper) : ListCommandHandler<GetListCurrencyQuery, MasterData.Domain.Currency, CurrencyDto>(_Repository, mapper)
    {
        public override Expression<Func<MasterData.Domain.Currency, bool>> CreateFilter(GetListCurrencyQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<MasterData.Domain.Currency>, IOrderedQueryable<MasterData.Domain.Currency>> CreateOrderBy(GetListCurrencyQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}

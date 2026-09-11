namespace Sales.Application.Dealers.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetListDealerQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<DealerDto> , IListQuery<ResultCollection<DealerDto>>;

    public sealed class GetListQueryHandler(IRepository<Sales.Domain.Dealer> _Repository, IMapper mapper) : ListCommandHandler<GetListDealerQuery, Sales.Domain.Dealer, DealerDto>(_Repository, mapper)
    {
        public override Expression<Func<Sales.Domain.Dealer, bool>> CreateFilter(GetListDealerQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.TypeId == request.TypeId &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Sales.Domain.Dealer>, IOrderedQueryable<Sales.Domain.Dealer>> CreateOrderBy(GetListDealerQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }

        public override string CreateInclude()
        {
            // See SearchQueryHandler.CreateInclude for why Country/City/District were added.
            return "DealerGroup,Account,Country,City,District";
        }
    }
}
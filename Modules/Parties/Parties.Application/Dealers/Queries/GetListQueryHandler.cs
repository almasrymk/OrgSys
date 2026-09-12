namespace Parties.Application.Dealers.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetListDealerQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<DealerDto> , IListQuery<ResultCollection<DealerDto>>;

    public sealed class GetListQueryHandler(IRepository<Parties.Domain.Dealer> _Repository, IMapper mapper) : ListCommandHandler<GetListDealerQuery, Parties.Domain.Dealer, DealerDto>(_Repository, mapper)
    {
        public override Expression<Func<Parties.Domain.Dealer, bool>> CreateFilter(GetListDealerQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.TypeId == request.TypeId &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Parties.Domain.Dealer>, IOrderedQueryable<Parties.Domain.Dealer>> CreateOrderBy(GetListDealerQuery request)
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
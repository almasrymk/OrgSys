namespace Parties.Application.Dealers.Queries
{
    using Accounting.Contracts.Accounts;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using MediatR;
    using System.Linq.Expressions;

    public sealed record SearchDealerQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<DealerDto> ,ISearchQuery<ResultPagination<DealerDto>>;

    public sealed class SearchQueryHandler(
        IRepository<Parties.Domain.Dealer> _Repository,
        ISender sender,
        IMapper mapper) : SearchCommandHandler<SearchDealerQuery, Parties.Domain.Dealer, DealerDto>(_Repository, mapper)
    {
        public override Expression<Func<Parties.Domain.Dealer, bool>> CreateFilter(SearchDealerQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.TypeId == request.TypeId &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        override public Func<IQueryable<Parties.Domain.Dealer>, IOrderedQueryable<Parties.Domain.Dealer>> CreateOrderBy(SearchDealerQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }

        public override string CreateInclude()
        {
            // Country/City/District added — DealerDto exposes CountryName/CityName/DistrictName but
            // this handler wasn't loading those navigations, so they always came back null even
            // though CountryId/CityId/DistrictId themselves save and read back correctly. Account
            // was removed from this list (see the GeneralLedger migration report) —
            // AccountCode/AccountName are patched in below instead.
            return "DealerGroup,Country,City,District";
        }

        public override async Task<ResultPagination<DealerDto>> Handle(SearchDealerQuery request, CancellationToken cancellationToken)
        {
            var result = await base.Handle(request, cancellationToken);

            var accountIds = result.Response.Where(e => e.AccountId is > 0).Select(e => e.AccountId!.Value).Distinct().ToList();
            if (accountIds.Count > 0)
            {
                var accounts = (await sender.Send(new GetAccountLookupsQuery(accountIds), cancellationToken)).Response ?? [];
                foreach (var dto in result.Response)
                    if (dto.AccountId is > 0 && accounts.TryGetValue(dto.AccountId.Value, out var account))
                    {
                        dto.AccountCode = account.Code;
                        dto.AccountName = account.Name;
                    }
            }

            return result;
        }
    }
}

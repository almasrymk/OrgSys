namespace Parties.Application.Dealers.Queries
{
    using Accounting.Contracts.Accounts;
    using MasterData.Contracts.Lookups;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using MediatR;
    using System.Linq.Expressions;

    public sealed record GetListDealerQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<DealerDto> , IListQuery<ResultCollection<DealerDto>>;

    public sealed class GetListQueryHandler(
        IRepository<Parties.Domain.Dealer> _Repository,
        ISender sender,
        IMapper mapper) : ListCommandHandler<GetListDealerQuery, Parties.Domain.Dealer, DealerDto>(_Repository, mapper)
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
            // Account was removed from this list (see the GeneralLedger migration report) —
            // AccountCode/AccountName are patched in below instead.
            return "DealerGroup";
        }

        public override async Task<ResultCollection<DealerDto>> Handle(GetListDealerQuery request, CancellationToken cancellationToken)
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

            var countryIds = result.Response.Where(e => e.CountryId is > 0).Select(e => e.CountryId!.Value).Distinct().ToList();
            if (countryIds.Count > 0)
            {
                var names = (await sender.Send(new GetCountryNamesQuery(countryIds), cancellationToken)).Response ?? [];
                foreach (var dto in result.Response)
                    if (dto.CountryId is > 0)
                        dto.CountryName = names.GetValueOrDefault(dto.CountryId.Value);
            }

            var cityIds = result.Response.Where(e => e.CityId is > 0).Select(e => e.CityId!.Value).Distinct().ToList();
            if (cityIds.Count > 0)
            {
                var names = (await sender.Send(new GetCityNamesQuery(cityIds), cancellationToken)).Response ?? [];
                foreach (var dto in result.Response)
                    if (dto.CityId is > 0)
                        dto.CityName = names.GetValueOrDefault(dto.CityId.Value);
            }

            var districtIds = result.Response.Where(e => e.DistrictId is > 0).Select(e => e.DistrictId!.Value).Distinct().ToList();
            if (districtIds.Count > 0)
            {
                var names = (await sender.Send(new GetDistrictNamesQuery(districtIds), cancellationToken)).Response ?? [];
                foreach (var dto in result.Response)
                    if (dto.DistrictId is > 0)
                        dto.DistrictName = names.GetValueOrDefault(dto.DistrictId.Value);
            }

            return result;
        }
    }
}

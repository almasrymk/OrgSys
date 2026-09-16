namespace Parties.Application.PartyAddresses.Queries
{
    using MasterData.Contracts.Lookups;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using MediatR;
    using System.Linq.Expressions;

    public sealed record GetListByDealerPartyAddressQuery(long DealerId, string KeySearch, long ParentId, long TypeId, int Page, int PageSize) : ICommandCollection<PartyAddressDto>, IListQuery<ResultCollection<PartyAddressDto>>;

    public sealed class GetListByDealerQueryHandler(IRepository<Parties.Domain.PartyAddress> _Repository, ISender sender, IMapper mapper) : ListCommandHandler<GetListByDealerPartyAddressQuery, Parties.Domain.PartyAddress, PartyAddressDto>(_Repository, mapper)
    {
        public override Expression<Func<Parties.Domain.PartyAddress, bool>> CreateFilter(GetListByDealerPartyAddressQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            e.DealerId == request.DealerId &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        override public Func<IQueryable<Parties.Domain.PartyAddress>, IOrderedQueryable<Parties.Domain.PartyAddress>> CreateOrderBy(GetListByDealerPartyAddressQuery request)
        {
            return q => q.OrderByDescending(e => e.IsPrimary).ThenBy(e => e.AddressType);
        }

        public override async Task<ResultCollection<PartyAddressDto>> Handle(GetListByDealerPartyAddressQuery request, CancellationToken cancellationToken)
        {
            var result = await base.Handle(request, cancellationToken);

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

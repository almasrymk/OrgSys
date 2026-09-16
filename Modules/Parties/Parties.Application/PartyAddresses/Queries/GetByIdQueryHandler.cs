namespace Parties.Application.PartyAddresses.Queries
{
    using MasterData.Contracts.Lookups;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using MediatR;
    using System.Linq.Expressions;

    public sealed record GetByIdPartyAddressQuery(long Id) : ICommand<PartyAddressDto>, IGetByIdQuery<Result<PartyAddressDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Parties.Domain.PartyAddress> _Repository, ISender sender, IMapper mapper) : GetCommandHandler<GetByIdPartyAddressQuery, Parties.Domain.PartyAddress, PartyAddressDto>(_Repository, mapper)
    {
        public override Expression<Func<Parties.Domain.PartyAddress, bool>> CreateFilter(GetByIdPartyAddressQuery request)
        {
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        public override async Task<Result<PartyAddressDto>> Handle(GetByIdPartyAddressQuery request, CancellationToken cancellationToken)
        {
            var result = await base.Handle(request, cancellationToken);
            if (result.Response is null)
                return result;

            if (result.Response.CountryId is > 0)
            {
                var names = (await sender.Send(new GetCountryNamesQuery([result.Response.CountryId.Value]), cancellationToken)).Response ?? [];
                result.Response.CountryName = names.GetValueOrDefault(result.Response.CountryId.Value);
            }

            if (result.Response.CityId is > 0)
            {
                var names = (await sender.Send(new GetCityNamesQuery([result.Response.CityId.Value]), cancellationToken)).Response ?? [];
                result.Response.CityName = names.GetValueOrDefault(result.Response.CityId.Value);
            }

            if (result.Response.DistrictId is > 0)
            {
                var names = (await sender.Send(new GetDistrictNamesQuery([result.Response.DistrictId.Value]), cancellationToken)).Response ?? [];
                result.Response.DistrictName = names.GetValueOrDefault(result.Response.DistrictId.Value);
            }

            return result;
        }
    }
}

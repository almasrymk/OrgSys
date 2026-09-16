namespace Parties.Application.Dealers.Queries
{
    using Accounting.Contracts.Accounts;
    using MasterData.Contracts.Lookups;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using MediatR;
    using System.Linq.Expressions;

    public sealed record GetByIdDealerQuery(long Id) : ICommand<DealerDto> , IGetByIdQuery<Result<DealerDto>>;

    public sealed class GetByIdQueryHandler(
        IRepository<Parties.Domain.Dealer> _Repository,
        ISender sender,
        IMapper mapper) : GetCommandHandler<GetByIdDealerQuery, Parties.Domain.Dealer, DealerDto>(_Repository, mapper)
    {
        public override Expression<Func<Parties.Domain.Dealer, bool>> CreateFilter(GetByIdDealerQuery request)
        {
            return e => e.Id == request.Id && e.Status !=OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        // See SearchQueryHandler.CreateInclude — this handler previously had no override at all, so
        // DealerGroupName/AccountName/CountryName/CityName/DistrictName all came back null on a
        // single-record fetch too. Account was removed from this list (see the GeneralLedger
        // migration report) — AccountCode/AccountName are patched in below instead.
        public override string CreateInclude()
        {
            return "DealerGroup";
        }

        public override async Task<Result<DealerDto>> Handle(GetByIdDealerQuery request, CancellationToken cancellationToken)
        {
            var result = await base.Handle(request, cancellationToken);

            if (result.Response is null)
                return result;

            if (result.Response.AccountId is > 0)
            {
                var account = (await sender.Send(new GetAccountQuery(result.Response.AccountId.Value), cancellationToken)).Response;
                result.Response.AccountCode = account?.Code;
                result.Response.AccountName = account?.Name;
            }

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

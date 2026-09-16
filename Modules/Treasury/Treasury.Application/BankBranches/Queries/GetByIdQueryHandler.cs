namespace Treasury.Application.BankBranches.Queries
{
    using MasterData.Contracts.Lookups;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using MediatR;
    using System.Linq.Expressions;

    public sealed record GetByIdBankBranchQuery(long Id) : ICommand<BankBranchDto> , IGetByIdQuery<Result<BankBranchDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Treasury.Domain.BankBranch> _Repository, ISender sender, IMapper mapper) : GetCommandHandler<GetByIdBankBranchQuery, Treasury.Domain.BankBranch, BankBranchDto>(_Repository, mapper)
    {
        public override Expression<Func<Treasury.Domain.BankBranch, bool>> CreateFilter(GetByIdBankBranchQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        public override async Task<Result<BankBranchDto>> Handle(GetByIdBankBranchQuery request, CancellationToken cancellationToken)
        {
            var result = await base.Handle(request, cancellationToken);
            if (result.Response is null)
                return result;

            var countries = (await sender.Send(new GetCountryNamesQuery([result.Response.CountryId]), cancellationToken)).Response ?? [];
            result.Response.CountryName = countries.GetValueOrDefault(result.Response.CountryId);

            var cities = (await sender.Send(new GetCityNamesQuery([result.Response.CityId]), cancellationToken)).Response ?? [];
            result.Response.CityName = cities.GetValueOrDefault(result.Response.CityId);

            var districts = (await sender.Send(new GetDistrictNamesQuery([result.Response.DistrictId]), cancellationToken)).Response ?? [];
            result.Response.DistrictName = districts.GetValueOrDefault(result.Response.DistrictId);

            return result;
        }
    }
}
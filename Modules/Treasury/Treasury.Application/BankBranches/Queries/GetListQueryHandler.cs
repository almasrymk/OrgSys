namespace Treasury.Application.BankBranches.Queries
{
    using MasterData.Contracts.Lookups;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using MediatR;
    using System.Linq.Expressions;

    public sealed record GetListBankBranchQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<BankBranchDto> , IListQuery<ResultCollection<BankBranchDto>>;

    public sealed class GetListQueryHandler(IRepository<Treasury.Domain.BankBranch> _Repository, ISender sender, IMapper mapper) : ListCommandHandler<GetListBankBranchQuery, Treasury.Domain.BankBranch, BankBranchDto>(_Repository, mapper)
    {
        public override Expression<Func<Treasury.Domain.BankBranch, bool>> CreateFilter(GetListBankBranchQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (request.ParentId == 0 || e.BankId == request.ParentId) &&
            (string.IsNullOrEmpty(request.KeySearch) || e.Name!.Contains(request.KeySearch)) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        override public Func<IQueryable<Treasury.Domain.BankBranch>, IOrderedQueryable<Treasury.Domain.BankBranch>> CreateOrderBy(GetListBankBranchQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }

        public override string CreateInclude()
        {
            return "Bank";
        }

        public override async Task<ResultCollection<BankBranchDto>> Handle(GetListBankBranchQuery request, CancellationToken cancellationToken)
        {
            var result = await base.Handle(request, cancellationToken);

            var countryIds = result.Response.Select(e => e.CountryId).Distinct().ToList();
            if (countryIds.Count > 0)
            {
                var names = (await sender.Send(new GetCountryNamesQuery(countryIds), cancellationToken)).Response ?? [];
                foreach (var dto in result.Response)
                    dto.CountryName = names.GetValueOrDefault(dto.CountryId);
            }

            var cityIds = result.Response.Select(e => e.CityId).Distinct().ToList();
            if (cityIds.Count > 0)
            {
                var names = (await sender.Send(new GetCityNamesQuery(cityIds), cancellationToken)).Response ?? [];
                foreach (var dto in result.Response)
                    dto.CityName = names.GetValueOrDefault(dto.CityId);
            }

            var districtIds = result.Response.Select(e => e.DistrictId).Distinct().ToList();
            if (districtIds.Count > 0)
            {
                var names = (await sender.Send(new GetDistrictNamesQuery(districtIds), cancellationToken)).Response ?? [];
                foreach (var dto in result.Response)
                    dto.DistrictName = names.GetValueOrDefault(dto.DistrictId);
            }

            return result;
        }
    }
}

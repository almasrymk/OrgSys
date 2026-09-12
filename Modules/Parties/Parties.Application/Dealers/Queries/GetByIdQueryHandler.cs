namespace Parties.Application.Dealers.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetByIdDealerQuery(long Id) : ICommand<DealerDto> , IGetByIdQuery<Result<DealerDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Parties.Domain.Dealer> _Repository, IMapper mapper) : GetCommandHandler<GetByIdDealerQuery, Parties.Domain.Dealer, DealerDto>(_Repository, mapper)
    {
        public override Expression<Func<Parties.Domain.Dealer, bool>> CreateFilter(GetByIdDealerQuery request)
        {
            return e => e.Id == request.Id && e.Status !=OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        // See SearchQueryHandler.CreateInclude — this handler previously had no override at all, so
        // DealerGroupName/AccountName/CountryName/CityName/DistrictName all came back null on a
        // single-record fetch too.
        public override string CreateInclude()
        {
            return "DealerGroup,Account,Country,City,District";
        }
    }
}
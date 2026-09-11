namespace MasterData.Application.Countries.Queries
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetByIdCountryQuery(long Id) : ICommand<CountryDto> , IGetByIdQuery<Result<CountryDto>>;

    public sealed class GetByIdQueryHandler(IRepository<MasterData.Domain.Country> _Repository, IMapper mapper) : GetCommandHandler<GetByIdCountryQuery, MasterData.Domain.Country, CountryDto>(_Repository, mapper)
    {
        public override Expression<Func<MasterData.Domain.Country, bool>> CreateFilter(GetByIdCountryQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}

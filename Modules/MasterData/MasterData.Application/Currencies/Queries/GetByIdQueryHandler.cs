namespace MasterData.Application.Currencies.Queries
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetByIdCurrencyQuery(long Id) : ICommand<CurrencyDto> , IGetByIdQuery<Result<CurrencyDto>>;

    public sealed class GetByIdQueryHandler(IRepository<MasterData.Domain.Currency> _Repository, IMapper mapper) : GetCommandHandler<GetByIdCurrencyQuery, MasterData.Domain.Currency, CurrencyDto>(_Repository, mapper)
    {
        public override Expression<Func<MasterData.Domain.Currency, bool>> CreateFilter(GetByIdCurrencyQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}

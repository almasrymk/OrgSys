namespace Treasury.Application.FinancialTypes.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetByIdFinancialTypeQuery(long Id) : ICommand<FinancialTypeDto> , IGetByIdQuery<Result<FinancialTypeDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Treasury.Domain.FinancialType> _Repository, IMapper mapper) : 
        GetCommandHandler<GetByIdFinancialTypeQuery, Treasury.Domain.FinancialType, FinancialTypeDto>(_Repository, mapper)
    {
        public override string CreateInclude()
        {
            return "";
        }

        public override Expression<Func<Treasury.Domain.FinancialType, bool>> CreateFilter(GetByIdFinancialTypeQuery request)
        {           
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}
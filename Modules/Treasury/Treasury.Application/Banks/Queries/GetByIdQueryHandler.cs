namespace Treasury.Application.Banks.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetByIdBankQuery(long Id) : ICommand<BankDto> , IGetByIdQuery<Result<BankDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Treasury.Domain.Bank> _Repository, IMapper mapper) : GetCommandHandler<GetByIdBankQuery, Treasury.Domain.Bank, BankDto>(_Repository, mapper)
    {
        public override Expression<Func<Treasury.Domain.Bank, bool>> CreateFilter(GetByIdBankQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}
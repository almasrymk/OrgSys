namespace Accounting.Application.AccountTypes.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetByIdAccountTypeQuery(long Id) : ICommand<AccountTypeDto> , IGetByIdQuery<Result<AccountTypeDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Accounting.Domain.AccountType> _Repository, IMapper mapper) : GetCommandHandler<GetByIdAccountTypeQuery, Accounting.Domain.AccountType, AccountTypeDto>(_Repository, mapper)
    {
        public override Expression<Func<Accounting.Domain.AccountType, bool>> CreateFilter(GetByIdAccountTypeQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}
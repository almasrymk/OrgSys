namespace Accounting.Application.Accounts.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetByIdAccountQuery(long Id) : ICommand<AccountDto> , IGetByIdQuery<Result<AccountDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Accounting.Domain.Account> _Repository, IMapper mapper) : GetCommandHandler<GetByIdAccountQuery, Accounting.Domain.Account, AccountDto>(_Repository, mapper)
    {
        public override Expression<Func<Accounting.Domain.Account, bool>> CreateFilter(GetByIdAccountQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        public override string CreateInclude()
        {
            return "AccountType";
        }
    }
}
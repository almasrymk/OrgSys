namespace Treasury.Application.FinancialAccounts.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetByIdFinancialAccountQuery(long Id) : ICommand<FinancialAccountDto>, IGetByIdQuery<Result<FinancialAccountDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Treasury.Domain.FinancialAccount> _Repository, IMapper mapper) : GetCommandHandler<GetByIdFinancialAccountQuery, Treasury.Domain.FinancialAccount, FinancialAccountDto>(_Repository, mapper)
    {
        public override Expression<Func<Treasury.Domain.FinancialAccount, bool>> CreateFilter(GetByIdFinancialAccountQuery request)
        {
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        public override string CreateInclude()
        {
            return "CashBox,BankAccount.Bank,BankAccount.BankBranch,Account,Currency";
        }
    }
}

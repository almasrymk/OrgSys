namespace Treasury.Application.FinancialAccounts.Queries
{
    using Accounting.Contracts.Accounts;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using MediatR;
    using System.Linq.Expressions;

    public sealed record GetByIdFinancialAccountQuery(long Id) : ICommand<FinancialAccountDto>, IGetByIdQuery<Result<FinancialAccountDto>>;

    public sealed class GetByIdQueryHandler(
        IRepository<Treasury.Domain.FinancialAccount> _Repository,
        ISender sender,
        IMapper mapper) : GetCommandHandler<GetByIdFinancialAccountQuery, Treasury.Domain.FinancialAccount, FinancialAccountDto>(_Repository, mapper)
    {
        public override Expression<Func<Treasury.Domain.FinancialAccount, bool>> CreateFilter(GetByIdFinancialAccountQuery request)
        {
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        // Account was removed from this list (see the GeneralLedger migration report) —
        // AccountCode/AccountName are patched in below instead.
        public override string CreateInclude()
        {
            return "CashBox,BankAccount.Bank,BankAccount.BankBranch,Currency";
        }

        public override async Task<Result<FinancialAccountDto>> Handle(GetByIdFinancialAccountQuery request, CancellationToken cancellationToken)
        {
            var result = await base.Handle(request, cancellationToken);

            if (result.Response?.AccountId is > 0)
            {
                var account = (await sender.Send(new GetAccountQuery(result.Response.AccountId.Value), cancellationToken)).Response;
                result.Response.AccountCode = account?.Code;
                result.Response.AccountName = account?.Name;
            }

            return result;
        }
    }
}

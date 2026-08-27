namespace Application.Commands.Org.Financials.FinancialAccount.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;

    public sealed record GetByIdFinancialAccountQuery(long Id) : ICommand<FinancialAccountDto>, IGetByIdQuery<Result<FinancialAccountDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Domain.Entities.FinancialAccount> _Repository, IMapper mapper) : GetCommandHandler<GetByIdFinancialAccountQuery, Domain.Entities.FinancialAccount, FinancialAccountDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.FinancialAccount, bool>> CreateFilter(GetByIdFinancialAccountQuery request)
        {
            return e => e.Id == request.Id && e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }

        public override string CreateInclude()
        {
            return "CashBox,BankAccount.Bank,BankAccount.BankBranch,Account,Currency";
        }
    }
}

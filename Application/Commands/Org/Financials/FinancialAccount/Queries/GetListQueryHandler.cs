namespace Application.Commands.Org.Financials.FinancialAccount.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Queries;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;

    public sealed record GetListFinancialAccountQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize) : ICommandCollection<FinancialAccountDto>, IListQuery<ResultCollection<FinancialAccountDto>>;

    public sealed class GetListQueryHandler(IRepository<Domain.Entities.FinancialAccount> _Repository, IMapper mapper) : ListCommandHandler<GetListFinancialAccountQuery, Domain.Entities.FinancialAccount, FinancialAccountDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.FinancialAccount, bool>> CreateFilter(GetListFinancialAccountQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (request.TypeId == 0 || (long)e.FinancialAccountType == request.TypeId) &&
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch) || (e.Code != null && e.Code.Contains(request.KeySearch))) &&
            e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }

        public override string CreateInclude()
        {
            return "CashBox,BankAccount.Bank,Account,Currency";
        }

        override public Func<IQueryable<Domain.Entities.FinancialAccount>, IOrderedQueryable<Domain.Entities.FinancialAccount>> CreateOrderBy(GetListFinancialAccountQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}

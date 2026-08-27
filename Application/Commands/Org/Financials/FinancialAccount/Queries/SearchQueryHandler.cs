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

    public sealed record SearchFinancialAccountQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize) : ICommandPagination<FinancialAccountDto>, ISearchQuery<ResultPagination<FinancialAccountDto>>;

    public sealed class SearchQueryHandler(IRepository<Domain.Entities.FinancialAccount> _Repository, IMapper mapper) : SearchCommandHandler<SearchFinancialAccountQuery, Domain.Entities.FinancialAccount, FinancialAccountDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.FinancialAccount, bool>> CreateFilter(SearchFinancialAccountQuery request)
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

        override public Func<IQueryable<Domain.Entities.FinancialAccount>, IOrderedQueryable<Domain.Entities.FinancialAccount>> CreateOrderBy(SearchFinancialAccountQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}

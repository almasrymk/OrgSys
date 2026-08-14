namespace Application.Commands.Org.Financials.Financial.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Queries;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;

    public sealed record SearchFinancialQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<FinancialDto> ,ISearchQuery<ResultPagination<FinancialDto>>;

    public sealed class SearchQueryHandler(IRepository<Domain.Entities.Financial> _Repository, IMapper mapper) : SearchCommandHandler<SearchFinancialQuery, Domain.Entities.Financial, FinancialDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.Financial, bool>> CreateFilter(SearchFinancialQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) ||
             (e.Code != null && e.Code.Contains(request.KeySearch)) ||
             (e.Notes != null && e.Notes.Contains(request.KeySearch)) ||
             (e.FinancialAccount != null && e.FinancialAccount.Name.Contains(request.KeySearch))) &&
            (request.ParentId ==0 || e.ParentId == request.ParentId) &&
            (request.TypeId == 0 || e.FinancialTypeId == request.TypeId) &&
            e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }

        public override string CreateInclude()
        {
            return "Dealer,Safe,Currency,FinancialAccount,FinancialType,Journal";
        }

        override public Func<IQueryable<Domain.Entities.Financial>, IOrderedQueryable<Domain.Entities.Financial>> CreateOrderBy(SearchFinancialQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}

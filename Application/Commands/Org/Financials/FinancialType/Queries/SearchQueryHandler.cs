namespace Application.Commands.Org.Financials.FinancialType.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Queries;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;
    using Utility;

    public sealed record SearchFinancialTypeQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) 
        : ICommandPagination<FinancialTypeModelView> ,ISearchQuery<ResultPagination<FinancialTypeModelView>>;

    public sealed class SearchQueryHandler(IRepository<Domain.Entities.FinancialType> _Repository, IMapper mapper) : 
        SearchCommandHandler<SearchFinancialTypeQuery, Domain.Entities.FinancialType, FinancialTypeModelView>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.FinancialType, bool>> CreateFilter(SearchFinancialTypeQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Code.Contains(request.KeySearch)) &&
            (request.ParentId ==0 || e.ParentId == request.ParentId) &&
            (request.TypeId == 0 || e.TypeId == request.TypeId) &&
            e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }

        public override string CreateInclude()
        {
            return "";
        }

        override public Func<IQueryable<Domain.Entities.FinancialType>, IOrderedQueryable<Domain.Entities.FinancialType>> CreateOrderBy(SearchFinancialTypeQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}
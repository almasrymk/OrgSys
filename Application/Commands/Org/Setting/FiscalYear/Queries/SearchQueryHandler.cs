namespace Application.Commands.Org.Setting.FiscalYear.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Queries;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;

    public sealed record SearchFiscalYearQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize) : ICommandPagination<FiscalYearDto>, ISearchQuery<ResultPagination<FiscalYearDto>>;

    public sealed class SearchQueryHandler(IRepository<Domain.Entities.FiscalYear> _Repository, IMapper mapper) : SearchCommandHandler<SearchFiscalYearQuery, Domain.Entities.FiscalYear, FiscalYearDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.FiscalYear, bool>> CreateFilter(SearchFiscalYearQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }

        override public Func<IQueryable<Domain.Entities.FiscalYear>, IOrderedQueryable<Domain.Entities.FiscalYear>> CreateOrderBy(SearchFiscalYearQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }

        public override string CreateInclude()
        {
            return "Periods";
        }
    }
}

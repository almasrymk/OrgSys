namespace Application.Commands.Org.Setting.CashBox.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Queries;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;

    public sealed record SearchCashBoxQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize) : ICommandPagination<CashBoxDto>, ISearchQuery<ResultPagination<CashBoxDto>>;

    public sealed class SearchQueryHandler(IRepository<Domain.Entities.CashBox> _Repository, IMapper mapper) : SearchCommandHandler<SearchCashBoxQuery, Domain.Entities.CashBox, CashBoxDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.CashBox, bool>> CreateFilter(SearchCashBoxQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }

        override public Func<IQueryable<Domain.Entities.CashBox>, IOrderedQueryable<Domain.Entities.CashBox>> CreateOrderBy(SearchCashBoxQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}

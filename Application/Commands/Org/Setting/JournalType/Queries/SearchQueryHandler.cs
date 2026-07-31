namespace Application.Commands.Org.Setting.JournalType.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Queries;
    using Application.DTOs;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using System.Linq.Expressions;

    public sealed record SearchJournalTypeQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize)
        : ICommandPagination<JournalTypeDto>, ISearchQuery<ResultPagination<JournalTypeDto>>;

    public sealed class SearchQueryHandler(IRepository<Domain.Entities.JournalType> repository, IMapper mapper)
        : SearchCommandHandler<SearchJournalTypeQuery, Domain.Entities.JournalType, JournalTypeDto>(repository, mapper)
    {
        public override Expression<Func<Domain.Entities.JournalType, bool>> CreateFilter(SearchJournalTypeQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;
            return e =>
                (string.IsNullOrEmpty(request.KeySearch) || (e.Name ?? "").Contains(request.KeySearch)) &&
                e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }

        public override Func<IQueryable<Domain.Entities.JournalType>, IOrderedQueryable<Domain.Entities.JournalType>> CreateOrderBy(SearchJournalTypeQuery request)
        {
            return q => q.OrderBy(e => e.Id);
        }
    }
}

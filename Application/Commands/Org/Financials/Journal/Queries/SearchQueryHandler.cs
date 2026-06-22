namespace Application.Commands.Org.Financials.Journal.Queries
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

    public sealed record SearchJournalQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<JournalModelView> ,ISearchQuery<ResultPagination<JournalModelView>>;

    public sealed class SearchQueryHandler(IRepository<Domain.Entities.Journal> _Repository, IMapper mapper) : SearchCommandHandler<SearchJournalQuery, Domain.Entities.Journal, JournalModelView>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.Journal, bool>> CreateFilter(SearchJournalQuery request)
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
            return "Currency";
        }

        override public Func<IQueryable<Domain.Entities.Journal>, IOrderedQueryable<Domain.Entities.Journal>> CreateOrderBy(SearchJournalQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}
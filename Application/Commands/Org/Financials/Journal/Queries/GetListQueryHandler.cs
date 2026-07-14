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

    public sealed record GetListJournalQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize) : ICommandCollection<JournalDto>, IListQuery<ResultCollection<JournalDto>>;

    public sealed class GetListQueryHandler(IRepository<Domain.Entities.Journal> _Repository, IMapper mapper) : ListCommandHandler<GetListJournalQuery, Domain.Entities.Journal, JournalDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.Journal, bool>> CreateFilter(GetListJournalQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
           (string.IsNullOrEmpty(request.KeySearch) || e.Code.Contains(request.KeySearch)) &&
           (request.ParentId == 0 || e.ParentId == request.ParentId) &&
           (request.TypeId == 0 || e.TypeId == request.TypeId) &&
           e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }

        override public Func<IQueryable<Domain.Entities.Journal>, IOrderedQueryable<Domain.Entities.Journal>> CreateOrderBy(GetListJournalQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}
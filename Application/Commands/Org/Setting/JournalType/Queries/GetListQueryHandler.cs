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

    public sealed record GetListJournalTypeQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize)
        : ICommandCollection<JournalTypeDto>, IListQuery<ResultCollection<JournalTypeDto>>;

    public sealed class GetListQueryHandler(IRepository<Domain.Entities.JournalType> repository, IMapper mapper)
        : ListCommandHandler<GetListJournalTypeQuery, Domain.Entities.JournalType, JournalTypeDto>(repository, mapper)
    {
        public override Expression<Func<Domain.Entities.JournalType, bool>> CreateFilter(GetListJournalTypeQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;
            return e =>
                (string.IsNullOrEmpty(request.KeySearch) || (e.Name ?? "").Contains(request.KeySearch)) &&
                e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }

        public override Func<IQueryable<Domain.Entities.JournalType>, IOrderedQueryable<Domain.Entities.JournalType>> CreateOrderBy(GetListJournalTypeQuery request)
        {
            return q => q.OrderBy(e => e.Id);
        }
    }
}

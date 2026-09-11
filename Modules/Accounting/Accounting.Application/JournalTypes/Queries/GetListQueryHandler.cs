namespace Accounting.Application.JournalTypes.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetListJournalTypeQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize)
        : ICommandCollection<JournalTypeDto>, IListQuery<ResultCollection<JournalTypeDto>>;

    public sealed class GetListQueryHandler(IRepository<Accounting.Domain.JournalType> repository, IMapper mapper)
        : ListCommandHandler<GetListJournalTypeQuery, Accounting.Domain.JournalType, JournalTypeDto>(repository, mapper)
    {
        public override Expression<Func<Accounting.Domain.JournalType, bool>> CreateFilter(GetListJournalTypeQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;
            return e =>
                (string.IsNullOrEmpty(request.KeySearch) || (e.Name ?? "").Contains(request.KeySearch)) &&
                e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        public override Func<IQueryable<Accounting.Domain.JournalType>, IOrderedQueryable<Accounting.Domain.JournalType>> CreateOrderBy(GetListJournalTypeQuery request)
        {
            return q => q.OrderBy(e => e.Id);
        }
    }
}

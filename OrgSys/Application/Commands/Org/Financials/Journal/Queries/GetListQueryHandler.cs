namespace Application.Commands.Org.Financials.Journal.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Queries;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;
    using System.Linq.Expressions;
    using Utility;

    public sealed record GetListJournalQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize) : ICommandCollection<JournalModelView>, IListQuery<ResultCollection<JournalModelView>>;

    public sealed class GetListQueryHandler(IRepository<Entity.Model.Journal> _Repository, IMapper mapper) : ListCommandHandler<GetListJournalQuery, Entity.Model.Journal, JournalModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.Journal, bool>> CreateFilter(GetListJournalQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
           (string.IsNullOrEmpty(request.KeySearch) || e.Code.Contains(request.KeySearch)) &&
           (request.ParentId == 0 || e.ParentId == request.ParentId) &&
           (request.TypeId == 0 || e.TypeId == request.TypeId) &&
           e.Status != Status.Deleted && e.Hide != true;
        }

        override public Func<IQueryable<Entity.Model.Journal>, IOrderedQueryable<Entity.Model.Journal>> CreateOrderBy(GetListJournalQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}
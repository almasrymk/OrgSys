namespace Application.Commands.Org.Setting.Branch.Queries
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

    public sealed record SearchBranchQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<BranchModelView> ,ISearchQuery<ResultPagination<BranchModelView>>;

    public sealed class SearchQueryHandler(IRepository<Entity.Model.Branch> _Repository, IMapper mapper) : SearchCommandHandler<SearchBranchQuery, Entity.Model.Branch, BranchModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.Branch, bool>> CreateFilter(SearchBranchQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Entity.Model.Branch>, IOrderedQueryable<Entity.Model.Branch>> CreateOrderBy(SearchBranchQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}
namespace Organization.Application.Branches.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record SearchBranchQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<BranchDto> ,ISearchQuery<ResultPagination<BranchDto>>;

    public sealed class SearchQueryHandler(IRepository<Organization.Domain.Branch> _Repository, IMapper mapper) : SearchCommandHandler<SearchBranchQuery, Organization.Domain.Branch, BranchDto>(_Repository, mapper)
    {
        public override Expression<Func<Organization.Domain.Branch, bool>> CreateFilter(SearchBranchQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Name!.Contains(request.KeySearch)) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Organization.Domain.Branch>, IOrderedQueryable<Organization.Domain.Branch>> CreateOrderBy(SearchBranchQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}
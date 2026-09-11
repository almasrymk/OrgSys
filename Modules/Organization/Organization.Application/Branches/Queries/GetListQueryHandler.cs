namespace Organization.Application.Branches.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetListBranchQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<BranchDto> , IListQuery<ResultCollection<BranchDto>>;

    public sealed class GetListQueryHandler(IRepository<Organization.Domain.Branch> _Repository, IMapper mapper) : ListCommandHandler<GetListBranchQuery, Organization.Domain.Branch, BranchDto>(_Repository, mapper)
    {
        public override Expression<Func<Organization.Domain.Branch, bool>> CreateFilter(GetListBranchQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name!.Contains(request.KeySearch)) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Organization.Domain.Branch>, IOrderedQueryable<Organization.Domain.Branch>> CreateOrderBy(GetListBranchQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}
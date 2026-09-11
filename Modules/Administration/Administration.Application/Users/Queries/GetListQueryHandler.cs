namespace Administration.Application.Users.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetListUserQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<UserDto> , IListQuery<ResultCollection<UserDto>>;

    public sealed class GetListQueryHandler(IRepository<Administration.Domain.User> _Repository, IMapper mapper) : ListCommandHandler<GetListUserQuery, Administration.Domain.User, UserDto>(_Repository, mapper)
    {
        public override Expression<Func<Administration.Domain.User, bool>> CreateFilter(GetListUserQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Administration.Domain.User>, IOrderedQueryable<Administration.Domain.User>> CreateOrderBy(GetListUserQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }

        public override string CreateInclude()
        {
            return "Role,Branch";
        }
    }
}
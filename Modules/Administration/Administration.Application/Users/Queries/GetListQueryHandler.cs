namespace Administration.Application.Users.Queries
{
    using Organization.Contracts.Branches;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using MediatR;
    using System.Linq.Expressions;

    public sealed record GetListUserQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<UserDto> , IListQuery<ResultCollection<UserDto>>;

    public sealed class GetListQueryHandler(IRepository<Administration.Domain.User> _Repository, IMapper mapper, ISender sender) : ListCommandHandler<GetListUserQuery, Administration.Domain.User, UserDto>(_Repository, mapper)
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
            return "Role";
        }

        public override async Task<ResultCollection<UserDto>> Handle(GetListUserQuery request, CancellationToken cancellationToken)
        {
            var result = await base.Handle(request, cancellationToken);
            var branchIds = result.Response.Where(e => e.BranchId is > 0).Select(e => e.BranchId!.Value).Distinct().ToList();
            if (branchIds.Count > 0)
            {
                var names = (await sender.Send(new GetBranchNamesQuery(branchIds), cancellationToken)).Response ?? [];
                foreach (var dto in result.Response)
                    if (dto.BranchId is > 0)
                        dto.BranchName = names.GetValueOrDefault(dto.BranchId.Value);
            }

            return result;
        }
    }
}

namespace Administration.Application.Users.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using Organization.Contracts.Branches;
    using AutoMapper;
    using MediatR;
    using System.Linq.Expressions;
    using System.Net;

    public sealed record GetByIdUserQuery(long Id) : ICommand<UserDto> , IGetByIdQuery<Result<UserDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Administration.Domain.User> _Repository,
        IRepository<RolePermission> _RepositoryRolePermission,
        IRepository<Permission> _RepositoryPermission,
        IMapper _Mapper,
         IMapper mapper,
         ISender sender) : GetCommandHandler<GetByIdUserQuery, Administration.Domain.User, UserDto>(_Repository, mapper)
    {

        public override async Task<Result<UserDto>> Handle(GetByIdUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _Repository.GetByFilterAsync(CreateFilter(request), "Role");

            if (user is null)
            {
                return new Result<UserDto>(HttpStatusCode.NotFound, null, new List<Error> { new Error("User not found") });
            }

            var rolePermissions = await _RepositoryRolePermission
                .GetListByFilterAsync(r => r.RoleId == user.RoleId);

            var permissionIds = rolePermissions.Select(r => r.PermissionId).ToList();

            var permissions = permissionIds.Any()
                ? await _RepositoryPermission.GetListByFilterAsync(p => permissionIds.Contains(p.Id))
                : Enumerable.Empty<Permission>();

            var userDto = _Mapper.Map<UserDto>(user);

            userDto.Permissions = permissions.ToList();
            if (user.BranchId is > 0)
            {
                var names = (await sender.Send(new GetBranchNamesQuery([user.BranchId.Value]), cancellationToken)).Response ?? [];
                userDto.BranchName = names.GetValueOrDefault(user.BranchId.Value);
            }

            return new Result<UserDto>(HttpStatusCode.OK,userDto,null);
        }
        public override Expression<Func<Administration.Domain.User, bool>> CreateFilter(GetByIdUserQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        public override string CreateInclude()
        {
            return "Role";
        }
    }
}
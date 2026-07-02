namespace Application.Commands.Org.Setting.User.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Common.Queries;
    using Application.DTOs;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Entities;
    using Domain.Shared;
    using System.Linq.Expressions;
    using System.Net;

    public sealed record GetByIdUserQuery(long Id) : ICommand<UserDto> , IGetByIdQuery<Result<UserDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Domain.Entities.User> _Repository,
        IRepository<RolePermission> _RepositoryRolePermission,
        IRepository<Permission> _RepositoryPermission,
        IMapper _Mapper,
         IMapper mapper) : GetCommandHandler<GetByIdUserQuery, Domain.Entities.User, UserDto>(_Repository, mapper)
    {

        public override async Task<Result<UserDto>> Handle(GetByIdUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _Repository.GetByFilterAsync(CreateFilter(request), "");

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

            return new Result<UserDto>(HttpStatusCode.OK,userDto,null);
        }
        public override Expression<Func<Domain.Entities.User, bool>> CreateFilter(GetByIdUserQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Domain.Enums.Status.Deleted && e.Hide != true;
        }

        public override string CreateInclude()
        {
            return "Role,Branch";
        }
    }
}
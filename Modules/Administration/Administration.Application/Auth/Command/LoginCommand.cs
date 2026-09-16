using OrgSys.SharedKernel;
using Administration.Application.Security;
using AutoMapper;
using System.Net;

namespace Administration.Application.Auth.Commands
{
    public sealed record LoginCommand(string UserName,string Password): ICommand<UserDto>;
    public sealed class LoginCommandHandler(IRepository<User> repository,
        IRepository<RolePermission> RolePermission,
        IRepository<Permission> PermissionRepository
        , IMapper mapper
        , IPasswordHasher passwordHasher
        ) : ICommandHandler<LoginCommand, UserDto>
    {
        public async Task<Result<UserDto>> Handle(
            LoginCommand request,
            CancellationToken cancellationToken)
        {
            var user = await repository.GetByFilterAsync(u => u.UserName == request.UserName, "Role");

            if (user is null
                || string.IsNullOrEmpty(user.Password)
                || !passwordHasher.VerifyHashedPassword(user.Password, request.Password))
            {
                return new Result<UserDto>(HttpStatusCode.NotFound, null, null);
            }

            if (user.MustResetPassword)
            {
                return new Result<UserDto>(
                    HttpStatusCode.Forbidden,
                    null,
                    new List<Error> { new Error("Password reset required. Existing credentials were invalidated by the password-hashing migration; an administrator must set a new password.", "PasswordResetRequired") });
            }

            var result = mapper.Map<UserDto>(user);

            var rolePermissions = await RolePermission.GetListByFilterAsync(e => e.RoleId == result.RoleId, "");
                
            if (rolePermissions is null || !rolePermissions.Any()) 
                return new Result<UserDto>(HttpStatusCode.NotFound, result, null);
            

            var permissions = await PermissionRepository.GetListByFilterAsync(e => rolePermissions.Select(rp => rp.PermissionId).Contains(e.Id), "");

            if (permissions is null || !permissions.Any())
                return new Result<UserDto>(HttpStatusCode.NotFound, result, null);

            result.Permissions = permissions.ToList();
   
            return new Result<UserDto>(HttpStatusCode.OK, result, null);
        }
    }
}

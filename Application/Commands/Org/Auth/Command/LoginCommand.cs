using Application.Abstraction.Command;
using Application.DTOs;
using AutoMapper;
using Domain.Abstraction;
using Domain.Entities;
using Domain.Shared;
using System.Net;

namespace Application.Commands.Authentication.Commands
{
    public sealed record LoginCommand(string UserName,string Password): ICommand<UserDto>;
    public sealed class LoginCommandHandler(IRepository<User> repository,
        IRepository<RolePermission> RolePermission,
        IRepository<Permission> PermissionRepository
        , IMapper mapper
        ) : ICommandHandler<LoginCommand, UserDto>
    {
        public async Task<Result<UserDto>> Handle(
            LoginCommand request,
            CancellationToken cancellationToken)
        {
            var p = Security.Encrypt(request.Password);
            //p = "fTxWMjHA5MbUktJph2vqIlc9Gu1cU5MrbdYztkd5yec=";
            var user = await repository.GetByFilterAsync(u => u.UserName ==request.UserName && u.Password == p, "Role");

            var result = mapper.Map<UserDto>(user);

            if (user is null)
                return new Result<UserDto>(HttpStatusCode.NotFound, result, null);

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
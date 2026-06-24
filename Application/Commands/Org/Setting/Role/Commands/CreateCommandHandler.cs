namespace Application.Commands.Org.Setting.Role.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    public sealed class CreateRoleCommand : RoleDto, ICommand, ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.Role> _Repository, IRepository<Domain.Entities.RolePermission> _rolePermissionRepository, IMapper mapper) : CreateCommandHandler<CreateRoleCommand, Domain.Entities.Role>(_UnitOfWork, _Repository, mapper)
    {
        public override async Task<Result> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var ob = mapper.Map<Domain.Entities.Role>(request);
                var res = await _Repository.CreateAsync(ob);

                if (_UnitOfWork.SaveChangeAsync().Result > 0)
                {
                    await _rolePermissionRepository.CreateAsync(CreateRolePermissions(request.PermissionList, res.Id));

                    if (_UnitOfWork.SaveChangeAsync().Result > 0)
                    {
                        return new Result(HttpStatusCode.OK, null);
                    }
                }
                
                return new Result(
                    HttpStatusCode.InternalServerError,
                    new List<Error> { new Error("Error") });
            }
            catch (Exception ex)
            {
                return new Result(
                    HttpStatusCode.InternalServerError,
                    new List<Error> { new Error(ex.Message) });
            }
        }

        private List<Domain.Entities.RolePermission> CreateRolePermissions(List<Domain.Entities.RolePermission> RolePermissionList, long RoleId)
        {
            foreach (var rolePermissions in RolePermissionList)
                rolePermissions.RoleId = RoleId;

            return RolePermissionList;
        }
    }
}
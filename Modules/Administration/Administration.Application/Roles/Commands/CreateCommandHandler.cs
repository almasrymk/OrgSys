namespace Administration.Application.Roles.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    public sealed class CreateRoleCommand : RoleDto, ICommand, ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Administration.Domain.Role> _Repository, IRepository<Administration.Domain.RolePermission> _rolePermissionRepository, IMapper mapper) : CreateCommandHandler<CreateRoleCommand, Administration.Domain.Role>(_UnitOfWork, _Repository, mapper)
    {
        public override async Task<Result> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var ob = mapper.Map<Administration.Domain.Role>(request);
                var res = await _Repository.CreateAsync(ob);

                if (await _UnitOfWork.SaveChangeAsync(cancellationToken) > 0)
                {
                    await _rolePermissionRepository.CreateAsync(CreateRolePermissions(request.PermissionList, res.Id));

                    if (await _UnitOfWork.SaveChangeAsync(cancellationToken) > 0)
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

        private List<Administration.Domain.RolePermission> CreateRolePermissions(List<Administration.Domain.RolePermission> RolePermissionList, long RoleId)
        {
            foreach (var rolePermissions in RolePermissionList)
                rolePermissions.RoleId = RoleId;

            return RolePermissionList;
        }
    }
}
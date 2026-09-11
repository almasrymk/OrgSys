namespace Administration.Application.Roles.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Net;

    public sealed class UpdateRoleCommand : RoleDto, ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Administration.Domain.Role> _Repository, IRepository<Administration.Domain.RolePermission> _rolePermissionRepository, IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateRoleCommand, Administration.Domain.Role>(_UnitOfWork, _Repository, mapper , _provider)
    {

        public override async Task<Result> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var ob = mapper.Map<Administration.Domain.Role>(request);
                var res = await _Repository.UpdateAsync(ob);

                var ids = await GetListIds(ob.Id);
                await _rolePermissionRepository.ShiftDeleteAsync(e => ids.Contains(e.Id));
                await _rolePermissionRepository.CreateAsync(CreateRolePermissions(request.PermissionList, ob.Id));

                if (await _UnitOfWork.SaveChangeAsync(cancellationToken) > 0)
                {
                    return new Result(HttpStatusCode.OK, null);
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

        public async Task<List<long>> GetListIds(long RoleId)
        {
            var deleted = await _rolePermissionRepository.GetListByFilterAsync(e => e.RoleId == RoleId);
            if (deleted == null)
                deleted = new List<Administration.Domain.RolePermission>();
            return deleted.Select(e => e.Id).ToList();
        }
    }
}
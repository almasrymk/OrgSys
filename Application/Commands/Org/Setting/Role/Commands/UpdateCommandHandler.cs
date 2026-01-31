namespace Application.Commands.Org.Setting.Role.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;
    using System.Net;

    public sealed class UpdateRoleCommand : RoleModelView, ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Role> _Repository, IRepository<Entity.Model.RolePermission> _rolePermissionRepository, IMapper mapper) : UpdateCommandHandler<UpdateRoleCommand, Entity.Model.Role>(_UnitOfWork, _Repository, mapper)
    {

        public override async Task<Result> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var ob = mapper.Map<Entity.Model.Role>(request);
                var res = await _Repository.UpdateAsync(ob);

                var ids = await GetListIds(ob.Id);
                await _rolePermissionRepository.ShiftDeleteAsync(e => ids.Contains(e.Id));
                await _rolePermissionRepository.CreateAsync(CreateRolePermissions(request.PermissionList, ob.Id));

                if (_UnitOfWork.SaveChangeAsync().Result > 0)
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

        private List<Entity.Model.RolePermission> CreateRolePermissions(List<Entity.Model.RolePermission> RolePermissionList, long RoleId)
        {
            foreach (var rolePermissions in RolePermissionList)
                rolePermissions.RoleId = RoleId;

            return RolePermissionList;
        }

        public async Task<List<long>> GetListIds(long RoleId)
        {
            var deleted = await _rolePermissionRepository.GetListByFilterAsync(e => e.RoleId == RoleId);
            if (deleted == null)
                deleted = new List<Entity.Model.RolePermission>();
            return deleted.Select(e => e.Id).ToList();
        }
    }
}
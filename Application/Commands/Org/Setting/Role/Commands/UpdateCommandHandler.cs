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

    public sealed class UpdateRoleCommand : RoleDto, ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.Role> _Repository, IRepository<Domain.Entities.RolePermission> _rolePermissionRepository, IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateRoleCommand, Domain.Entities.Role>(_UnitOfWork, _Repository, mapper , _provider)
    {

        public override async Task<Result> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var ob = mapper.Map<Domain.Entities.Role>(request);
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

        private List<Domain.Entities.RolePermission> CreateRolePermissions(List<Domain.Entities.RolePermission> RolePermissionList, long RoleId)
        {
            foreach (var rolePermissions in RolePermissionList)
                rolePermissions.RoleId = RoleId;

            return RolePermissionList;
        }

        public async Task<List<long>> GetListIds(long RoleId)
        {
            var deleted = await _rolePermissionRepository.GetListByFilterAsync(e => e.RoleId == RoleId);
            if (deleted == null)
                deleted = new List<Domain.Entities.RolePermission>();
            return deleted.Select(e => e.Id).ToList();
        }
    }
}
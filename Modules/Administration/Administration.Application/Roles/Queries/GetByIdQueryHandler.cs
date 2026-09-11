namespace Administration.Application.Roles.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    public sealed record GetByIdRoleQuery(long Id) : ICommand<RoleDto> , IGetByIdQuery<Result<RoleDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Administration.Domain.Role> _Repository, IRepository<Administration.Domain.RolePermission> _RolePermissionRepository, IRepository<Administration.Domain.Permission> _PermissionRepository, IMapper mapper) : GetCommandHandler<GetByIdRoleQuery, Administration.Domain.Role, RoleDto>(_Repository, mapper)
    {
        public override async Task<Result<RoleDto>> Handle(GetByIdRoleQuery request, CancellationToken cancellationToken)
        {
            var ob = await _Repository.GetByFilterAsync(e => e.Id == request.Id, CreateInclude());
            if (ob == null)
                ob = new Role();

            var PermissionList = await GetPermissions();
            var RolePermissionList = await GetRolePermissions(request.Id);
            var obModel = Map(ob, RolePermissionList, PermissionList);

            return new Result<RoleDto>(
                    HttpStatusCode.OK,
                    obModel,
                    null);
        }

        public override Expression<Func<Administration.Domain.Role, bool>> CreateFilter(GetByIdRoleQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        public override string CreateInclude()
        {
            return base.CreateInclude();
        }

        private async Task< IEnumerable<RolePermission>> GetRolePermissions(long roleId)
        {
            var List = await _RolePermissionRepository.GetListByFilterAsync(e => e.RoleId == roleId);
            return List!;
        }

        private async Task<IEnumerable<Permission>> GetPermissions()
        {
            var List = await _PermissionRepository.GetListByFilterAsync(CreateInclude());
            return List!;
        }

        private RoleDto Map(Role ob , IEnumerable<RolePermission> rolePermissions , IEnumerable<Permission> permissions)
        {
            var obModel = mapper.Map<RoleDto>(ob);
            obModel.PermissionsTree = permissions.Select(e => new TreeView { Id = e.Id, Key = e.Key, Value = global::Domain.Resource.Translate.GetTranslate(e.Name), ParentId = e.ParentId }).ToList();
            foreach (var item in obModel.PermissionsTree)
            {
                if (rolePermissions.Any(e => e.PermissionId == item.Id))
                    item.Select = true;
            }

            return obModel;
        }
    }
}
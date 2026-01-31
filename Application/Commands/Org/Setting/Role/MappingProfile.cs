using Application.Commands.Org.Setting.Role.Commands;
using Application.Commands.Org.Setting.Role.Commands;
using AutoMapper;
using Entity.Model;
using Entity.ModelView;

public partial class MappingProfile : Profile
{
    public void RoleMappingProfile()
    {
        #region Role
        CreateMap<Role, RoleModelView>();
        CreateMap<RoleModelView, Role>();

        CreateMap<Role, CreateRoleCommand>();
        CreateMap<CreateRoleCommand, Role>();
        CreateMap<Role, UpdateRoleCommand>();
        CreateMap<UpdateRoleCommand, Role>();
        CreateMap<Role, DeleteRoleCommand>();
        CreateMap<DeleteRoleCommand, Role>();

        CreateMap<RoleModelView, CreateRoleCommand>();
        CreateMap<CreateRoleCommand, RoleModelView>();
        CreateMap<RoleModelView, UpdateRoleCommand>();
        CreateMap<UpdateRoleCommand, RoleModelView>();
        #endregion
    }
}
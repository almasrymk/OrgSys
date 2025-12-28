using AutoMapper;
using Entity.Model;
using Entity.ModelView;
using Application.Commands.Org.Setting.Role.Commands;

public partial class MappingProfile : Profile
{
    public void RoleMappingProfile()
    {
        #region Role
        CreateMap<Role, RoleModelView>();
        CreateMap<RoleModelView, Role>();       
        #endregion
    }
}
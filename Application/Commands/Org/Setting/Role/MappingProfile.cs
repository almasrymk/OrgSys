using Application.Commands.Org.Setting.Role.Commands;
using AutoMapper;
using Domain.Entities;
using Application.DTOs;

public partial class MappingProfile : Profile
{
    public void RoleMappingProfile()
    {
        #region Role
        CreateMap<Role, RoleDto>();
        CreateMap<RoleDto, Role>();

        CreateMap<Role, CreateRoleCommand>();
        CreateMap<CreateRoleCommand, Role>();
        CreateMap<Role, UpdateRoleCommand>();
        CreateMap<UpdateRoleCommand, Role>();
        CreateMap<Role, DeleteRoleCommand>();
        CreateMap<DeleteRoleCommand, Role>();

        CreateMap<RoleDto, CreateRoleCommand>();
        CreateMap<CreateRoleCommand, RoleDto>();
        CreateMap<RoleDto, UpdateRoleCommand>();
        CreateMap<UpdateRoleCommand, RoleDto>();
        #endregion
    }
}
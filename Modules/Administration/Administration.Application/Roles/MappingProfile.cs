namespace Administration.Application;

﻿using Administration.Application.Roles.Commands;
using AutoMapper;

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
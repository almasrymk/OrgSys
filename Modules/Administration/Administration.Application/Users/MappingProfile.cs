namespace Administration.Application;

﻿using Administration.Application.Users.Commands;
using AutoMapper;

public partial class MappingProfile : Profile
{
    public void UserMappingProfile()
    {
        #region User
        CreateMap<User, UserDto>()
        .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.Name))
        .ForMember(dest => dest.BranchName, opt => opt.Ignore())
        .ForMember(dest => dest.Password, opt => opt.Ignore())
        .ForMember(dest => dest.NewPassword, opt => opt.Ignore())
        .ForMember(dest => dest.ConfirmPassword, opt => opt.Ignore());
        CreateMap<UserDto, User>();

        CreateMap<User, CreateUserCommand>();
        CreateMap<CreateUserCommand, User>();
        CreateMap<User, UpdateUserCommand>();
        CreateMap<UpdateUserCommand, User>();
        CreateMap<User, DeleteUserCommand>();
        CreateMap<DeleteUserCommand, User>();

        CreateMap<UserDto, CreateUserCommand>();
        CreateMap<CreateUserCommand, UserDto>();
        CreateMap<UserDto, UpdateUserCommand>();
        CreateMap<UpdateUserCommand, UserDto>();
        #endregion
    }
}
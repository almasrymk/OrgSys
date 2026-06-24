using Application.Commands.Org.Setting.User.Commands;
using AutoMapper;
using Domain.Entities;
using Application.DTOs;

public partial class MappingProfile : Profile
{
    public void UserMappingProfile()
    {
        #region User
        CreateMap<User, UserDto>()
        .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.Name))
        .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch.Name));
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
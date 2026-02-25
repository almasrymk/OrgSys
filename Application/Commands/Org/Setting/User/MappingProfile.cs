using Application.Commands.Org.Setting.User.Commands;
using AutoMapper;
using Entity.Model;
using Entity.ModelView;

public partial class MappingProfile : Profile
{
    public void UserMappingProfile()
    {
        #region User
        CreateMap<User, UserModelView>()
        .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.Name))
        .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch.Name));
        CreateMap<UserModelView, User>();

        CreateMap<User, CreateUserCommand>();
        CreateMap<CreateUserCommand, User>();
        CreateMap<User, UpdateUserCommand>();
        CreateMap<UpdateUserCommand, User>();
        CreateMap<User, DeleteUserCommand>();
        CreateMap<DeleteUserCommand, User>();

        CreateMap<UserModelView, CreateUserCommand>();
        CreateMap<CreateUserCommand, UserModelView>();
        CreateMap<UserModelView, UpdateUserCommand>();
        CreateMap<UpdateUserCommand, UserModelView>();
        #endregion
    }
}
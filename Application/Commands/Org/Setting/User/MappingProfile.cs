using AutoMapper;
using Entity.Model;
using Entity.ModelView;
using Application.Commands.Org.Setting.User.Commands;

public partial class MappingProfile : Profile
{
    public void UserMappingProfile()
    {
        #region User
        CreateMap<User, UserModelView>();
        CreateMap<UserModelView, User>();       
        #endregion
    }
}
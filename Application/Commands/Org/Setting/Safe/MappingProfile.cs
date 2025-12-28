using AutoMapper;
using Entity.Model;
using Entity.ModelView;
using Application.Commands.Org.Setting.Safe.Commands;

public partial class MappingProfile : Profile
{
    public void SafeMappingProfile()
    {
        #region Safe
        CreateMap<Safe, SafeModelView>();
        CreateMap<SafeModelView, Safe>();       
        #endregion
    }
}
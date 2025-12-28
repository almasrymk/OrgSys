using AutoMapper;
using Entity.Model;
using Entity.ModelView;
using Application.Commands.Org.Setting.Unit.Commands;

public partial class MappingProfile : Profile
{
    public void UnitMappingProfile()
    {
        #region Unit
        CreateMap<Unit, UnitModelView>();
        CreateMap<UnitModelView, Unit>();       
        #endregion
    }
}
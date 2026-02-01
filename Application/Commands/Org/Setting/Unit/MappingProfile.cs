using Application.Commands.Org.Setting.Unit.Commands;
using AutoMapper;
using Entity.Model;
using Entity.ModelView;

public partial class MappingProfile : Profile
{
    public void UnitMappingProfile()
    {
        #region Unit
        CreateMap<Unit, UnitModelView>();
        CreateMap<UnitModelView, Unit>();

        CreateMap<Unit, CreateUnitCommand>();
        CreateMap<CreateUnitCommand, Unit>();
        CreateMap<Unit, UpdateUnitCommand>();
        CreateMap<UpdateUnitCommand, Unit>();
        CreateMap<Unit, DeleteUnitCommand>();
        CreateMap<DeleteUnitCommand, Unit>();

        CreateMap<UnitModelView, CreateUnitCommand>();
        CreateMap<CreateUnitCommand, UnitModelView>();
        CreateMap<UnitModelView, UpdateUnitCommand>();
        CreateMap<UpdateUnitCommand, UnitModelView>();
        #endregion
    }
}
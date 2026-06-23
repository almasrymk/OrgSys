using Application.Commands.Org.Setting.Unit.Commands;
using AutoMapper;
using Domain.Entities;
using Application.DTOs;

public partial class MappingProfile : Profile
{
    public void UnitMappingProfile()
    {
        #region Unit
        CreateMap<Unit, UnitDto>();
        CreateMap<UnitDto, Unit>();

        CreateMap<Unit, CreateUnitCommand>();
        CreateMap<CreateUnitCommand, Unit>();
        CreateMap<Unit, UpdateUnitCommand>();
        CreateMap<UpdateUnitCommand, Unit>();
        CreateMap<Unit, DeleteUnitCommand>();
        CreateMap<DeleteUnitCommand, Unit>();

        CreateMap<UnitDto, CreateUnitCommand>();
        CreateMap<CreateUnitCommand, UnitDto>();
        CreateMap<UnitDto, UpdateUnitCommand>();
        CreateMap<UpdateUnitCommand, UnitDto>();
        #endregion
    }
}
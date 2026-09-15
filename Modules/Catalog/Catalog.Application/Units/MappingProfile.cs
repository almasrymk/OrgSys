namespace Catalog.Application;

﻿using Catalog.Application.Units.Commands;
using AutoMapper;
// Bare "Unit" collides with MediatR.Unit (pulled in globally for IRequest<Unit>) — alias needed
// only in this file since it's the only one that names the type unqualified.
using Unit = Catalog.Domain.Unit;

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

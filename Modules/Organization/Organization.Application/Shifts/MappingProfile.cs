namespace Organization.Application;

﻿using AutoMapper;
using Organization.Application.Shifts.Commands;

public partial class MappingProfile : Profile
{
    public void ShiftMappingProfile()
    {
        #region Shift
        CreateMap<Shift, ShiftDto>();
        CreateMap<ShiftDto, Shift>();       
        #endregion
    }
}
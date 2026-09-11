namespace Treasury.Application;

﻿using AutoMapper;
using Treasury.Application.Outlays.Commands;

public partial class MappingProfile : Profile
{
    public void OutlayMappingProfile()
    {
        #region Outlay
        CreateMap<Outlay, OutlayDto>();
        CreateMap<OutlayDto, Outlay>();       
        #endregion
    }
}
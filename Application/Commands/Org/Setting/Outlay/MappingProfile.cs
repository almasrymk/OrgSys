using AutoMapper;
using Domain.Entities;
using Application.DTOs;
using Application.Commands.Org.Setting.Outlay.Commands;

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
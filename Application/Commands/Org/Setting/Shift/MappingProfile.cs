using AutoMapper;
using Domain.Entities;
using Application.DTOs;
using Application.Commands.Org.Setting.Shift.Commands;

public partial class MappingProfile : Profile
{
    public void ShiftMappingProfile()
    {
        #region Shift
        CreateMap<Shift, ShiftModelView>();
        CreateMap<ShiftModelView, Shift>();       
        #endregion
    }
}
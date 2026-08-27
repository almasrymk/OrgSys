using Application.Commands.Org.Setting.CashBox.Commands;
using AutoMapper;
using Domain.Entities;
using Application.DTOs;

public partial class MappingProfile : Profile
{
    public void CashBoxMappingProfile()
    {
        #region CashBox
        CreateMap<CashBox, CashBoxDto>()
        .ForMember(dest => dest.AccountName, opt => opt.MapFrom(src => src.Account.Name));

        CreateMap<CashBoxDto, CashBox>();

        CreateMap<CashBox, CreateCashBoxCommand>();
        CreateMap<CreateCashBoxCommand, CashBox>();
        CreateMap<CashBox, UpdateCashBoxCommand>();
        CreateMap<UpdateCashBoxCommand, CashBox>();
        CreateMap<CashBox, DeleteCashBoxCommand>();
        CreateMap<DeleteCashBoxCommand, CashBox>();

        CreateMap<CashBoxDto, CreateCashBoxCommand>();
        CreateMap<CreateCashBoxCommand, CashBoxDto>();
        CreateMap<CashBoxDto, UpdateCashBoxCommand>();
        CreateMap<UpdateCashBoxCommand, CashBoxDto>();
        #endregion
    }
}

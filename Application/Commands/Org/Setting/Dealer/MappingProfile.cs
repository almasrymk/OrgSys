using Application.Commands.Org.Setting.Dealer.Commands;
using AutoMapper;
using Domain.Entities;
using Application.DTOs;

public partial class MappingProfile : Profile
{
    public void DealerMappingProfile()
    {
        #region Dealer
        CreateMap<Dealer, DealerDto>()
        .ForMember(dest => dest.DealerGroupName, opt => opt.MapFrom(src => src.DealerGroup.Name))
        .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.Country.Name))
        .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City.Name))
        .ForMember(dest => dest.AccountCode, opt => opt.MapFrom(src => src.Account.Code))
        .ForMember(dest => dest.AccountName, opt => opt.MapFrom(src => src.Account.Name))
        .ForMember(dest => dest.DistrictName, opt => opt.MapFrom(src => src.District.Name));
        CreateMap<DealerDto, Dealer>();

        CreateMap<Dealer, CreateDealerCommand>();
        CreateMap<CreateDealerCommand, Dealer>();
        CreateMap<Dealer, UpdateDealerCommand>();
        CreateMap<UpdateDealerCommand, Dealer>();
        CreateMap<Dealer, DeleteDealerCommand>();
        CreateMap<DeleteDealerCommand, Dealer>();

        CreateMap<DealerDto, CreateDealerCommand>();
        CreateMap<CreateDealerCommand, DealerDto>();
        CreateMap<DealerDto, UpdateDealerCommand>();
        CreateMap<UpdateDealerCommand, DealerDto>();
        #endregion
    }
}
namespace Parties.Application;

﻿using Parties.Application.Dealers.Commands;
using AutoMapper;

public partial class MappingProfile : Profile
{
    public void DealerMappingProfile()
    {
        #region Dealer
        // AccountCode/AccountName are no longer populated via an EF navigation (Dealer.Account was
        // removed — Parties.Domain must not reference Accounting.Domain, see the GeneralLedger
        // migration report). The Dealer query handlers patch them in after mapping, resolved
        // through IRepository<Accounting.Domain.Account> (an already-accepted Application-layer
        // cross-module read — see ModuleLayerDependencyTests).
        CreateMap<Dealer, DealerDto>()
        .ForMember(dest => dest.DealerGroupName, opt => opt.MapFrom(src => src.DealerGroup.Name))
        .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.Country.Name))
        .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City.Name))
        .ForMember(dest => dest.AccountCode, opt => opt.Ignore())
        .ForMember(dest => dest.AccountName, opt => opt.Ignore())
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
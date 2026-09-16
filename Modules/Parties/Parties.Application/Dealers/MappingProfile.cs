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
        // through Accounting.Contracts.Accounts.GetAccountQuery/GetAccountLookupsQuery — no direct
        // reference to Accounting.Domain or Accounting.Application.
        CreateMap<Dealer, DealerDto>()
        .ForMember(dest => dest.DealerGroupName, opt => opt.MapFrom(src => src.DealerGroup != null ? src.DealerGroup.Name : null))
        .ForMember(dest => dest.CountryName, opt => opt.Ignore())
        .ForMember(dest => dest.CityName, opt => opt.Ignore())
        .ForMember(dest => dest.AccountCode, opt => opt.Ignore())
        .ForMember(dest => dest.AccountName, opt => opt.Ignore())
        .ForMember(dest => dest.DistrictName, opt => opt.Ignore());
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
namespace MasterData.Application;

﻿using MasterData.Application.Currencies.Commands;
using AutoMapper;

public partial class MappingProfile : Profile
{
    public void CurrencyMappingProfile()
    {
        #region Currency
        CreateMap<Currency, CurrencyDto>();
        CreateMap<CurrencyDto, Currency>();

        CreateMap<Currency, CreateCurrencyCommand>();
        CreateMap<CreateCurrencyCommand, Currency>();
        CreateMap<Currency, UpdateCurrencyCommand>();
        CreateMap<UpdateCurrencyCommand, Currency>();
        CreateMap<Currency, DeleteCurrencyCommand>();
        CreateMap<DeleteCurrencyCommand, Currency>();

        CreateMap<CurrencyDto, CreateCurrencyCommand>();
        CreateMap<CreateCurrencyCommand, CurrencyDto>();
        CreateMap<CurrencyDto, UpdateCurrencyCommand>();
        CreateMap<UpdateCurrencyCommand, CurrencyDto>();
        #endregion
    }
}

namespace Treasury.Application;

﻿using Treasury.Application.Banks.Commands;
using AutoMapper;

public partial class MappingProfile : Profile
{
    public void BankMappingProfile()
    {
        #region Bank
        CreateMap<Bank, BankDto>();
        CreateMap<BankDto, Bank>();

        CreateMap<Bank, CreateBankCommand>();
        CreateMap<CreateBankCommand, Bank>();
        CreateMap<Bank, UpdateBankCommand>();
        CreateMap<UpdateBankCommand, Bank>();
        CreateMap<Bank, DeleteBankCommand>();
        CreateMap<DeleteBankCommand, Bank>();

        CreateMap<BankDto, CreateBankCommand>();
        CreateMap<CreateBankCommand, BankDto>();
        CreateMap<BankDto, UpdateBankCommand>();
        CreateMap<UpdateBankCommand, BankDto>();
        #endregion
    }
}
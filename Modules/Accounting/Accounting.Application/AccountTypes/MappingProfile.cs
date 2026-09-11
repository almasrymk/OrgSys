namespace Accounting.Application;

﻿using Accounting.Application.AccountTypes.Commands;
using AutoMapper;

public partial class MappingProfile : Profile
{
    public void AccountTypeMappingProfile()
    {
        #region AccountType
        CreateMap<AccountType, AccountTypeDto>();
        CreateMap<AccountTypeDto, AccountType>();

        CreateMap<AccountType, CreateAccountTypeCommand>();
        CreateMap<CreateAccountTypeCommand, AccountType>();
        CreateMap<AccountType, UpdateAccountTypeCommand>();
        CreateMap<UpdateAccountTypeCommand, AccountType>();
        CreateMap<AccountType, DeleteAccountTypeCommand>();
        CreateMap<DeleteAccountTypeCommand, AccountType>();

        CreateMap<AccountTypeDto, CreateAccountTypeCommand>();
        CreateMap<CreateAccountTypeCommand, AccountTypeDto>();
        CreateMap<AccountTypeDto, UpdateAccountTypeCommand>();
        CreateMap<UpdateAccountTypeCommand, AccountTypeDto>();
        #endregion
    }
}
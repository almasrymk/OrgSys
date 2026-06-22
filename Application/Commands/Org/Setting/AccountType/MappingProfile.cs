using Application.Commands.Org.Setting.AccountType.Commands;
using AutoMapper;
using Domain.Entities;
using Application.DTOs;

public partial class MappingProfile : Profile
{
    public void AccountTypeMappingProfile()
    {
        #region AccountType
        CreateMap<AccountType, AccountTypeModelView>();
        CreateMap<AccountTypeModelView, AccountType>();

        CreateMap<AccountType, CreateAccountTypeCommand>();
        CreateMap<CreateAccountTypeCommand, AccountType>();
        CreateMap<AccountType, UpdateAccountTypeCommand>();
        CreateMap<UpdateAccountTypeCommand, AccountType>();
        CreateMap<AccountType, DeleteAccountTypeCommand>();
        CreateMap<DeleteAccountTypeCommand, AccountType>();

        CreateMap<AccountTypeModelView, CreateAccountTypeCommand>();
        CreateMap<CreateAccountTypeCommand, AccountTypeModelView>();
        CreateMap<AccountTypeModelView, UpdateAccountTypeCommand>();
        CreateMap<UpdateAccountTypeCommand, AccountTypeModelView>();
        #endregion
    }
}
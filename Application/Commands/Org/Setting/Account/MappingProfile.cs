using Application.Commands.Org.Setting.Account.Commands;
using AutoMapper;
using Domain.Entities;
using Application.DTOs;

public partial class MappingProfile : Profile
{
    public void AccountMappingProfile()
    {
        #region Account
        CreateMap<Account, AccountModelView>()
        .ForMember(dest => dest.AccountTypeName, opt => opt.MapFrom(src => src.AccountType.Name));
        CreateMap<AccountModelView, Account>();

        CreateMap<Account, CreateAccountCommand>();
        CreateMap<CreateAccountCommand, Account>();
        CreateMap<Account, UpdateAccountCommand>();
        CreateMap<UpdateAccountCommand, Account>();
        CreateMap<Account, DeleteAccountCommand>();
        CreateMap<DeleteAccountCommand, Account>();

        CreateMap<AccountModelView, CreateAccountCommand>();
        CreateMap<CreateAccountCommand, AccountModelView>();
        CreateMap<AccountModelView, UpdateAccountCommand>();
        CreateMap<UpdateAccountCommand, AccountModelView>();
        #endregion
    }
}
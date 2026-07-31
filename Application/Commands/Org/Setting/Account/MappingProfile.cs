using Application.Commands.Org.Setting.Account.Commands;
using AutoMapper;
using Domain.Entities;
using Application.DTOs;

public partial class MappingProfile : Profile
{
    public void AccountMappingProfile()
    {
        #region Account
        CreateMap<Account, AccountDto>()
        .ForMember(dest => dest.AccountTypeName, opt => opt.MapFrom(src => src.AccountType!.Name));
        CreateMap<AccountDto, Account>();

        CreateMap<Account, CreateAccountCommand>();
        CreateMap<CreateAccountCommand, Account>();
        CreateMap<Account, UpdateAccountCommand>();
        CreateMap<UpdateAccountCommand, Account>();
        CreateMap<Account, DeleteAccountCommand>();
        CreateMap<DeleteAccountCommand, Account>();

        CreateMap<AccountDto, CreateAccountCommand>();
        CreateMap<CreateAccountCommand, AccountDto>();
        CreateMap<AccountDto, UpdateAccountCommand>();
        CreateMap<UpdateAccountCommand, AccountDto>();
        #endregion
    }
}
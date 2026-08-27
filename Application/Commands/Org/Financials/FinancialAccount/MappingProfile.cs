using Application.Commands.Org.Financials.FinancialAccount.Commands;
using AutoMapper;
using Application.DTOs;

public partial class MappingProfile : Profile
{
    public void FinancialAccountMappingProfile()
    {
        #region FinancialAccount
        CreateMap<Domain.Entities.FinancialAccount, FinancialAccountDto>()
            .ForMember(dest => dest.TypeId, opt => opt.MapFrom(src => (long)src.FinancialAccountType))
            .ForMember(dest => dest.AccountName, opt => opt.MapFrom(src => src.Account != null ? src.Account.Name : null))
            .ForMember(dest => dest.AccountCode, opt => opt.MapFrom(src => src.Account != null ? src.Account.Code : null))
            .ForMember(dest => dest.CurrencyName, opt => opt.MapFrom(src => src.Currency != null ? src.Currency.Name : null))
            .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.CashBox != null ? src.CashBox.BranchId : null))
            .ForMember(dest => dest.KeeperUserId, opt => opt.MapFrom(src => src.CashBox != null ? src.CashBox.KeeperUserId : null))
            .ForMember(dest => dest.BankId, opt => opt.MapFrom(src => src.BankAccount != null ? (long?)src.BankAccount.BankId : null))
            .ForMember(dest => dest.BankName, opt => opt.MapFrom(src => src.BankAccount != null && src.BankAccount.Bank != null ? src.BankAccount.Bank.Name : null))
            .ForMember(dest => dest.BankBranchId, opt => opt.MapFrom(src => src.BankAccount != null ? src.BankAccount.BankBranchd : null))
            .ForMember(dest => dest.BankBranchDisplayName, opt => opt.MapFrom(src => src.BankAccount != null && src.BankAccount.BankBranch != null ? src.BankAccount.BankBranch.Name : null))
            .ForMember(dest => dest.AccountNumber, opt => opt.MapFrom(src => src.BankAccount != null ? src.BankAccount.AccountNumber : null))
            .ForMember(dest => dest.IBAN, opt => opt.MapFrom(src => src.BankAccount != null ? src.BankAccount.IBAN : null))
            .ForMember(dest => dest.SwiftCode, opt => opt.MapFrom(src => src.BankAccount != null ? src.BankAccount.SwiftCode : null))
            .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.BankAccount != null ? src.BankAccount.BranchName : null));
        CreateMap<FinancialAccountDto, Domain.Entities.FinancialAccount>();

        CreateMap<Domain.Entities.FinancialAccount, CreateFinancialAccountCommand>();
        CreateMap<CreateFinancialAccountCommand, Domain.Entities.FinancialAccount>();
        CreateMap<Domain.Entities.FinancialAccount, UpdateFinancialAccountCommand>();
        CreateMap<UpdateFinancialAccountCommand, Domain.Entities.FinancialAccount>();

        CreateMap<FinancialAccountDto, CreateFinancialAccountCommand>();
        CreateMap<CreateFinancialAccountCommand, FinancialAccountDto>();
        CreateMap<FinancialAccountDto, UpdateFinancialAccountCommand>();
        CreateMap<UpdateFinancialAccountCommand, FinancialAccountDto>();
        #endregion
    }
}

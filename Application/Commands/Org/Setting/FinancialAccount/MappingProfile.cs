using Application.Commands.Org.Setting.FinancialAccount.Commands;
using AutoMapper;
using Application.DTOs;
using Domain.Enums;

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
            .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.CashBox != null ? src.CashBox.BranchId : (src.BankAccount != null ? src.BankAccount.BranchId : null)))
            .ForMember(dest => dest.KeeperUserId, opt => opt.MapFrom(src => src.CashBox != null ? src.CashBox.KeeperUserId : null))
            .ForMember(dest => dest.BankId, opt => opt.MapFrom(src => src.BankAccount != null ? (long?)src.BankAccount.BankId : null))
            .ForMember(dest => dest.BankName, opt => opt.MapFrom(src => src.BankAccount != null && src.BankAccount.Bank != null ? src.BankAccount.Bank.Name : null))
            .ForMember(dest => dest.BankBranchId, opt => opt.MapFrom(src => src.BankAccount != null ? src.BankAccount.BankBranchd : null))
            .ForMember(dest => dest.BankBranchDisplayName, opt => opt.MapFrom(src => src.BankAccount != null && src.BankAccount.BankBranch != null ? src.BankAccount.BankBranch.Name : null))
            .ForMember(dest => dest.AccountNumber, opt => opt.MapFrom(src => src.BankAccount != null ? src.BankAccount.AccountNumber : null))
            .ForMember(dest => dest.IBAN, opt => opt.MapFrom(src => src.BankAccount != null ? src.BankAccount.IBAN : null))
            .ForMember(dest => dest.SwiftCode, opt => opt.MapFrom(src => src.BankAccount != null ? src.BankAccount.SwiftCode : null));
        CreateMap<FinancialAccountDto, Domain.Entities.FinancialAccount>();

        CreateMap<Domain.Entities.FinancialAccount, CreateFinancialAccountCommand>();
        // Unlike UpdateFinancialAccountCommand (which goes through SaveDetials to update the existing
        // CashBox/BankAccount row by its own Id), Create has no existing detail row to reconcile against,
        // so the generic CreateCommandHandler<> base's plain mapper.Map<FinancialAccount>(request) needs the
        // detail row attached to the graph here — EF cascades the insert once the header gets its Id.
        CreateMap<CreateFinancialAccountCommand, Domain.Entities.FinancialAccount>()
            .ForMember(dest => dest.CashBox, opt => opt.MapFrom(src => src.FinancialAccountType == FinancialAccountType.CashBox
                ? new Domain.Entities.CashBox { Name = src.Name, AccountId = src.AccountId, BranchId = src.BranchId, KeeperUserId = src.KeeperUserId }
                : null))
            .ForMember(dest => dest.BankAccount, opt => opt.MapFrom(src => src.FinancialAccountType == FinancialAccountType.Bank
                ? new Domain.Entities.BankAccount
                {
                    Name = src.Name,
                    AccountId = src.AccountId,
                    BankId = src.BankId ?? 0,
                    BankBranchd = src.BankBranchId,
                    AccountNumber = src.AccountNumber,
                    IBAN = src.IBAN,
                    SwiftCode = src.SwiftCode,
                    BranchId = src.BranchId
                }
                : null));
        CreateMap<Domain.Entities.FinancialAccount, UpdateFinancialAccountCommand>();
        CreateMap<UpdateFinancialAccountCommand, Domain.Entities.FinancialAccount>();

        CreateMap<FinancialAccountDto, CreateFinancialAccountCommand>();
        CreateMap<CreateFinancialAccountCommand, FinancialAccountDto>();
        CreateMap<FinancialAccountDto, UpdateFinancialAccountCommand>();
        CreateMap<UpdateFinancialAccountCommand, FinancialAccountDto>();
        #endregion
    }
}

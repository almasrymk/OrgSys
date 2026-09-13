namespace Treasury.Application;

using Treasury.Application.FinancialAccounts.Commands;
using AutoMapper;

public partial class MappingProfile : Profile
{
    public void FinancialAccountMappingProfile()
    {
        #region FinancialAccount
        // AccountName/AccountCode are no longer populated via an EF navigation
        // (FinancialAccount.Account was removed — Treasury.Domain must not reference
        // Accounting.Domain, see the GeneralLedger migration report). The FinancialAccount query
        // handlers patch them in after mapping, resolved through Accounting.Contracts.Accounts.
        // GetAccountQuery — no Accounting.Domain/Accounting.Application reference at all.
        CreateMap<Treasury.Domain.FinancialAccount, FinancialAccountDto>()
            .ForMember(dest => dest.TypeId, opt => opt.MapFrom(src => (long)src.FinancialAccountType))
            .ForMember(dest => dest.AccountName, opt => opt.Ignore())
            .ForMember(dest => dest.AccountCode, opt => opt.Ignore())
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
        CreateMap<FinancialAccountDto, Treasury.Domain.FinancialAccount>();

        CreateMap<Treasury.Domain.FinancialAccount, CreateFinancialAccountCommand>();
        // Unlike UpdateFinancialAccountCommand (which goes through SaveDetials to update the existing
        // CashBox/BankAccount row by its own Id), Create has no existing detail row to reconcile against,
        // so the generic CreateCommandHandler<> base's plain mapper.Map<FinancialAccount>(request) needs the
        // detail row attached to the graph here — EF cascades the insert once the header gets its Id.
        CreateMap<CreateFinancialAccountCommand, Treasury.Domain.FinancialAccount>()
            .ForMember(dest => dest.CashBox, opt => opt.MapFrom(src => src.FinancialAccountType == FinancialAccountType.CashBox
                ? new Treasury.Domain.CashBox { Name = src.Name, AccountId = src.AccountId, BranchId = src.BranchId, KeeperUserId = src.KeeperUserId }
                : null))
            .ForMember(dest => dest.BankAccount, opt => opt.MapFrom(src => src.FinancialAccountType == FinancialAccountType.Bank
                ? new Treasury.Domain.BankAccount
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
        CreateMap<Treasury.Domain.FinancialAccount, UpdateFinancialAccountCommand>();
        CreateMap<UpdateFinancialAccountCommand, Treasury.Domain.FinancialAccount>();

        CreateMap<FinancialAccountDto, CreateFinancialAccountCommand>();
        CreateMap<CreateFinancialAccountCommand, FinancialAccountDto>();
        CreateMap<FinancialAccountDto, UpdateFinancialAccountCommand>();
        CreateMap<UpdateFinancialAccountCommand, FinancialAccountDto>();
        #endregion
    }
}

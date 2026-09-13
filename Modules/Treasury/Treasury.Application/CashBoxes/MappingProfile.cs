namespace Treasury.Application;

using Treasury.Application.CashBoxes.Commands;
using AutoMapper;

public partial class MappingProfile : Profile
{
    public void CashBoxMappingProfile()
    {
        #region CashBox
        // AccountName was never actually populated even before this: none of this screen's query
        // handlers ever Include()d the Account navigation, so src.Account was always null here
        // (AutoMapper's safe-navigation silently mapped that to a null AccountName) — a
        // pre-existing, unrelated gap this migration does not fix, only makes explicit. See
        // Treasury.Domain/Entities/CashBox.cs — CashBox.Account was removed (Treasury.Domain must
        // not reference Accounting.Domain, see the GeneralLedger migration report).
        CreateMap<CashBox, CashBoxDto>()
        .ForMember(dest => dest.AccountName, opt => opt.Ignore());

        CreateMap<CashBoxDto, CashBox>();

        CreateMap<CashBox, CreateCashBoxCommand>();
        CreateMap<CreateCashBoxCommand, CashBox>();
        CreateMap<CashBox, UpdateCashBoxCommand>();
        CreateMap<UpdateCashBoxCommand, CashBox>();
        CreateMap<CashBox, DeleteCashBoxCommand>();
        CreateMap<DeleteCashBoxCommand, CashBox>();

        CreateMap<CashBoxDto, CreateCashBoxCommand>();
        CreateMap<CreateCashBoxCommand, CashBoxDto>();
        CreateMap<CashBoxDto, UpdateCashBoxCommand>();
        CreateMap<UpdateCashBoxCommand, CashBoxDto>();
        #endregion
    }
}

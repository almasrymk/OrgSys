using AutoMapper;
using Entity.Model;
using Entity.ModelView;
using Application.Commands.Org.Setting.Currency.Commands;

public partial class MappingProfile : Profile
{
    public void CurrencyMappingProfile()
    {
        #region Currency
        CreateMap<Currency, CurrencyModelView>();
        CreateMap<CurrencyModelView, Currency>();       
        #endregion
    }
}
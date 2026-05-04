using Application.Commands.Org.Setting.Currency.Commands;
using AutoMapper;
using Entity.Model;
using Entity.ModelView;

public partial class MappingProfile : Profile
{
    public void CurrencyMappingProfile()
    {
        #region Currency
        CreateMap<Currency, CurrencyModelView>();
        CreateMap<CurrencyModelView, Currency>();

        CreateMap<Currency, CreateCurrencyCommand>();
        CreateMap<CreateCurrencyCommand, Currency>();
        CreateMap<Currency, UpdateCurrencyCommand>();
        CreateMap<UpdateCurrencyCommand, Currency>();
        CreateMap<Currency, DeleteCurrencyCommand>();
        CreateMap<DeleteCurrencyCommand, Currency>();

        CreateMap<CurrencyModelView, CreateCurrencyCommand>();
        CreateMap<CreateCurrencyCommand, CurrencyModelView>();
        CreateMap<CurrencyModelView, UpdateCurrencyCommand>();
        CreateMap<UpdateCurrencyCommand, CurrencyModelView>();
        #endregion
    }
}
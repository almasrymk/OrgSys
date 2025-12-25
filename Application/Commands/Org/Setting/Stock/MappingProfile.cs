using AutoMapper;
using Entity.Model;
using Entity.ModelView;
using Application.Commands.Org.Setting.Stock.Commands;

public partial class MappingProfile : Profile
{
    public void StockMappingProfile()
    {
        #region Stock
        CreateMap<Stock, StockModelView>();
        CreateMap<StockModelView, Stock>();       
        #endregion
    }
}
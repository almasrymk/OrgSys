using Application.Commands.Org.Setting.Stock.Commands;
using AutoMapper;
using Domain.Entities;
using Application.DTOs;

public partial class MappingProfile : Profile
{
    public void StockMappingProfile()
    {
        #region Stock
        CreateMap<Stock, StockModelView>()
        .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch.Name));
        CreateMap<StockModelView, Stock>();

        CreateMap<Stock, CreateStockCommand>();
        CreateMap<CreateStockCommand, Stock>();
        CreateMap<Stock, UpdateStockCommand>();
        CreateMap<UpdateStockCommand, Stock>();
        CreateMap<Stock, DeleteStockCommand>();
        CreateMap<DeleteStockCommand, Stock>();

        CreateMap<StockModelView, CreateStockCommand>();
        CreateMap<CreateStockCommand, StockModelView>();
        CreateMap<StockModelView, UpdateStockCommand>();
        CreateMap<UpdateStockCommand, StockModelView>();
        #endregion
    }
}
using Application.Commands.Org.Setting.Stock.Commands;
using AutoMapper;
using Domain.Entities;
using Application.DTOs;

public partial class MappingProfile : Profile
{
    public void StockMappingProfile()
    {
        #region Stock
        CreateMap<Stock, StockDto>()
        .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch.Name))
        .ForMember(dest => dest.AccountName, opt => opt.MapFrom(src => src.Account.Name));
        CreateMap<StockDto, Stock>();

        CreateMap<Stock, CreateStockCommand>();
        CreateMap<CreateStockCommand, Stock>();
        CreateMap<Stock, UpdateStockCommand>();
        CreateMap<UpdateStockCommand, Stock>();
        CreateMap<Stock, DeleteStockCommand>();
        CreateMap<DeleteStockCommand, Stock>();

        CreateMap<StockDto, CreateStockCommand>();
        CreateMap<CreateStockCommand, StockDto>();
        CreateMap<StockDto, UpdateStockCommand>();
        CreateMap<UpdateStockCommand, StockDto>();
        #endregion
    }
}

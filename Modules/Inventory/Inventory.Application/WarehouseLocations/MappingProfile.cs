namespace Inventory.Application;

using Inventory.Application.WarehouseLocations.Commands;
using AutoMapper;

public partial class MappingProfile
{
    public void WarehouseLocationMappingProfile()
    {
        CreateMap<WarehouseLocation, WarehouseLocationDto>()
            .ForMember(dest => dest.StockName, opt => opt.MapFrom(src => src.Stock != null ? src.Stock.Name : null));
        CreateMap<WarehouseLocationDto, WarehouseLocation>();

        CreateMap<CreateWarehouseLocationCommand, WarehouseLocation>();
        CreateMap<WarehouseLocation, CreateWarehouseLocationCommand>();
    }
}

namespace Catalog.Application;

﻿using Catalog.Application.Products.Commands;
using AutoMapper;

public partial class MappingProfile : Profile
{
    public void ProductMappingProfile()
    {
        #region Product
        CreateMap<Product, ProductDto>()
        .ForMember(dest => dest.ClassificationName, opt => opt.MapFrom(src => src.Classification.Name))
        .ForMember(dest => dest.DealerName, opt => opt.MapFrom(src => src.Dealer.Name))
        .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.Name))
        .ForMember(dest => dest.ProductUnits, opt => opt.MapFrom(src => src.ProductUnits))
        .ForMember(dest => dest.ProductPropertyElements, opt => opt.MapFrom(src => src.ProductPropertyElements));
        CreateMap<ProductDto, Product>()
        .ForMember(dest => dest.ProductUnits, opt => opt.MapFrom(src => src.ProductUnits))
        .ForMember(dest => dest.ProductPropertyElements, opt => opt.MapFrom(src => src.ProductPropertyElements));

        CreateMap<Product, CreateProductCommand>();
        CreateMap<CreateProductCommand, Product>()
        .ForMember(dest => dest.ProductUnits, opt => opt.MapFrom(src => src.ProductUnits))
        .ForMember(dest => dest.ProductPropertyElements, opt => opt.MapFrom(src => src.ProductPropertyElements));

        CreateMap<Product, UpdateProductCommand>()
        .ForMember(dest => dest.ClassificationName, opt => opt.MapFrom(src => src.Classification.Name))
        .ForMember(dest => dest.DealerName, opt => opt.MapFrom(src => src.Dealer.Name))
        .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.Name))
        .ForMember(dest => dest.ProductUnits, opt => opt.MapFrom(src => src.ProductUnits))
        .ForMember(dest => dest.ProductPropertyElements, opt => opt.MapFrom(src => src.ProductPropertyElements));

        CreateMap<UpdateProductCommand, Product>()
        .ForMember(dest => dest.ProductUnits, opt => opt.MapFrom(src => src.ProductUnits))
        .ForMember(dest => dest.ProductPropertyElements, opt => opt.MapFrom(src => src.ProductPropertyElements));

        CreateMap<Product, DeleteProductCommand>();
        CreateMap<DeleteProductCommand, Product>();

        CreateMap<ProductDto, CreateProductCommand>();
        CreateMap<CreateProductCommand, ProductDto>();
        CreateMap<ProductDto, UpdateProductCommand>();
        CreateMap<UpdateProductCommand, ProductDto>();
        #endregion
    }
}

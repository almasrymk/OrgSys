namespace Inventory.Application;

﻿using Inventory.Application.Products.Commands;
using AutoMapper;

public partial class MappingProfile : Profile
{
    public void ProductMappingProfile()
    {
        #region Product              
        CreateMap<Product, ProductDto>()
        .ForMember(dest => dest.ClassificationName, opt => opt.MapFrom(src => src.Classification.Name))
        .ForMember(dest => dest.DealerName, opt => opt.MapFrom(src => src.Dealer.Name))
        .ForMember(dest => dest.ProductUnits, opt => opt.MapFrom(src => src.ProductUnits))        
        .ForMember(dest => dest.ProductPropertyElements, opt => opt.MapFrom(src => src.ProductPropertyElements))
        .ForMember(dest => dest.ProductRecipes, opt => opt.MapFrom(src => src.ProductRecipes));
        CreateMap<ProductDto, Product>()
        .ForMember(dest => dest.ProductUnits, opt => opt.MapFrom(src => src.ProductUnits))
        .ForMember(dest => dest.ProductPropertyElements, opt => opt.MapFrom(src => src.ProductPropertyElements))
        .ForMember(dest => dest.ProductRecipes, opt => opt.MapFrom(src => src.ProductRecipes));
        
        CreateMap<Product, CreateProductCommand>();
        CreateMap<CreateProductCommand, Product>()
        .ForMember(dest => dest.ProductUnits, opt => opt.MapFrom(src => src.ProductUnits))
        .ForMember(dest => dest.ProductPropertyElements, opt => opt.MapFrom(src => src.ProductPropertyElements))
        .ForMember(dest => dest.ProductRecipes, opt => opt.MapFrom(src => src.ProductRecipes));

        CreateMap<Product, UpdateProductCommand>()
        .ForMember(dest => dest.ClassificationName, opt => opt.MapFrom(src => src.Classification.Name))
        .ForMember(dest => dest.DealerName, opt => opt.MapFrom(src => src.Dealer.Name))
        .ForMember(dest => dest.ProductUnits, opt => opt.MapFrom(src => src.ProductUnits))
        .ForMember(dest => dest.ProductPropertyElements, opt => opt.MapFrom(src => src.ProductPropertyElements))
        .ForMember(dest => dest.ProductRecipes, opt => opt.MapFrom(src => src.ProductRecipes));

        CreateMap<UpdateProductCommand, Product>()
        .ForMember(dest => dest.ProductUnits, opt => opt.MapFrom(src => src.ProductUnits))
        .ForMember(dest => dest.ProductPropertyElements, opt => opt.MapFrom(src => src.ProductPropertyElements))
        .ForMember(dest => dest.ProductRecipes, opt => opt.MapFrom(src => src.ProductRecipes));

        CreateMap<Product, DeleteProductCommand>();
        CreateMap<DeleteProductCommand, Product>();

        CreateMap<ProductDto, CreateProductCommand>();
        CreateMap<CreateProductCommand, ProductDto>();
        CreateMap<ProductDto, UpdateProductCommand>();
        CreateMap<UpdateProductCommand, ProductDto>();
        #endregion
    }
}
using Application.Commands.Org.Setting.Product.Commands;
using AutoMapper;
using Entity.Model;
using Entity.ModelView;

public partial class MappingProfile : Profile
{
    public void ProductMappingProfile()
    {
        #region Product              
        CreateMap<Product, ProductModelView>()
        .ForMember(dest => dest.ClassificationName, opt => opt.MapFrom(src => src.Classification.Name))
        .ForMember(dest => dest.DealerName, opt => opt.MapFrom(src => src.Dealer.Name))
        .ForMember(dest => dest.ProductUnits, opt => opt.MapFrom(src => src.ProductUnits))        
        .ForMember(dest => dest.ProductPropertyElements, opt => opt.MapFrom(src => src.ProductPropertyElements))
        .ForMember(dest => dest.ProductRecipes, opt => opt.MapFrom(src => src.ProductRecipes));
        CreateMap<ProductModelView, Product>()
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

        CreateMap<ProductModelView, CreateProductCommand>();
        CreateMap<CreateProductCommand, ProductModelView>();
        CreateMap<ProductModelView, UpdateProductCommand>();
        CreateMap<UpdateProductCommand, ProductModelView>();
        #endregion
    }
}
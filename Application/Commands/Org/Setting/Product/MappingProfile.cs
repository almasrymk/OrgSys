using AutoMapper;
using Entity.Model;
using Entity.ModelView;
using Application.Commands.Org.Setting.Product.Commands;

public partial class MappingProfile : Profile
{
    public void ProductMappingProfile()
    {
        #region Product
        CreateMap<Product, ProductModelView>();
        CreateMap<ProductModelView, Product>();       
        #endregion
    }
}
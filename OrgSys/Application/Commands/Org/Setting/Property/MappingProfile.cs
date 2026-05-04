using AutoMapper;
using Entity.Model;
using Entity.ModelView;
using Application.Commands.Org.Setting.Property.Commands;

public partial class MappingProfile : Profile
{
    public void PropertyMappingProfile()
    {
        #region Property
        CreateMap<Property, PropertyModelView>();
        CreateMap<PropertyModelView, Property>();       
        #endregion
    }
}
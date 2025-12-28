using AutoMapper;
using Entity.Model;
using Entity.ModelView;
using Application.Commands.Org.Setting.Classification.Commands;

public partial class MappingProfile : Profile
{
    public void ClassificationMappingProfile()
    {
        #region Classification
        CreateMap<Classification, ClassificationModelView>();
        CreateMap<ClassificationModelView, Classification>();       
        #endregion
    }
}
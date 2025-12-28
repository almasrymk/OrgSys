using AutoMapper;
using Entity.Model;
using Entity.ModelView;
using Application.Commands.Org.Setting.District.Commands;

public partial class MappingProfile : Profile
{
    public void DistrictMappingProfile()
    {
        #region District
        CreateMap<District, DistrictModelView>();
        CreateMap<DistrictModelView, District>();       
        #endregion
    }
}
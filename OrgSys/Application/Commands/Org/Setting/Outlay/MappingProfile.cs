using AutoMapper;
using Entity.Model;
using Entity.ModelView;
using Application.Commands.Org.Setting.Outlay.Commands;

public partial class MappingProfile : Profile
{
    public void OutlayMappingProfile()
    {
        #region Outlay
        CreateMap<Outlay, OutlayModelView>();
        CreateMap<OutlayModelView, Outlay>();       
        #endregion
    }
}
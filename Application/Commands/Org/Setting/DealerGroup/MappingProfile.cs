using AutoMapper;
using Entity.Model;
using Entity.ModelView;
using Application.Commands.Org.Setting.DealerGroup.Commands;

public partial class MappingProfile : Profile
{
    public void DealerGroupMappingProfile()
    {
        #region DealerGroup
        CreateMap<DealerGroup, DealerGroupModelView>();
        CreateMap<DealerGroupModelView, DealerGroup>();       
        #endregion
    }
}
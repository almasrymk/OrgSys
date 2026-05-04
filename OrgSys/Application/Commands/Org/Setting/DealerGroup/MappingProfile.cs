using Application.Commands.Org.Setting.DealerGroup.Commands;
using AutoMapper;
using Entity.Model;
using Entity.ModelView;

public partial class MappingProfile : Profile
{
    public void DealerGroupMappingProfile()
    {
        #region DealerGroup
        CreateMap<DealerGroup, DealerGroupModelView>();
        CreateMap<DealerGroupModelView, DealerGroup>();

        CreateMap<DealerGroup, CreateDealerGroupCommand>();
        CreateMap<CreateDealerGroupCommand, DealerGroup>();
        CreateMap<DealerGroup, UpdateDealerGroupCommand>();
        CreateMap<UpdateDealerGroupCommand, DealerGroup>();
        CreateMap<DealerGroup, DeleteDealerGroupCommand>();
        CreateMap<DeleteDealerGroupCommand, DealerGroup>();

        CreateMap<DealerGroupModelView, CreateDealerGroupCommand>();
        CreateMap<CreateDealerGroupCommand, DealerGroupModelView>();
        CreateMap<DealerGroupModelView, UpdateDealerGroupCommand>();
        CreateMap<UpdateDealerGroupCommand, DealerGroupModelView>();
        #endregion
    }
}
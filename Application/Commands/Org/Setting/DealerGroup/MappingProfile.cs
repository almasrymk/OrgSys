using Application.Commands.Org.Setting.DealerGroup.Commands;
using AutoMapper;
using Domain.Entities;
using Application.DTOs;

public partial class MappingProfile : Profile
{
    public void DealerGroupMappingProfile()
    {
        #region DealerGroup
        CreateMap<DealerGroup, DealerGroupDto>();
        CreateMap<DealerGroupDto, DealerGroup>();

        CreateMap<DealerGroup, CreateDealerGroupCommand>();
        CreateMap<CreateDealerGroupCommand, DealerGroup>();
        CreateMap<DealerGroup, UpdateDealerGroupCommand>();
        CreateMap<UpdateDealerGroupCommand, DealerGroup>();
        CreateMap<DealerGroup, DeleteDealerGroupCommand>();
        CreateMap<DeleteDealerGroupCommand, DealerGroup>();

        CreateMap<DealerGroupDto, CreateDealerGroupCommand>();
        CreateMap<CreateDealerGroupCommand, DealerGroupDto>();
        CreateMap<DealerGroupDto, UpdateDealerGroupCommand>();
        CreateMap<UpdateDealerGroupCommand, DealerGroupDto>();
        #endregion
    }
}
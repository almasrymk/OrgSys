namespace SaaS.Application;

using SaaS.Application.Plans.Commands;
using AutoMapper;

public partial class MappingProfile
{
    public void PlanMappingProfile()
    {
        #region Plan
        CreateMap<Plan, PlanDto>();
        CreateMap<PlanDto, Plan>();

        CreateMap<Plan, CreatePlanCommand>();
        CreateMap<CreatePlanCommand, Plan>();
        CreateMap<Plan, UpdatePlanCommand>();
        CreateMap<UpdatePlanCommand, Plan>();

        CreateMap<PlanDto, CreatePlanCommand>();
        CreateMap<CreatePlanCommand, PlanDto>();
        CreateMap<PlanDto, UpdatePlanCommand>();
        CreateMap<UpdatePlanCommand, PlanDto>();
        #endregion
    }
}

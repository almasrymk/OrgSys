namespace SaaS.Application;

using AutoMapper;

public partial class MappingProfile : Profile
{
    public MappingProfile()
    {
        TenantMappingProfile();
        PlanMappingProfile();
        FeatureMappingProfile();
        SubscriptionMappingProfile();
    }
}

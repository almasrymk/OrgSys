namespace SaaS.Application;

using AutoMapper;

public partial class MappingProfile
{
    public void SubscriptionMappingProfile()
    {
        #region Subscription
        CreateMap<Subscription, SubscriptionDto>();
        CreateMap<SubscriptionDto, Subscription>();
        #endregion
    }
}

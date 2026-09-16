namespace SaaS.Domain;

public enum BillingPeriod
{
    Monthly = 1,
    Yearly = 2
}

/// <summary>Subscription lifecycle (brief §56). Distinct from TenantLifecycleStatus: a Tenant can be
/// Active while its Subscription is PastDue, for example — Tenant tracks the account, Subscription
/// tracks the billing record.</summary>
public enum SubscriptionStatus
{
    Trial = 1,
    Active = 2,
    PastDue = 3,
    Suspended = 4,
    Cancelled = 5,
    Expired = 6
}

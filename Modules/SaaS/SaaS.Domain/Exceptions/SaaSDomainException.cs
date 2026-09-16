namespace SaaS.Domain.Exceptions;

/// <summary>Base type for a SaaS-lifecycle invariant violation. The Application layer catches these
/// at the command-handler boundary and translates them into OrgSys.SharedKernel.Result/Error, same
/// convention as Accounting.Domain.Exceptions.AccountingDomainException.</summary>
public abstract class SaaSDomainException(string message) : Exception(message);

/// <summary>A Tenant lifecycle transition was attempted that isn't valid from its current TenantLifecycleStatus (e.g. Activate on an already-Cancelled tenant).</summary>
public sealed class InvalidTenantTransitionException(string message) : SaaSDomainException(message);

/// <summary>An operation requiring an Active tenant (e.g. creating a Subscription) was attempted against a Suspended/Cancelled tenant.</summary>
public sealed class TenantNotActiveException(string message) : SaaSDomainException(message);

/// <summary>A Subscription lifecycle transition was attempted that isn't valid from its current SubscriptionStatus.</summary>
public sealed class InvalidSubscriptionTransitionException(string message) : SaaSDomainException(message);

namespace SaaS.Contracts.Features;

/// <summary>
/// Centralized feature/limit enforcement (brief §55/§58) — every module checks entitlement through
/// this single abstraction instead of scattering "if (plan == ...)" checks. Registered by
/// SaaS.Infrastructure's AddSaaSModule(), implemented in SaaS.Application. Backend is the source of
/// truth (brief §58) — this must be called from the Application/API layer of the *consuming*
/// module before performing a gated operation, never trusted from the frontend alone.
/// </summary>
public interface ITenantFeatureService
{
    Task<bool> IsFeatureEnabledAsync(long tenantId, string featureKey, CancellationToken cancellationToken = default);

    /// <summary>Checks a Plan numeric limit (MaxUsers/MaxCompanies/MaxBranches/MaxWarehouses/
    /// MaxTransactionsPerMonth) against a caller-supplied current usage count. A null limit on the
    /// Plan means unlimited — always returns true in that case.</summary>
    Task<bool> IsWithinLimitAsync(long tenantId, TenantLimit limit, int currentCount, CancellationToken cancellationToken = default);
}

public enum TenantLimit
{
    Users,
    Companies,
    Branches,
    Warehouses,
    TransactionsPerMonth
}

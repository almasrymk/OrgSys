namespace SaaS.Application.Features
{
    using OrgSys.SharedKernel;
    using SaaS.Contracts.Features;

    /// <summary>Implements SaaS.Contracts.Features.ITenantFeatureService (brief §55/§58) — the single
    /// place every module's feature/limit checks go through, instead of scattering "if (plan ==
    /// ...)" logic. Resolves the Tenant's current usable Subscription (Trial/Active/PastDue) →
    /// Plan, same "most recent usable row wins" convention as Organization's GetDefaultCompanyQuery.</summary>
    public sealed class TenantFeatureService(
        IRepository<Subscription> subscriptionRepository,
        IRepository<PlanFeature> planFeatureRepository,
        IRepository<Feature> featureRepository,
        IRepository<Plan> planRepository) : ITenantFeatureService
    {
        private async Task<Plan?> GetActivePlanAsync(long tenantId, CancellationToken cancellationToken)
        {
            var subscriptions = await subscriptionRepository.GetListByFilterAsync(e => e.TenantId == tenantId);
            var active = (subscriptions ?? [])
                .Where(s => s.SubscriptionStatus is SubscriptionStatus.Trial or SubscriptionStatus.Active or SubscriptionStatus.PastDue)
                .OrderByDescending(s => s.Id)
                .FirstOrDefault();

            if (active is null)
                return null;

            return await planRepository.GetByFilterAsync(e => e.Id == active.PlanId, string.Empty);
        }

        public async Task<bool> IsFeatureEnabledAsync(long tenantId, string featureKey, CancellationToken cancellationToken = default)
        {
            var plan = await GetActivePlanAsync(tenantId, cancellationToken);
            if (plan is null)
                return false;

            var planFeatures = await planFeatureRepository.GetListByFilterAsync(e => e.PlanId == plan.Id);
            var featureIds = (planFeatures ?? []).Select(e => e.FeatureId).ToList();
            if (featureIds.Count == 0)
                return false;

            return await featureRepository.AnyAsync(e => featureIds.Contains(e.Id) && e.Key == featureKey, cancellationToken);
        }

        public async Task<bool> IsWithinLimitAsync(long tenantId, TenantLimit limit, int currentCount, CancellationToken cancellationToken = default)
        {
            var plan = await GetActivePlanAsync(tenantId, cancellationToken);
            if (plan is null)
                return false;

            int? max = limit switch
            {
                TenantLimit.Users => plan.MaxUsers,
                TenantLimit.Companies => plan.MaxCompanies,
                TenantLimit.Branches => plan.MaxBranches,
                TenantLimit.Warehouses => plan.MaxWarehouses,
                TenantLimit.TransactionsPerMonth => plan.MaxTransactionsPerMonth,
                _ => null
            };

            return max is null || currentCount < max.Value;
        }
    }
}

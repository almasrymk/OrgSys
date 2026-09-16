namespace SaaS.Application.PlanFeatures.Queries
{
    using OrgSys.SharedKernel;
    using System.Net;

    public sealed record GetFeaturesForPlanQuery(long PlanId) : IQuery<List<string>>;

    /// <summary>Returns the Feature.Key list a Plan entitles a Tenant to — the read model
    /// TenantFeatureService.IsFeatureEnabledAsync is built on.</summary>
    public sealed class GetFeaturesForPlanQueryHandler(IRepository<PlanFeature> planFeatureRepository, IRepository<Feature> featureRepository) : IQueryHandler<GetFeaturesForPlanQuery, List<string>>
    {
        public async Task<Result<List<string>>> Handle(GetFeaturesForPlanQuery request, CancellationToken cancellationToken)
        {
            var planFeatures = await planFeatureRepository.GetListByFilterAsync(e => e.PlanId == request.PlanId);
            var featureIds = (planFeatures ?? []).Select(e => e.FeatureId).Distinct().ToList();
            if (featureIds.Count == 0)
                return new Result<List<string>>(HttpStatusCode.OK, [], null);

            var features = await featureRepository.GetListByFilterAsync(e => featureIds.Contains(e.Id));
            var keys = (features ?? []).Select(e => e.Key).ToList();

            return new Result<List<string>>(HttpStatusCode.OK, keys, null);
        }
    }
}

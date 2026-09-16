namespace SaaS.Domain
{
    /// <summary>Join entity: which Features a Plan entitles a Tenant to (brief §54). Real EF
    /// navigation to Plan/Feature — both are intra-module (SaaS.Domain → SaaS.Domain), not a
    /// cross-module reference.</summary>
    [Table("PlanFeature")]
    public class PlanFeature : BaseModel
    {
        [ForeignKey("Plan")]
        public virtual long PlanId { get; set; }

        public virtual Plan? Plan { get; set; }

        [ForeignKey("Feature")]
        public virtual long FeatureId { get; set; }

        public virtual Feature? Feature { get; set; }
    }
}

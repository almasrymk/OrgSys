namespace SaaS.Domain
{
    /// <summary>Commercial plan (brief §53). Limits are nullable — null means unlimited, matching this
    /// codebase's existing "nullable scalar means unset/no constraint" convention (e.g. Branch.CompanyId
    /// before the Organization pass). Feature entitlement is a separate concern, see PlanFeature.</summary>
    [Table("Plan")]
    public class Plan : BaseModel
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public virtual string Name { get; set; } = null!;

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Price { get; set; }

        public virtual BillingPeriod BillingPeriod { get; set; } = BillingPeriod.Monthly;

        public virtual int? MaxUsers { get; set; }

        public virtual int? MaxCompanies { get; set; }

        public virtual int? MaxBranches { get; set; }

        public virtual int? MaxWarehouses { get; set; }

        public virtual int? MaxTransactionsPerMonth { get; set; }

        public virtual ICollection<PlanFeature> PlanFeatures { get; set; } = new List<PlanFeature>();
    }
}

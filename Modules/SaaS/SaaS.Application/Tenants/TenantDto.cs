namespace SaaS.Application
{
    using System.ComponentModel.DataAnnotations;

    public class TenantDto : BaseModel
    {
        [StringLength(150, MinimumLength = 2)]
        public string? Name { get; set; }

        public TenantLifecycleStatus TenantStatus { get; set; }

        public DateTime? TrialEndsAt { get; set; }

        public DateTime? ActivatedAt { get; set; }

        public DateTime? SuspendedAt { get; set; }
    }
}

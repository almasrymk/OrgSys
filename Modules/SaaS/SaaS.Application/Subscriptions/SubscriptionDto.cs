namespace SaaS.Application
{
    public class SubscriptionDto : BaseModel
    {
        public long TenantId { get; set; }

        public long PlanId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public SubscriptionStatus SubscriptionStatus { get; set; }

        public DateTime? TrialEndsAt { get; set; }

        public DateTime? RenewalDate { get; set; }
    }
}

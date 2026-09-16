namespace SaaS.Domain
{
    using SaaS.Domain.Exceptions;

    /// <summary>
    /// A Tenant's billing record against a Plan (brief §56) — the authoritative "what plan is this
    /// tenant on right now" source (see Tenant.cs for why Tenant itself doesn't duplicate PlanId).
    /// Entirely separate from ERP tenant-facing documents (Sales/CommercialDocuments.Invoice) per
    /// brief §57 — this is OrgSys's own revenue record, never mixed with a tenant's own Customer
    /// invoices.
    /// </summary>
    [Table("Subscription")]
    public class Subscription : BaseModel
    {
        [ForeignKey("Tenant")]
        public virtual long TenantId { get; set; }

        public virtual Tenant? Tenant { get; set; }

        [ForeignKey("Plan")]
        public virtual long PlanId { get; set; }

        public virtual Plan? Plan { get; set; }

        public virtual DateTime StartDate { get; set; }

        public virtual DateTime? EndDate { get; set; }

        public virtual SubscriptionStatus SubscriptionStatus { get; set; } = SubscriptionStatus.Trial;

        public virtual DateTime? TrialEndsAt { get; set; }

        public virtual DateTime? RenewalDate { get; set; }

        public void Activate()
        {
            if (SubscriptionStatus is SubscriptionStatus.Cancelled or SubscriptionStatus.Expired)
                throw new InvalidSubscriptionTransitionException($"Subscription {Id} is {SubscriptionStatus} and cannot be activated.");

            SubscriptionStatus = SubscriptionStatus.Active;
        }

        /// <summary>brief §71 ChangePlan — moves this subscription to a new Plan without losing
        /// history (the Subscription row itself is the history; OrgSys.SharedKernel has no generic
        /// audit trail to also write to, see docs/architecture/remaining-contexts-current-state.md §1
        /// "Auditing").</summary>
        public void ChangePlan(long newPlanId)
        {
            if (SubscriptionStatus is SubscriptionStatus.Cancelled or SubscriptionStatus.Expired)
                throw new InvalidSubscriptionTransitionException($"Subscription {Id} is {SubscriptionStatus} and cannot change plan.");

            PlanId = newPlanId;
        }

        public void MarkPastDue()
        {
            if (SubscriptionStatus != SubscriptionStatus.Active)
                throw new InvalidSubscriptionTransitionException($"Subscription {Id} must be Active to be marked PastDue (was {SubscriptionStatus}).");

            SubscriptionStatus = SubscriptionStatus.PastDue;
        }

        public void Cancel(DateTime nowUtc)
        {
            if (SubscriptionStatus == SubscriptionStatus.Cancelled)
                throw new InvalidSubscriptionTransitionException($"Subscription {Id} is already Cancelled.");

            SubscriptionStatus = SubscriptionStatus.Cancelled;
            EndDate = nowUtc;
        }

        public void Expire(DateTime nowUtc)
        {
            if (SubscriptionStatus is SubscriptionStatus.Cancelled or SubscriptionStatus.Expired)
                throw new InvalidSubscriptionTransitionException($"Subscription {Id} is already {SubscriptionStatus}.");

            SubscriptionStatus = SubscriptionStatus.Expired;
            EndDate = nowUtc;
        }

        public bool IsUsable() => SubscriptionStatus is SubscriptionStatus.Trial or SubscriptionStatus.Active or SubscriptionStatus.PastDue;
    }
}

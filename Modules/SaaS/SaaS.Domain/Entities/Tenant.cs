namespace SaaS.Domain
{
    using SaaS.Domain.Exceptions;

    /// <summary>
    /// OrgSys's own SaaS customer (brief §51) — top of the Tenant → Company → Branch hierarchy
    /// (see docs/architecture/adr/tenant-vs-company.md). A Tenant does NOT carry a PlanId directly:
    /// the brief's suggested shape lists one, but this design keeps the active commercial
    /// relationship exclusively on Subscription (Tenant → active Subscription → Plan) to avoid two
    /// competing sources of truth for "what plan is this tenant on" — a documented, deliberate
    /// simplification, not an oversight.
    /// </summary>
    [Table("Tenant")]
    public class Tenant : BaseModel
    {
        [Required]
        [StringLength(150, MinimumLength = 2)]
        public virtual string Name { get; set; } = null!;

        public virtual TenantLifecycleStatus TenantStatus { get; set; } = TenantLifecycleStatus.Trial;

        public virtual DateTime? TrialEndsAt { get; set; }

        public virtual DateTime? ActivatedAt { get; set; }

        public virtual DateTime? SuspendedAt { get; set; }

        /// <summary>Trial → Active (brief §51). A Cancelled tenant can never be reactivated — a new
        /// Tenant must be provisioned instead, matching how OrgSys treats every other terminal
        /// lifecycle state (e.g. Journal.Cancel is likewise a dead end, see Accounting.Domain.Journal).</summary>
        public void Activate(DateTime nowUtc)
        {
            if (TenantStatus == TenantLifecycleStatus.Cancelled)
                throw new InvalidTenantTransitionException($"Tenant '{Name}' is Cancelled and cannot be reactivated.");

            TenantStatus = TenantLifecycleStatus.Active;
            ActivatedAt = nowUtc;
            SuspendedAt = null;
        }

        /// <summary>Active/Trial → Suspended. Idempotent-safe: suspending an already-Suspended tenant is a no-op error, matching JournalAlreadyPostedException's "reject repeat transition" convention.</summary>
        public void Suspend(DateTime nowUtc)
        {
            if (TenantStatus is TenantLifecycleStatus.Cancelled)
                throw new InvalidTenantTransitionException($"Tenant '{Name}' is Cancelled and cannot be suspended.");
            if (TenantStatus == TenantLifecycleStatus.Suspended)
                throw new InvalidTenantTransitionException($"Tenant '{Name}' is already Suspended.");

            TenantStatus = TenantLifecycleStatus.Suspended;
            SuspendedAt = nowUtc;
        }

        /// <summary>Any state → Cancelled. Terminal — see Activate().</summary>
        public void Cancel()
        {
            if (TenantStatus == TenantLifecycleStatus.Cancelled)
                throw new InvalidTenantTransitionException($"Tenant '{Name}' is already Cancelled.");

            TenantStatus = TenantLifecycleStatus.Cancelled;
        }

        /// <summary>Final-authority check for any operation that requires a usable tenant (creating a
        /// Company, logging in, etc.) — mirrors FiscalYear.EnsureOpenForPosting's role.</summary>
        public void EnsureActive()
        {
            if (TenantStatus is TenantLifecycleStatus.Suspended or TenantLifecycleStatus.Cancelled)
                throw new TenantNotActiveException($"Tenant '{Name}' is {TenantStatus} and cannot be used.");
        }
    }
}

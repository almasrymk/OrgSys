namespace SaaS.Domain;

/// <summary>OrgSys's own SaaS-customer lifecycle (brief §51) — unrelated to OrgSys.SharedKernel.Status
/// (BaseModel's generic New/Deleted/... workflow status), which Tenant keeps separately for its
/// row-level state (e.g. Deleted) per this codebase's established convention (see FiscalYearStatus
/// for the precedent of a domain-specific status enum living alongside BaseModel.Status).</summary>
public enum TenantLifecycleStatus
{
    Trial = 1,
    Active = 2,
    Suspended = 3,
    Cancelled = 4
}

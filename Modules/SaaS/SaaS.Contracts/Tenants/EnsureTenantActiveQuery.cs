namespace SaaS.Contracts.Tenants;

using OrgSys.SharedKernel;

/// <summary>Synchronous Contracts call other modules use to gate a request on tenant usability
/// (brief §31 "no module accesses another module's internals" + brief's "use a synchronous
/// Contracts call when the caller needs the result in the same request" guidance from
/// docs/modular-monolith-target-architecture.md §5). Returns true only if the Tenant exists and is
/// Trial/Active (not Suspended/Cancelled).</summary>
public record EnsureTenantActiveQuery(long TenantId) : IQuery<bool>;

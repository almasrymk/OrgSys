namespace Domain.Abstraction
{
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// The named DbSet&lt;T&gt; properties this interface used to declare (one per entity, spanning
    /// every module) were dead: Repository&lt;T&gt; and UnitOfWork — the only two consumers of
    /// IOrgContext — use only Set&lt;TEntity&gt;() and the members below. Removing them means this
    /// interface (and therefore Domain.csproj) no longer needs a reference to every module's
    /// Domain project just to declare a property nothing reads — which is what let Treasury.Domain
    /// reference Domain.csproj (for Dealer/Invoice navigation) without a cycle. OrgContext itself
    /// keeps its own DbSet properties (EF Core's entity-discovery mechanism) unchanged.
    /// </summary>
    public interface IOrgContext : IDisposable
    {
        void ResetDbContextState();

        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        Task BeginTransactionAsync();

        Task CommitAsync();

        Task RollbackAsync();
    }
}

namespace OrgSys.SharedKernel
{
    public interface IUnitOfWork : IDisposable
    {
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
        Task<int> SaveChangeAsync(CancellationToken cancellationToken = default);
        void ResetDbContextState();
    }
}
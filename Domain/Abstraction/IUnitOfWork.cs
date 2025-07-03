namespace Domain.Abstraction
{
    public interface IUnitOfWork : IDisposable 
    {
        Task<int> SaveChangeAsync(CancellationToken cancellationToken = default);
    }
}
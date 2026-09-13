namespace Infrastructure.Persistence.UnitOfWork
{
    using System.Threading.Tasks;

    public class UnitOfWork(IOrgContext dbContext) : IUnitOfWork
    {
       
        public async Task<int> SaveChangeAsync(CancellationToken cancellationToken = default)
        {
            return await dbContext.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
           
        }

        public Task BeginTransactionAsync()
        {
            return dbContext.BeginTransactionAsync();
        }

        public Task CommitAsync()
        {
            return dbContext.CommitAsync();
        }

        public Task RollbackAsync()
        {
            return dbContext.RollbackAsync();
        }

        public void ResetDbContextState()
        {
            dbContext.ResetDbContextState();
        }
    }
}
namespace Infrastructure.Persistence.UnitOfWork
{
    using Domain.Abstraction;

    public class UnitOfWork(IOrgContext dbContext) : IUnitOfWork
    {
       
        public async Task<int> SaveChangeAsync(CancellationToken cancellationToken = default)
        {
            return dbContext.SaveChangesAsync(cancellationToken).Result;
        }

        public void Dispose()
        {
           
        }
    }
}
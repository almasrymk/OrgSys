namespace Infrastructure.Persistence.UnitOfWork
{
    using Domain.Common.Base;
    using Domain.Abstraction;   
    using System.Linq.Expressions;
    using Microsoft.EntityFrameworkCore;

    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly IOrgContext dbContext;
        private readonly DbSet<TEntity> dbEntity;

        public Repository(IOrgContext _dbContext)
        {
            dbContext = _dbContext;
            dbEntity = dbContext.Set<TEntity>();
        }

        public async ValueTask<bool> Commit()
        {
            if (await dbContext.SaveChangesAsync() > 0)
                return true;
            return false;
        }

        public virtual async ValueTask<TEntity> CreateAsync(TEntity Ob)
        {
            await dbEntity.AddAsync(Ob);
            return Ob;
        }

        public virtual async ValueTask<bool> DeleteAsync(Expression<Func<TEntity, bool>> Filter)
        {
            var Ob = await dbEntity.FirstOrDefaultAsync(Filter);
            if (Ob != null)
            {
                Ob.Status =  Domain.Enums.Status.Deleted;
                dbEntity.Attach(Ob);
                return true;
            }

            return false;
        }

        public virtual async ValueTask<TEntity?> GetByFilterAsync(Expression<Func<TEntity, bool>> Filter)
        {
            return await dbEntity.FirstOrDefaultAsync(Filter);
        }

        public virtual async ValueTask<IEnumerable<TEntity>?> GetListByFilterAsync()
        {
            return await dbEntity.AsQueryable().ToListAsync();
        }

        public virtual async ValueTask<IEnumerable<TEntity>?> GetListByFilterAsync(Expression<Func<TEntity, bool>> Filter)
        {
            return await dbEntity.Where(Filter).AsQueryable().ToListAsync();
        }

        public virtual async ValueTask<bool> ShiftDeleteAsync(Expression<Func<TEntity, bool>> Filter)
        {
            var Ob = await dbEntity.FirstOrDefaultAsync(Filter);
            if (Ob != null)
            {
                dbEntity.Remove(Ob);
                return true;
            }

            return false;
        }

        public virtual async ValueTask<bool> UpdateAsync(TEntity Ob)
        {
            if (await dbEntity.AnyAsync(e => e == Ob))
            {
                dbEntity.Attach(Ob);
                return true;
            }

            return false;
        }
    }
}
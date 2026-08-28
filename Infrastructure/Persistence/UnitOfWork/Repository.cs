namespace Infrastructure.Persistence.UnitOfWork
{
    using CorePagination.Extensions;
    using CorePagination.Paginators.SizeAwarePaginator;
    using Domain.Abstraction;   
    using Microsoft.EntityFrameworkCore;
    using System.Linq.Expressions;
    using static System.Net.WebRequestMethods;

    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Domain.Entities.BaseModel //BaseEntity
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

        public virtual async ValueTask<List<TEntity>> CreateAsync(List<TEntity> Ob)
        {
            await dbEntity.AddRangeAsync(Ob);
            return Ob;
        }

        public virtual async ValueTask<bool> DeleteAsync(Expression<Func<TEntity, bool>> Filter)
        {
            var ObList = dbEntity.Where(Filter);
            if (ObList != null)
            {
                foreach (var Ob in ObList)
                {
                    Ob.Status = Domain.Enums.Status.Deleted;
                    dbEntity.Entry(dbEntity.Find(Ob.Id)!).CurrentValues.SetValues(Ob);
                }
                return true;
            }

            return false;
        }

        public virtual async ValueTask<TResponse> GetMaxByFilterAsync<TResponse>(Expression<Func<TEntity, bool>> Filter , Expression<Func<TEntity, TResponse>> Selector)
        {
            var query = dbEntity.Where(Filter);
            if (!await query.AnyAsync())
                return default!;
            return await query.MaxAsync(Selector);
        }

        public virtual async ValueTask<TEntity?> GetByFilterAsync(Expression<Func<TEntity, bool>> Filter, string includeProperties)
        {
            var query = dbEntity.AsQueryable();
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            return await query.FirstOrDefaultAsync(Filter);
        }

        public virtual async ValueTask<IEnumerable<TEntity>?> GetListByFilterAsync(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, string includeProperties)
        {
            var query = dbEntity.AsQueryable();
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return await orderBy(query.Where(e => e.Status != Domain.Enums.Status.Deleted && e.Hide != true)).AsQueryable().ToListAsync();
            }
            else
            {
                return await query.Where(e => e.Status != Domain.Enums.Status.Deleted && e.Hide != true).AsQueryable().ToListAsync();
            }
        }

        public virtual async ValueTask<IEnumerable<TEntity>?> GetListByFilterAsync(Expression<Func<TEntity, bool>> Filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, string includeProperties, int Page, int PageSize)
        {
            var query = dbEntity.Where(Filter).AsQueryable();
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return await orderBy(query.Where(e => e.Status != Domain.Enums.Status.Deleted && e.Hide != true)).AsQueryable().Skip((Page - 1) * PageSize).Take(PageSize).ToListAsync();
            }
            else
            {
                return await query.Where(e => e.Status != Domain.Enums.Status.Deleted && e.Hide != true).AsQueryable().Skip((Page - 1) * PageSize).Take(PageSize).ToListAsync();
            }
        }

        public virtual async ValueTask<IEnumerable<TEntity>?> GetListByFilterAsync(Expression<Func<TEntity, bool>> Filter)
        {
            var query = dbEntity.Where(Filter).AsQueryable();

            return await query.Where(e => e.Status != Domain.Enums.Status.Deleted && e.Hide != true).AsQueryable().ToListAsync();
        }

        public virtual async ValueTask<IEnumerable<TEntity>?> GetListByFilterAsync(Expression<Func<TEntity, bool>> Filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy)
        {
            var query = dbEntity.Where(Filter).AsQueryable();

            return await query.Where(e => e.Status != Domain.Enums.Status.Deleted && e.Hide != true).AsQueryable().ToListAsync();
        }

        public virtual async ValueTask<IEnumerable<TEntity>?> GetListByFilterAsync(string includeProperties)
        {
            var query = dbEntity.AsQueryable();
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return await query.Where(e => e.Status != Domain.Enums.Status.Deleted && e.Hide != true).AsQueryable().ToListAsync();
        }

        public virtual async ValueTask<IEnumerable<TEntity>?> GetListByFilterAsync(Expression<Func<TEntity, bool>> Filter, string includeProperties)
        {
            var query = dbEntity.Where(Filter).AsQueryable();
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return await query.Where(e => e.Status != Domain.Enums.Status.Deleted && e.Hide != true).AsQueryable().ToListAsync();
        }

        public virtual async ValueTask<SizeAwarePaginationResult<TEntity>?> GetPaginationByFilterAsync(Expression<Func<TEntity, bool>> Filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy , string includeProperties , int Page , int PageSize)
        {
            var query =  dbEntity.Where(Filter);
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return await orderBy(query.Where(e => e.Status != Domain.Enums.Status.Deleted  && e.Hide != true)).AsQueryable().PaginateAsync(Page, PageSize);
            }
            else
            {
                return await query.Where(e => e.Status != Domain.Enums.Status.Deleted  && e.Hide != true).AsQueryable().PaginateAsync(Page, PageSize);
            }
        }

        public virtual async ValueTask<bool> ShiftDeleteAsync(Expression<Func<TEntity, bool>> Filter)
        {
            var Ob = dbEntity.Where(Filter);
            if (Ob != null)
            {
                dbEntity.RemoveRange(Ob);
                return true;
            }

            return false;
        }

        public virtual async ValueTask<bool> UpdateAsync(TEntity Ob)
        {
            if (await dbEntity.AnyAsync(e => e == Ob))
            {
                dbEntity.Entry(dbEntity.Find(Ob.Id)!).CurrentValues.SetValues(Ob);
                return true;
            }

            return false;
        }

        public virtual async ValueTask<bool> AnyAsync(Expression<Func<TEntity, bool>> Filter)
        {
            return await dbEntity.AnyAsync(Filter);
        }

        public virtual async ValueTask<bool> AnyAsync(Expression<Func<TEntity, bool>> Filter, CancellationToken cancellationToken)
        {
            return await dbEntity.AnyAsync(Filter , cancellationToken);
        }

        public virtual async ValueTask<TResponse> GetMaxAsync<TResponse>(Expression<Func<TEntity, TResponse>> Selector)
        {
            return await dbEntity.MaxAsync(Selector);
        }
    }
}
namespace OrgSys.SharedKernel
{
    using System.Linq.Expressions;
    using CorePagination.Paginators.SizeAwarePaginator;

    public interface IRepository<TEntity> where TEntity : BaseModel //BaseEntity
    {
        ValueTask<TEntity> CreateAsync(TEntity Ob);
        ValueTask<List<TEntity>> CreateAsync(List<TEntity> Ob);
        ValueTask<bool> UpdateAsync(TEntity Ob);
        ValueTask<bool> AnyAsync(Expression<Func<TEntity, bool>> Filter);
        ValueTask<bool> AnyAsync(Expression<Func<TEntity, bool>> Filter, CancellationToken cancellationToken);
        ValueTask<TEntity?> GetByFilterAsync(Expression<Func<TEntity, bool>> Filter, string includeProperties);
        ValueTask<TResponse> GetMaxAsync<TResponse>(Expression<Func<TEntity, TResponse>> Selector);
        ValueTask<TResponse> GetMaxByFilterAsync<TResponse>(Expression<Func<TEntity, bool>> Filter, Expression<Func<TEntity, TResponse>> Selector);
        ValueTask<IEnumerable<TEntity>?> GetListByFilterAsync(Expression<Func<TEntity, bool>> Filter);
        ValueTask<IEnumerable<TEntity>?> GetListByFilterAsync(string includeProperties);
        ValueTask<IEnumerable<TEntity>?> GetListByFilterAsync(Expression<Func<TEntity, bool>> Filter, string includeProperties);
        ValueTask<IEnumerable<TEntity>?> GetListByFilterAsync(Expression<Func<TEntity, bool>> Filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy);
        ValueTask<IEnumerable<TEntity>?> GetListByFilterAsync(Expression<Func<TEntity, bool>> Filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, string includeProperties, int Page, int PageSize);
        ValueTask<SizeAwarePaginationResult<TEntity>?> GetPaginationByFilterAsync(Expression<Func<TEntity, bool>> Filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, string includeProperties, int Page, int PageSize);
        ValueTask<bool> DeleteAsync(Expression<Func<TEntity, bool>> Filter);
        ValueTask<bool> ShiftDeleteAsync(Expression<Func<TEntity, bool>> Filter);
        ValueTask<bool> Commit();
    }
}
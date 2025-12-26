namespace Domain.Abstraction
{
    using System.Linq.Expressions;
    using CorePagination.Paginators.SizeAwarePaginator;

    public interface IRepository<TEntity> where TEntity : Entity.BaseModel //BaseEntity
    {
        ValueTask<TEntity> CreateAsync(TEntity Ob);
        ValueTask<bool> UpdateAsync(TEntity Ob);
        ValueTask<TEntity?> GetByFilterAsync(Expression<Func<TEntity, bool>> Filter, string includeProperties);
        ValueTask<TResponse> GetMaxByFilterAsync<TResponse>(Expression<Func<TEntity, bool>> Filter, Expression<Func<TEntity, TResponse>> Selector);
        ValueTask<IEnumerable<TEntity>?> GetListByFilterAsync(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, string includeProperties);
        ValueTask<IEnumerable<TEntity>?> GetListByFilterAsync(Expression<Func<TEntity, bool>> Filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, string includeProperties, int Page, int PageSize);
        ValueTask<SizeAwarePaginationResult<TEntity>?> GetPaginationByFilterAsync(Expression<Func<TEntity, bool>> Filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, string includeProperties, int Page, int PageSize);
        ValueTask<bool> DeleteAsync(Expression<Func<TEntity, bool>> Filter);
        ValueTask<bool> ShiftDeleteAsync(Expression<Func<TEntity, bool>> Filter);
        ValueTask<bool> Commit();
    }
}
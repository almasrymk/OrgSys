namespace Domain.Abstraction
{
    using System.Linq.Expressions;
    using CorePagination.Paginators.SizeAwarePaginator;

    public interface IRepository<TEntity> where TEntity : Entity.BaseModel //BaseEntity
    {
        ValueTask<TEntity> CreateAsync(TEntity Ob);
        ValueTask<bool> UpdateAsync(TEntity Ob);
        ValueTask<TEntity?> GetByFilterAsync(Expression<Func<TEntity, bool>> Filter);
        ValueTask<IEnumerable<TEntity>?> GetListByFilterAsync();
        ValueTask<IEnumerable<TEntity>?> GetListByFilterAsync(Expression<Func<TEntity, bool>> Filter);
        ValueTask<SizeAwarePaginationResult<TEntity>?> GetListByFilterAsync(Expression<Func<TEntity, bool>> Filter, int Page, int PageSize);
        ValueTask<bool> DeleteAsync(Expression<Func<TEntity, bool>> Filter);
        ValueTask<bool> ShiftDeleteAsync(Expression<Func<TEntity, bool>> Filter);
        ValueTask<bool> Commit();
    }
}
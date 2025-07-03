namespace Domain.Abstraction
{
    using System.Linq.Expressions;
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        ValueTask<TEntity> CreateAsync(TEntity Ob);
        ValueTask<bool> UpdateAsync(TEntity Ob);
        ValueTask<TEntity?> GetByFilterAsync(Expression<Func<TEntity, bool>> Filter);
        ValueTask<IEnumerable<TEntity>?> GetListByFilterAsync();
        ValueTask<IEnumerable<TEntity>?> GetListByFilterAsync(Expression<Func<TEntity, bool>> Filter);
        ValueTask<bool> DeleteAsync(Expression<Func<TEntity, bool>> Filter);
        ValueTask<bool> ShiftDeleteAsync(Expression<Func<TEntity, bool>> Filter);
        ValueTask<bool> Commit();
    }
}
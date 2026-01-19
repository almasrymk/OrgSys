using Domain.Abstraction;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Windows.Input;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

public abstract class Validator<TCommand, TEntity>(IRepository<TEntity> _Repository) : AbstractValidator<TCommand>     
    where TEntity : Entity.BaseModel
{     
    public async Task<bool> AnyAsync(Expression< Func<TEntity, bool>> Predicate , CancellationToken cancellationToken = default)       
    {
        return await _Repository.AnyAsync(Predicate, cancellationToken);
    }

    public async Task<bool> NotAnyAsync(Expression<Func<TEntity, bool>> Predicate, CancellationToken cancellationToken = default)
    {
        return !await _Repository.AnyAsync(Predicate, cancellationToken);
    }

    public async Task<bool> IsAllIdsExist(List<long> Ids)
    {
        var existingIds = await _Repository.GetListByFilterAsync(c => Ids.Contains(c.Id));
        return !Ids.Except(existingIds!.Select(e => e.Id)).Any();
    }
}
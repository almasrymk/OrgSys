namespace Application.Common.Commands
{
    using Application.Abstraction.Command;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Microsoft.EntityFrameworkCore;
    using System.Linq.Expressions;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    public class DeleteCommandHandler<TDto, TModel>(IUnitOfWork _UnitOfWork, IRepository<TModel> _Repository, IMapper mapper) : ICommandHandler<TDto>
        where TDto : ICommand
        where TModel : Entity.BaseModel
    {

        public async Task<Result> Handle(TDto request, CancellationToken cancellationToken)
        {
            using (var transaction = _UnitOfWork.BeginTransactionAsync())
            {
                try
                {
                    var res = await _Repository.ShiftDeleteAsync(CreateFilter(request));
                    if (_UnitOfWork.SaveChangeAsync().Result > 0)
                    {
                        await _UnitOfWork.CommitAsync();
                        return new Result(HttpStatusCode.OK, null);
                    }
                    else
                    {
                        var res2 = await _Repository.DeleteAsync(CreateFilter(request));
                        if (_UnitOfWork.SaveChangeAsync().Result > 0)
                        {
                            await _UnitOfWork.CommitAsync();
                            return new Result(HttpStatusCode.OK, null);
                        }
                    }
                     
                    return new Result(
                        HttpStatusCode.InternalServerError,
                        new List<string> { "Error" });
                }
                catch (AggregateException ex)
                {
                    _UnitOfWork.ResetDbContextState();
                    var res2 = await _Repository.DeleteAsync(CreateFilter(request));
                    if (_UnitOfWork.SaveChangeAsync().Result > 0)
                    {
                        await _UnitOfWork.CommitAsync();
                        return new Result(HttpStatusCode.OK, null);
                    }

                    return new Result(
                     HttpStatusCode.InternalServerError,
                     new List<string> { "Error" });
                }
                catch (Exception ex)
                {
                    return new Result(
                        HttpStatusCode.InternalServerError,
                        new List<string> { "Error" });
                }
            }
        }

        public virtual Expression<Func<TModel, bool>> CreateFilter(TDto request)
        {
            return e => true;
        }
    }
}
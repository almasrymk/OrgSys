namespace Application.Common.Commands
{
    using Application.Abstraction.Command;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using System.Linq.Expressions;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    public class DeleteCommandHandler<TDto, TModel>(IUnitOfWork _UnitOfWork, IRepository<TModel> _Repository , IServiceProvider _provider) : ICommandHandler<TDto>
        where TDto : ICommand
        where TModel : Domain.Entities.BaseModel
    {

        public async Task<Result> Handle(TDto request, CancellationToken cancellationToken)
        {
            using (var transaction = _UnitOfWork.BeginTransactionAsync())
            {
                try
                {
                    var resDetails = await RemoveDetails(request);
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
                    new List<Error> { new Error("Error") });
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
                    new List<Error> { new Error( ex.Message) });
                }
                catch (Exception ex)
                {
                    return new Result(
                        HttpStatusCode.InternalServerError,
                    new List<Error> { new Error( ex.Message) });
                }
            }
        }

        public virtual Expression<Func<TModel, bool>> CreateFilter(TDto request)
        {
            return e => true;
        }

        public virtual async Task<bool> RemoveDetails(TDto request)
        {
            return true;
        }
         
        protected async Task<bool> RemoveDetails<TModelDetails>(Expression<Func<TModelDetails , bool>> Filter)
          where TModelDetails : Domain.Entities.BaseModel
        {
            try
            {
                var repository = _provider.GetRequiredService<IRepository<TModelDetails>>();
                var res = await repository.ShiftDeleteAsync(Filter);
                return res;
            }
            catch
            {
                return false;
            }
        }
    }
}
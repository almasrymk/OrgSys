namespace Application.Common.Commands
{
    using Application.Abstraction.Command;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using System.Linq.Expressions;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetMaxCommandHandler<TRequest, TModel>(IRepository<TModel> _Repository, IMapper mapper) : ICommandHandler<TRequest, object>
        where TRequest : ICommand<object>
        where TModel : Entity.BaseModel 
    {
        public virtual async Task<Result<object>> Handle(TRequest request, CancellationToken cancellationToken)
        {
            try
            {                
                var res = await _Repository.GetMaxByFilterAsync(CreateFilter(request), CreateSelector());
                if (res!= null)
                {
                    return new Result<object>(
                    HttpStatusCode.OK,
                    res,
                    null);
                }

                return new Result<object>(
                    HttpStatusCode.InternalServerError,
                    null,
                    new List<string> { "Error" });
            }
            catch (Exception ex)
            {
                return new Result<object>(
                    HttpStatusCode.InternalServerError,
                    null,
                    new List<string> { "Error" });
            }
        }

        public virtual Expression<Func<TModel, bool>> CreateFilter(TRequest request)
        {
            return e => true;
        }

        public virtual Expression<Func<TModel, object>> CreateSelector()
        {
           return null;
        }
    }
}
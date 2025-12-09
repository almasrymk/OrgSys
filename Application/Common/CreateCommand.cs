namespace Application.Common
{
    using System.Net;
    using Domain.Shared;
    using System.Threading;
    using Domain.Abstraction;
    using System.Threading.Tasks;
    using Application.Abstraction.Command;


    public class CreateCommandHandler<TRequest, TModel, TResponse>(IUnitOfWork _UnitOfWork, IRepository<TModel> _Repository) : ICommandHandler<TRequest, TResponse>
        where TRequest : ICommand<TResponse>
        where TModel : Entity.BaseModel 
        where TResponse : Entity.BaseModel
    {
        public virtual async Task<Result<TResponse>> Handle(TRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var ob = GetMapModel(request);

                var res = await _Repository.CreateAsync(ob);
                if (_UnitOfWork.SaveChangeAsync().Result > 0)
                {
                    return new Result<TResponse>(
                    HttpStatusCode.InternalServerError,
                    GetMapResponse(res),
                    null);
                }

                return new Result<TResponse>(
                    HttpStatusCode.InternalServerError,
                    null,
                    new List<string> { "Error" });
            }
            catch (Exception ex)
            {
                return new Result<TResponse>(
                    HttpStatusCode.InternalServerError,
                    null,
                    new List<string> { "Error" });
            }
        }

        public virtual TModel GetMapModel(TRequest request)
        {
            return default!;
        }

        public virtual TResponse GetMapResponse(TModel model)
        {
            return default!;
        }
    }
}
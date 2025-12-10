namespace Application.Common.Commands
{
    using Application.Abstraction.Command;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;


    public class CreateCommandHandler<TRequest, TModel, TResponse>(IUnitOfWork _UnitOfWork, IRepository<TModel> _Repository, IMapper mapper) : ICommandHandler<TRequest, TResponse>
        where TRequest : ICommand<TResponse>
        where TModel : Entity.BaseModel 
        where TResponse : Entity.BaseModel
    {
        public virtual async Task<Result<TResponse>> Handle(TRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var ob = mapper.Map<TModel>(request);

                var res = await _Repository.CreateAsync(ob);
                if (_UnitOfWork.SaveChangeAsync().Result > 0)
                {
                    return new Result<TResponse>(
                    HttpStatusCode.InternalServerError,
                   mapper.Map<TResponse>(ob),
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
    }
}
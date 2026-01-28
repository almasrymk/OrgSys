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

    public class GetCommandHandler<TRequest, TModel, TResponse>(IRepository<TModel> _Repository, IMapper mapper) : ICommandHandler<TRequest, TResponse>
        where TRequest : ICommand<TResponse>
        where TModel : Entity.BaseModel 
        where TResponse : Entity.BaseModel
    {
        public virtual async Task<Result<TResponse>> Handle(TRequest request, CancellationToken cancellationToken)
        {
            try
            {
                TModel? res = await _Repository.GetByFilterAsync(CreateFilter(request), CreateInclude());
                if (res == null || res.Id == 0)
                    res = (TModel)Activator.CreateInstance(typeof(TModel))!;

                return new Result<TResponse>(
                     HttpStatusCode.OK,
                     mapper.Map<TResponse>(res),
                     null);
            }
            catch (Exception ex)
            {
                return new Result<TResponse>(
                    HttpStatusCode.InternalServerError,
                    null,
                    new List<Error> { new Error(ex.Message) });
            }
        }

        public virtual Expression<Func<TModel, bool>> CreateFilter(TRequest request)
        {
            return e => true;
        }

        public virtual string CreateInclude()
        {
            return "";
        }
    }
}
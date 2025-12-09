namespace Application.Common
{
    using System.Net;
    using Domain.Shared;
    using System.Threading;
    using Domain.Abstraction;
    using System.Threading.Tasks;
    using Application.Abstraction.Command;

    public class UpdateCommandHandler<TDto, TModel>(IUnitOfWork _UnitOfWork, IRepository<TModel> _Repository) : ICommandHandler<TDto>
        where TDto : ICommand
        where TModel : Entity.BaseModel 
    {

        public async Task<Result> Handle(TDto request, CancellationToken cancellationToken)
        {
            try
            {
                var ob = GetMapModel(request);

                var res = await _Repository.UpdateAsync(ob);
                if (_UnitOfWork.SaveChangeAsync().Result > 0)
                {
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
        
        public virtual TModel GetMapModel(TDto request)
        {
            return default!;
        } 
    }
}
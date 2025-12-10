namespace Application.Common.Commands
{
    using Application.Abstraction.Command;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    public class DeleteCommandHandler<TDto, TModel>(IUnitOfWork _UnitOfWork, IRepository<TModel> _Repository, IMapper mapper) : ICommandHandler<TDto>
        where TDto : ICommand
        where TModel : Entity.BaseModel
    {

        public async Task<Result> Handle(TDto request, CancellationToken cancellationToken)
        {
            try
            {
                var ob = mapper.Map<TModel>(request);

                var res = await _Repository.DeleteAsync(e => e.Id == ob.Id);
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
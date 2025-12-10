namespace Application.Common.Commands
{
    using System.Net;
    using AutoMapper;
    using Domain.Shared;
    using System.Threading;
    using Domain.Abstraction;
    using System.Threading.Tasks;
    using Application.Abstraction.Command;

    public class CreateCommandHandler<TDto, TModel>(IUnitOfWork _UnitOfWork, IRepository<TModel> _Repository, IMapper mapper) : ICommandHandler<TDto>
        where TDto : ICommand
        where TModel : Entity.BaseModel
    {

        public async Task<Result> Handle(TDto request, CancellationToken cancellationToken)
        {
            try
            {
                var ob = mapper.Map<TModel>(request);

                var res = await _Repository.CreateAsync(ob);
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
    }
}
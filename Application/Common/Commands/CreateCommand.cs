namespace Application.Common.Commands
{
    using Application.Abstraction.Command;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

    public class CreateCommandHandler<TDto, TModel>(IUnitOfWork _UnitOfWork, IRepository<TModel> _Repository, IMapper mapper) : ICommandHandler<TDto>
        where TDto : ICommand
        where TModel : Domain.Entities.BaseModel
    {
        // Set once Handle succeeds, so a subclass override can chain a follow-up command (e.g. an
        // auto-post) against the newly-generated Id without this base Result carrying a payload —
        // ICreateCommand<Result> is shared by every entity's Create, so it can't just be Result<TModel>.
        protected TModel? CreatedEntity { get; private set; }

        public virtual async Task<Result> Handle(TDto request, CancellationToken cancellationToken)
        {
            try
            {
                var ob = mapper.Map<TModel>(request);

                var res = await _Repository.CreateAsync(ob);
                if (await _UnitOfWork.SaveChangeAsync(cancellationToken) > 0)
                {
                    CreatedEntity = res;
                    return new Result(HttpStatusCode.OK, null);
                }

                return new Result(
                    HttpStatusCode.InternalServerError,
                    new List<Error> { new Error("Error") });
            }
            catch (Exception ex)
            {
                return new Result(
                    HttpStatusCode.InternalServerError,
                    new List<Error> { new Error(ex.Message) });
            }
        }
    }
}
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
        where TModel : Entity.BaseModel        
    {

        public virtual async Task<Result> Handle(TDto request, CancellationToken cancellationToken)
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
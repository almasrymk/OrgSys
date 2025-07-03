namespace Application.Common
{
    using System.Net;
    using Domain.Shared;
    using Domain.Entities;
    using System.Threading;
    using Domain.Common.Base;
    using Domain.Abstraction;
    using System.Threading.Tasks;
    using Application.Abstraction.Command;
    using Application.Commands.Org.City.Create;

    //public class CreateCommandHandler<TEntity, TCommand , TResponse>(IUnitOfWork _UnitOfWork, IRepository<TEntity> _Repository) : ICommandHandler<TCommand, Result<TResponse>> where TEntity : BaseEntity where TCommand : ICommand<TResponse> where TResponse : class
    //{
    //    public virtual async Task<Result<CreateCommandResponse>> Handle(CreateCommand request, CancellationToken cancellationToken)
    //    {
    //        try
    //        {
    //            var ob = new City
    //            {
    //                Id = new Guid(),
    //                Name = request.Name,
    //                Status = 0
    //            };

    //            var res = await _Repository.CreateAsync(ob);
    //            if (await _UnitOfWork.SaveChangeAsync() > 0)
    //            {
    //                return new Result<CreateCommandResponse>(
    //                    HttpStatusCode.OK,
    //                    new CreateCommandResponse(res.Id, res.Name!),
    //                    null);
    //            }

    //            return new Result<CreateCommandResponse>(
    //                HttpStatusCode.InternalServerError,
    //                null,
    //                new List<string> { "Error" });
    //        }
    //        catch (Exception ex)
    //        {
    //            throw;
    //        }
    //    }
    //}

}

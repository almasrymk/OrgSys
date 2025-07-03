namespace Application.Commands.Org.City.Create
{
    using System.Net;
    using Domain.Shared;  
    using Domain.Entities;
    using System.Threading;
    using Domain.Abstraction;
    using System.Threading.Tasks;
    using Application.Abstraction.Command;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<City> _Repository) : ICommandHandler<CreateCommand, CreateCommandResponse>
    {
        public async Task<Result<CreateCommandResponse>> Handle(CreateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var ob = new City
                {
                    Id = new Guid(),
                    Name = request.Name,
                    Status = 0
                };

                var res = await _Repository.CreateAsync(ob);
                if (await _UnitOfWork.SaveChangeAsync() > 0)
                {
                    return new Result<CreateCommandResponse>(
                        HttpStatusCode.OK,
                        new CreateCommandResponse(res.Id, res.Name!),
                        null);
                }

                return new Result<CreateCommandResponse>(
                    HttpStatusCode.InternalServerError,
                    null,
                    new List<string> { "Error" });
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
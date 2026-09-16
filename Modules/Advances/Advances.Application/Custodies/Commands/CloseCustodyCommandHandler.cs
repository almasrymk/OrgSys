namespace Advances.Application.Custodies.Commands;

using Advances.Domain.Exceptions;
using System.Net;

public sealed record CloseCustodyCommand(long Id) : ICommand;

public sealed class CloseCustodyCommandHandler(IRepository<Custody> repository, IUnitOfWork unitOfWork)
    : ICommandHandler<CloseCustodyCommand>
{
    public async Task<Result> Handle(CloseCustodyCommand request, CancellationToken cancellationToken)
    {
        var custody = await repository.GetByFilterAsync(e => e.Id == request.Id && e.Status != Status.Deleted, string.Empty);
        if (custody is null)
            return new Result(HttpStatusCode.NotFound, [new Error("Custody not found.")]);

        try
        {
            custody.Close();
            await repository.UpdateAsync(custody);
            await unitOfWork.SaveChangeAsync(cancellationToken);
            return new Result(HttpStatusCode.OK, null);
        }
        catch (CustodyDomainException ex)
        {
            return new Result(HttpStatusCode.BadRequest, [new Error(ex.Message)]);
        }
    }
}

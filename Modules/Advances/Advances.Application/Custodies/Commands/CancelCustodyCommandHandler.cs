namespace Advances.Application.Custodies.Commands;

using Advances.Domain.Exceptions;
using System.Net;

public sealed record CancelCustodyCommand(long Id) : ICommand;

public sealed class CancelCustodyCommandHandler(IRepository<Custody> repository, IUnitOfWork unitOfWork)
    : ICommandHandler<CancelCustodyCommand>
{
    public async Task<Result> Handle(CancelCustodyCommand request, CancellationToken cancellationToken)
    {
        var custody = await repository.GetByFilterAsync(e => e.Id == request.Id && e.Status != Status.Deleted, string.Empty);
        if (custody is null)
            return new Result(HttpStatusCode.NotFound, [new Error("Custody not found.")]);

        try
        {
            custody.Cancel();
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

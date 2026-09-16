namespace Advances.Application.Custodies.Commands;

using Advances.Domain.Exceptions;
using System.Net;

public sealed record TransferCustodyCommand(long Id, long ToHolderId, string Reason, long ApprovedByUserId, DateTime TransferDate) : ICommand;

public sealed class TransferCustodyCommandHandler(IRepository<Custody> repository, IUnitOfWork unitOfWork)
    : ICommandHandler<TransferCustodyCommand>
{
    public async Task<Result> Handle(TransferCustodyCommand request, CancellationToken cancellationToken)
    {
        var custody = await repository.GetByFilterAsync(e => e.Id == request.Id && e.Status != Status.Deleted, "Handovers");
        if (custody is null)
            return new Result(HttpStatusCode.NotFound, [new Error("Custody not found.")]);

        try
        {
            custody.TransferHolder(
                request.ToHolderId,
                request.Reason,
                request.ApprovedByUserId,
                request.TransferDate == default ? DateTime.Now : request.TransferDate);
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

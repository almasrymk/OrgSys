namespace Inventory.Application.InventoryIssues.Commands;

using System.Net;

public sealed record CancelInventoryIssueCommand(long InventoryIssueId) : ICommand;

public sealed class CancelInventoryIssueCommandHandler(
    IRepository<InventoryIssue> issueRepository, IUnitOfWork unitOfWork) : ICommandHandler<CancelInventoryIssueCommand>
{
    public async Task<Result> Handle(CancelInventoryIssueCommand request, CancellationToken cancellationToken)
    {
        var issue = await issueRepository.GetByFilterAsync(r => r.Id == request.InventoryIssueId, "");
        if (issue is null)
            return new Result(HttpStatusCode.NotFound, [new Error("Inventory issue not found.")]);

        try
        {
            issue.Cancel();
            await issueRepository.UpdateAsync(issue);
            await unitOfWork.SaveChangeAsync(cancellationToken);
            return new Result(HttpStatusCode.OK, null);
        }
        catch (InventoryDomainException ex)
        {
            return new Result(HttpStatusCode.BadRequest, [new Error(ex.Message)]);
        }
    }
}

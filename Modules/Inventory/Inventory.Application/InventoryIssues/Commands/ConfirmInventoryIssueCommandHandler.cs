namespace Inventory.Application.InventoryIssues.Commands;

using System.Net;

public sealed record ConfirmInventoryIssueCommand(long InventoryIssueId) : ICommand;

public sealed class ConfirmInventoryIssueCommandHandler(
    IRepository<InventoryIssue> issueRepository, IUnitOfWork unitOfWork) : ICommandHandler<ConfirmInventoryIssueCommand>
{
    public async Task<Result> Handle(ConfirmInventoryIssueCommand request, CancellationToken cancellationToken)
    {
        var issue = await issueRepository.GetByFilterAsync(r => r.Id == request.InventoryIssueId, "Lines");
        if (issue is null)
            return new Result(HttpStatusCode.NotFound, [new Error("Inventory issue not found.")]);

        try
        {
            issue.Confirm();
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

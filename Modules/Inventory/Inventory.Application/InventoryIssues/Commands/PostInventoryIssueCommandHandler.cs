namespace Inventory.Application.InventoryIssues.Commands;

using Inventory.Application.Postings;
using System.Net;

public sealed record PostInventoryIssueCommand(long InventoryIssueId) : ICommand;

public sealed class PostInventoryIssueCommandHandler(
    IRepository<InventoryIssue> issueRepository,
    InventoryLedgerPoster poster,
    IUnitOfWork unitOfWork) : ICommandHandler<PostInventoryIssueCommand>
{
    public async Task<Result> Handle(PostInventoryIssueCommand request, CancellationToken cancellationToken)
    {
        var issue = await issueRepository.GetByFilterAsync(r => r.Id == request.InventoryIssueId, "Lines");
        if (issue is null)
            return new Result(HttpStatusCode.NotFound, [new Error("Inventory issue not found.")]);

        await unitOfWork.BeginTransactionAsync();
        try
        {
            issue.Post(DateTime.Now);
            await poster.PostIssueAsync(issue, cancellationToken);

            await issueRepository.UpdateAsync(issue);
            await unitOfWork.SaveChangeAsync(cancellationToken);
            await unitOfWork.CommitAsync();
            return new Result(HttpStatusCode.OK, null);
        }
        catch (InventoryDomainException ex)
        {
            await unitOfWork.RollbackAsync();
            return new Result(HttpStatusCode.BadRequest, [new Error(ex.Message)]);
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();
            return new Result(HttpStatusCode.InternalServerError, [new Error(ex.Message)]);
        }
    }
}

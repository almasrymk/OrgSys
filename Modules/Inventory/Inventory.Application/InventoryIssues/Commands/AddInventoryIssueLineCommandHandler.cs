namespace Inventory.Application.InventoryIssues.Commands;

using System.Net;

public sealed record AddInventoryIssueLineCommand(
    long InventoryIssueId, long ProductId, long UnitId, decimal Quantity, long? BatchId, long? SerialId, string? Notes) : ICommand;

public sealed class AddInventoryIssueLineCommandHandler(
    IRepository<InventoryIssue> issueRepository, IUnitOfWork unitOfWork) : ICommandHandler<AddInventoryIssueLineCommand>
{
    public async Task<Result> Handle(AddInventoryIssueLineCommand request, CancellationToken cancellationToken)
    {
        var issue = await issueRepository.GetByFilterAsync(r => r.Id == request.InventoryIssueId, "Lines");
        if (issue is null)
            return new Result(HttpStatusCode.NotFound, [new Error("Inventory issue not found.")]);

        try
        {
            issue.AddLine(request.ProductId, request.UnitId, request.Quantity, request.BatchId, request.SerialId, request.Notes);
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

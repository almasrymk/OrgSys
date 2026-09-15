namespace Inventory.Application.InventoryIssues.Commands;

using System.Net;

public sealed record InventoryIssueLineInput(long ProductId, long UnitId, decimal Quantity, long? BatchId, long? SerialId, string? Notes);

public sealed record CreateInventoryIssueCommand(
    long StockId, long? LocationId, long? DealerId, DateTime Date, long CreateUserId, long? BranchId,
    string? Notes, List<InventoryIssueLineInput> Lines) : ICommand<long>;

public sealed class CreateInventoryIssueCommandHandler(
    IRepository<InventoryIssue> issueRepository, IUnitOfWork unitOfWork) : ICommandHandler<CreateInventoryIssueCommand, long>
{
    public async Task<Result<long>> Handle(CreateInventoryIssueCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var issue = InventoryIssue.Create(
                request.StockId, request.LocationId, request.DealerId, request.Date,
                request.CreateUserId, DateTime.Now, request.BranchId, request.Notes);

            foreach (var line in request.Lines)
                issue.AddLine(line.ProductId, line.UnitId, line.Quantity, line.BatchId, line.SerialId, line.Notes);

            await issueRepository.CreateAsync(issue);
            await unitOfWork.SaveChangeAsync(cancellationToken);

            return new Result<long>(HttpStatusCode.OK, issue.Id, null);
        }
        catch (InventoryDomainException ex)
        {
            return new Result<long>(HttpStatusCode.BadRequest, 0, [new Error(ex.Message)]);
        }
    }
}

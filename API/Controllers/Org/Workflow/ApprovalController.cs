using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Workflow.Contracts.Approvals;

namespace API.Controllers.Org.Workflow;

[Authorize]
[ApiController]
[Route("[controller]")]
public class ApprovalController(ISender sender) : ControllerBase
{
    [HttpPost("Start")]
    public Task<Result> Start([FromBody] StartApprovalCommand command, CancellationToken cancellationToken) =>
        sender.Send(command, cancellationToken);

    [HttpPost("Decide")]
    public Task<Result> Decide([FromBody] DecideApprovalCommand command, CancellationToken cancellationToken) =>
        sender.Send(command, cancellationToken);

    [HttpGet("GetByDocument")]
    public Task<Result<ApprovalRequestDto?>> GetByDocument(WorkflowDocumentTypeCode documentType, long documentId, CancellationToken cancellationToken) =>
        sender.Send(new GetApprovalByDocumentQuery(documentType, documentId), cancellationToken);
}

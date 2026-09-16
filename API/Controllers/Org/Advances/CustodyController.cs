using Advances.Application.Custodies.Commands;
using Advances.Application.Custodies.Queries;
using Advances.Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrgSys.SharedKernel;

namespace API.Controllers.Org.Advances;

[Authorize]
[ApiController]
[Route("[controller]")]
public sealed class CustodyController(ISender sender) : ControllerBase
{
    [HttpPost]
    public Task<Result<long>> Create(CreateCustodyCommand command, CancellationToken cancellationToken) =>
        sender.Send(command, cancellationToken);

    [HttpGet("{id:long}")]
    public Task<Result<CustodyDto>> GetById(long id, CancellationToken cancellationToken) =>
        sender.Send(new GetCustodyByIdQuery(id), cancellationToken);

    [HttpGet]
    public Task<ResultCollection<CustodyDto>> GetList(long? holderId, CustodyStatus? status, CancellationToken cancellationToken) =>
        sender.Send(new GetCustodyListQuery(holderId, status), cancellationToken);

    [HttpPut("{id:long}/Approve")]
    public Task<Result> Approve(long id, CancellationToken cancellationToken) =>
        sender.Send(new ApproveCustodyCommand(id), cancellationToken);

    [HttpPut("{id:long}/Issue")]
    public Task<Result> Issue(long id, [FromBody] IssueCustodyCommand command, CancellationToken cancellationToken) =>
        sender.Send(command with { Id = id }, cancellationToken);

    [HttpPut("{id:long}/Settle")]
    public Task<Result> Settle(long id, [FromBody] SettleCustodyCommand command, CancellationToken cancellationToken) =>
        sender.Send(command with { Id = id }, cancellationToken);

    [HttpPut("{id:long}/Return")]
    public Task<Result> Return(long id, [FromBody] ReturnCustodyCommand command, CancellationToken cancellationToken) =>
        sender.Send(command with { Id = id }, cancellationToken);

    [HttpPut("{id:long}/Close")]
    public Task<Result> Close(long id, CancellationToken cancellationToken) =>
        sender.Send(new CloseCustodyCommand(id), cancellationToken);

    [HttpPut("{id:long}/Cancel")]
    public Task<Result> Cancel(long id, CancellationToken cancellationToken) =>
        sender.Send(new CancelCustodyCommand(id), cancellationToken);

    [HttpPut("{id:long}/Transfer")]
    public Task<Result> Transfer(long id, [FromBody] TransferCustodyCommand command, CancellationToken cancellationToken) =>
        sender.Send(command with { Id = id }, cancellationToken);
}

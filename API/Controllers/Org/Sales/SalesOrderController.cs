using Sales.Application.Quotations.Commands;
using Sales.Application.Quotations.Queries;
using Sales.Application.SalesOrders.Commands;
using Sales.Application.SalesOrders.Queries;
using Sales.Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrgSys.SharedKernel;

namespace API.Controllers.Org.Sales;

[Authorize]
[ApiController]
[Route("[controller]")]
public sealed class SalesOrderController(ISender sender) : ControllerBase
{
    [HttpPost]
    public Task<Result<long>> Create(CreateSalesOrderCommand command, CancellationToken cancellationToken) =>
        sender.Send(command, cancellationToken);

    [HttpGet("{id:long}")]
    public Task<Result<SalesOrderDto>> GetById(long id, CancellationToken cancellationToken) =>
        sender.Send(new GetSalesOrderByIdQuery(id), cancellationToken);

    [HttpGet]
    public Task<ResultCollection<SalesOrderDto>> GetList(long? customerId, SalesOrderStatus? status, CancellationToken cancellationToken) =>
        sender.Send(new GetSalesOrderListQuery(customerId, status), cancellationToken);

    [HttpPut("{id:long}/Confirm")]
    public Task<Result> Confirm(long id, [FromBody] ConfirmSalesOrderCommand command, CancellationToken cancellationToken) =>
        sender.Send(command with { Id = id }, cancellationToken);

    [HttpPut("{id:long}/Cancel")]
    public Task<Result> Cancel(long id, CancellationToken cancellationToken) =>
        sender.Send(new CancelSalesOrderCommand(id), cancellationToken);
}

[Authorize]
[ApiController]
[Route("[controller]")]
public sealed class QuotationController(ISender sender) : ControllerBase
{
    [HttpPost]
    public Task<Result<long>> Create(CreateQuotationCommand command, CancellationToken cancellationToken) =>
        sender.Send(command, cancellationToken);

    [HttpGet("{id:long}")]
    public Task<Result<QuotationDto>> GetById(long id, CancellationToken cancellationToken) =>
        sender.Send(new GetQuotationByIdQuery(id), cancellationToken);

    [HttpGet]
    public Task<ResultCollection<QuotationDto>> GetList(long? customerId, QuotationStatus? status, CancellationToken cancellationToken) =>
        sender.Send(new GetQuotationListQuery(customerId, status), cancellationToken);

    [HttpPut("{id:long}/Send")]
    public Task<Result> Send(long id, CancellationToken cancellationToken) =>
        sender.Send(new SendQuotationCommand(id), cancellationToken);

    [HttpPut("{id:long}/Accept")]
    public Task<Result> Accept(long id, CancellationToken cancellationToken) =>
        sender.Send(new AcceptQuotationCommand(id), cancellationToken);

    [HttpPut("{id:long}/Reject")]
    public Task<Result> Reject(long id, CancellationToken cancellationToken) =>
        sender.Send(new RejectQuotationCommand(id), cancellationToken);

    [HttpPut("{id:long}/Cancel")]
    public Task<Result> Cancel(long id, CancellationToken cancellationToken) =>
        sender.Send(new CancelQuotationCommand(id), cancellationToken);

    [HttpPost("{id:long}/Convert")]
    public Task<Result<long>> Convert(long id, long createUserId, CancellationToken cancellationToken) =>
        sender.Send(new ConvertQuotationCommand(id, createUserId), cancellationToken);
}

using Application.Commands.Org.Financials.Financial.Commands;
using Application.Commands.Org.Financials.Financial.Queries;
using Domain.Shared;
using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Financials
{
    [Route("[controller]")]
    [ApiController]
    public class FinancialController(ISender sender) : BaseController<GetByIdFinancialQuery, SearchFinancialQuery, GetListFinancialQuery,
        CreateFinancialCommand, UpdateFinancialCommand, DeleteFinancialCommand, DeleteListFinancialCommand,
        GetMaxFinancialQuery, FinancialModelView>(sender)
    {

        [HttpPut("Redo")]
        public async Task<Result> Redo(long Id, CancellationToken cancellationToken)
        {
            return await sender.Send(new RedoFinancialCommand(Id), cancellationToken);
        }

        [HttpPut("Cancel")]
        public async Task<Result> Cancel(long Id, CancellationToken cancellationToken)
        {
            return await sender.Send(new CancelFinancialCommand(Id), cancellationToken);
        }
    }
}
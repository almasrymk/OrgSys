using Application.Commands.Org.Invoices.Invoice.Commands;
using Application.Commands.Org.Invoices.Invoice.Queries;
using Application.Commands.Org.Setting.Invoice.Queries;
using Application.Interfaces.CQRS;
using Azure;
using Domain.Shared;
using Entity.ModelView;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Invoices
{
    [ApiController]
    [Route("[controller]")]
    public class InvoiceController(ISender sender) : BaseController<GetByIdInvoiceQuery, SearchInvoiceQuery, GetListInvoiceQuery, CreateInvoiceCommand, UpdateInvoiceCommand, DeleteInvoiceCommand, DeleteListInvoiceCommand, InvoiceModelView>(sender)
    {

        [HttpPut("Cancel")]
        public async Task<Result> Cancel(long Id, CancellationToken cancellationToken)
        {
            return await sender.Send(new CancelInvoiceCommand(Id), cancellationToken);
        }

        [HttpPut("Redo")]
        public async Task<Result> Redo(long Id, CancellationToken cancellationToken)
        {
            return await sender.Send(new RedoInvoiceCommand(Id), cancellationToken);
        }


        //public async Task<Result> CollectPaidInvoice(long Id, CancellationToken cancellationToken)
        //{
        //    // Implement your method logic here
        //    //return await Task.FromResult(new Result(HttpStatusCode.OK, null));
        //}
    }
}
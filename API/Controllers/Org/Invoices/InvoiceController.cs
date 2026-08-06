using Application.Commands.Org.Financials.Financial.Command;
using Application.Commands.Org.Invoices.Invoice.Commands;
using Application.Commands.Org.Invoices.Invoice.Queries;
using Application.Commands.Org.Setting.Invoice.Queries;
using Application.Interfaces.CQRS;
using Domain.Shared;
using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers.Org.Invoices
{
    [ApiController]
    [Route("[controller]")]
    public class InvoiceController(ISender sender) : BaseController<GetByIdInvoiceQuery,
        SearchInvoiceQuery, GetListInvoiceQuery, CreateInvoiceCommand, UpdateInvoiceCommand, DeleteInvoiceCommand, 
        DeleteListInvoiceCommand, GetMaxInvoiceQuery, InvoiceDto>(sender)
    {

        [HttpPut("Cancel")]
        public async Task<Result> Cancel(long Id, CancellationToken cancellationToken)
        {
            return await sender.Send(new CancelInvoiceCommand(Id), cancellationToken);
        }

        [HttpPut("Update")]
        public override async Task<Result> Update([FromBody] UpdateInvoiceCommand Update, CancellationToken cancellationToken)
        {
            var result = await base.Update(Update, cancellationToken);
            if (result.StatusCode != HttpStatusCode.OK)
                return result;

            return await sender.Send(
                new CreateTransactionByInvoiceCommand(Update.Id, RespectAutoCreatePreference: true),
                cancellationToken);
        }

        [HttpPut("Redo")]
        public async Task<Result> Redo(long Id, CancellationToken cancellationToken)
        {
            return await sender.Send(new RedoInvoiceCommand(Id), cancellationToken);
        }


        [HttpPost("CollectPaidInvoice")]
        public async Task<Result> CollectPaidInvoice(long InvoiceId, CancellationToken cancellationToken)
        {
            return await sender.Send(new CreateFinancialPaidInvoiceCommand(InvoiceId), cancellationToken);
        }

        [HttpPost("CreateTransactionInvoice")]
        public async Task<Result> CreateTransactionInvoice(long InvoiceId, CancellationToken cancellationToken)
        {
            return await sender.Send(new CreateTransactionByInvoiceCommand(InvoiceId), cancellationToken);
        }

        [HttpPost("CreateJournal")]
        public Task<Result> CreateJournal(long InvoiceId, CancellationToken cancellationToken) =>
            sender.Send(new CreateJournalByInvoiceCommand(InvoiceId), cancellationToken);


        [HttpGet("GetInvoicesNotReturn")]
        public async Task<ResultPagination<InvoiceDto>> GetInvoicesNotReturn(string KeySearch = "", long ParentId = 0, long TypeId = 0, int Page = 1, int PageSize = 10, CancellationToken cancellationToken = default)
        {
            return await sender.Send(new GetInvoiceNotReturnedQuery(KeySearch, ParentId, TypeId, Page, PageSize), cancellationToken);
        }

        [HttpGet("SearchInvoice")]
        public async Task<ResultPagination<InvoiceDto>> SearchInvoice(string KeySearch = "", long dealerId = 0, long currencyId = 0
           , int typeId = 1, int Page = 1 ,int PageSize = 10 , string Ids = "", CancellationToken cancellationToken = default)
        {
            return await sender.Send(new GetCreditAllByDealerIdQuery(KeySearch, typeId, Page, PageSize, 0, dealerId, currencyId , Ids), cancellationToken);
        }

        [HttpGet("GetProductInvoicesNotReturn")]
        public async Task<ResultCollection<InvoiceProductDto>> GetProductInvoicesNotReturn(long Id, CancellationToken cancellationToken = default)
        {
            return await sender.Send(new GetProductsNotReturnedQuery(Id), cancellationToken);
        }
    }
}

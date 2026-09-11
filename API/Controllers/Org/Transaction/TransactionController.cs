using Inventory.Application.Transactions.Queries;
using Inventory.Application.Transactions.Commands;
using Inventory.Application.Transactions.Queries;
using Inventory.Application.TransactionTypes.Commands;
using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Transaction
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionController(ISender sender) : BaseController<GetByIdTransactionQuery, SearchTransactionQuery ,GetListTransactionQuery,CreateTransactionCommand,UpdateTransactionCommand ,DeleteTransactionCommand, DeleteListTransactionCommand ,GetMaxTransactionQuery, TransactionDto>(sender)
    {
        [HttpPut("Cancel")]
        public Task<Result> Cancel(long Id, CancellationToken cancellationToken) =>
            Sender.Send(new CancelTransactionCommand(Id), cancellationToken);

        [HttpPut("Redo")]
        public Task<Result> Redo(long Id, CancellationToken cancellationToken) =>
            Sender.Send(new RedoTransactionCommand(Id), cancellationToken);

        [HttpPost("CreateReceived")]
        public Task<Result> CreateReceived(long TransferId, CancellationToken cancellationToken) =>
            Sender.Send(new CreateReceivedByTransferCommand(TransferId), cancellationToken);

        [HttpPost("CreateJournal")]
        public Task<Result> CreateJournal(long TransactionId, CancellationToken cancellationToken) =>
            Sender.Send(new CreateJournalByTransactionCommand(TransactionId), cancellationToken);

    }
}

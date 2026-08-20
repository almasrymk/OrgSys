using Application.Commands.Org.Setting.Transaction.Queries;
using Application.Commands.Org.Transactions.Transaction.Commands;
using Application.Commands.Org.Transactions.Transaction.Queries;
using Application.Commands.Org.Transactions.TransactionType.Commands;
using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Domain.Shared;

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

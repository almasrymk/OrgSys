using Application.Commands.Org.Financials.Journal.Commands;
using Application.Commands.Org.Financials.Journal.Queries;
using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Domain.Shared;

namespace API.Controllers.Org.Journals
{
    [ApiController]
    [Route("[controller]")]
    public class JournalController(ISender sender) : BaseController<GetByIdJournalQuery,SearchJournalQuery, GetListJournalQuery, CreateJournalCommand, UpdateJournalCommand, DeleteJournalCommand, DeleteListJournalCommand, GetMaxJournalQuery, JournalDto>(sender)
    {
        [HttpPut("Cancel")]
        public Task<Result> Cancel(long Id, CancellationToken cancellationToken) =>
            Sender.Send(new CancelJournalCommand(Id), cancellationToken);

        [HttpPut("Redo")]
        public Task<Result> Redo(long Id, CancellationToken cancellationToken) =>
            Sender.Send(new RedoJournalCommand(Id), cancellationToken);

        [HttpPut("Post")]
        public Task<Result> Post(long Id, CancellationToken cancellationToken) =>
            Sender.Send(new PostJournalCommand(Id), cancellationToken);
    }
}

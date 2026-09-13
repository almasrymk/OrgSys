using Accounting.Application.Journals.Commands;
using Accounting.Application.Journals.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPut("Reverse")]
        public Task<Result> Reverse(long Id, CancellationToken cancellationToken) =>
            Sender.Send(new ReverseJournalCommand(Id), cancellationToken);
    }
}

using Parties.Application.PartyContacts.Commands;
using Parties.Application.PartyContacts.Queries;
using OrgSys.SharedKernel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Parties
{
    /// <summary>Plain controller, not BaseController&lt;&gt; — contacts are always viewed/managed in
    /// the context of one Dealer (GetListByDealer), not searched globally, so the generic
    /// GetById/Search/List/Create/Update/Delete/DeleteList 8-type-parameter shape doesn't fit.</summary>
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PartyContactController(ISender sender) : ControllerBase
    {
        [HttpGet("GetById")]
        public virtual async Task<Result<PartyContactDto>> GetById(long id, CancellationToken cancellationToken)
        {
            return await sender.Send(new GetByIdPartyContactQuery(id), cancellationToken);
        }

        [HttpGet("GetListByDealer")]
        public virtual async Task<ResultCollection<PartyContactDto>> GetListByDealer(long dealerId, string? keySearch, int page, int pageSize, CancellationToken cancellationToken)
        {
            return await sender.Send(new GetListByDealerPartyContactQuery(dealerId, keySearch ?? string.Empty, 0, 0, page, pageSize), cancellationToken);
        }

        [HttpPost("Create")]
        public virtual async Task<Result> Create([FromBody] CreatePartyContactCommand command, CancellationToken cancellationToken)
        {
            return await sender.Send(command, cancellationToken);
        }

        [HttpPut("Update")]
        public virtual async Task<Result> Update([FromBody] UpdatePartyContactCommand command, CancellationToken cancellationToken)
        {
            return await sender.Send(command, cancellationToken);
        }

        [HttpDelete("Delete")]
        public virtual async Task<Result> Delete(long id, CancellationToken cancellationToken)
        {
            return await sender.Send(new DeletePartyContactCommand(id), cancellationToken);
        }

        [HttpDelete("DeleteList")]
        public virtual async Task<Result> DeleteList([FromQuery] List<long> ids, CancellationToken cancellationToken)
        {
            return await sender.Send(new DeleteListPartyContactCommand(ids), cancellationToken);
        }
    }
}

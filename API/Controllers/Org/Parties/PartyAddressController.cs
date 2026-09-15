using Parties.Application.PartyAddresses.Commands;
using Parties.Application.PartyAddresses.Queries;
using OrgSys.SharedKernel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Parties
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PartyAddressController(ISender sender) : ControllerBase
    {
        [HttpGet("GetById")]
        public virtual async Task<Result<PartyAddressDto>> GetById(long id, CancellationToken cancellationToken)
        {
            return await sender.Send(new GetByIdPartyAddressQuery(id), cancellationToken);
        }

        [HttpGet("GetListByDealer")]
        public virtual async Task<ResultCollection<PartyAddressDto>> GetListByDealer(long dealerId, int page, int pageSize, CancellationToken cancellationToken)
        {
            return await sender.Send(new GetListByDealerPartyAddressQuery(dealerId, string.Empty, 0, 0, page, pageSize), cancellationToken);
        }

        [HttpPost("Create")]
        public virtual async Task<Result> Create([FromBody] CreatePartyAddressCommand command, CancellationToken cancellationToken)
        {
            return await sender.Send(command, cancellationToken);
        }

        [HttpPut("Update")]
        public virtual async Task<Result> Update([FromBody] UpdatePartyAddressCommand command, CancellationToken cancellationToken)
        {
            return await sender.Send(command, cancellationToken);
        }

        [HttpDelete("Delete")]
        public virtual async Task<Result> Delete(long id, CancellationToken cancellationToken)
        {
            return await sender.Send(new DeletePartyAddressCommand(id), cancellationToken);
        }

        [HttpDelete("DeleteList")]
        public virtual async Task<Result> DeleteList([FromQuery] List<long> ids, CancellationToken cancellationToken)
        {
            return await sender.Send(new DeleteListPartyAddressCommand(ids), cancellationToken);
        }
    }
}

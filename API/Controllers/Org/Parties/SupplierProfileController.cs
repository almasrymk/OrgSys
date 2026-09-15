using Parties.Application.SupplierProfiles.Commands;
using Parties.Application.SupplierProfiles.Queries;
using OrgSys.SharedKernel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Parties
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class SupplierProfileController(ISender sender) : ControllerBase
    {
        [HttpGet("GetByDealerId")]
        public virtual async Task<Result<SupplierProfileDto>> GetByDealerId(long dealerId, CancellationToken cancellationToken)
        {
            return await sender.Send(new GetSupplierProfileByDealerIdQuery(dealerId), cancellationToken);
        }

        [HttpPost("Assign")]
        public virtual async Task<Result> Assign([FromBody] AssignSupplierRoleCommand command, CancellationToken cancellationToken)
        {
            return await sender.Send(command, cancellationToken);
        }
    }
}

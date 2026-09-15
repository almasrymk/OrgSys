using Parties.Application.CustomerProfiles.Commands;
using Parties.Application.CustomerProfiles.Queries;
using OrgSys.SharedKernel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Parties
{
    /// <summary>Plain controller, not BaseController&lt;&gt; — the Customer role is an explicit
    /// business action on an existing Dealer (brief §2.15/CQRS RULE), not a listable/generic-CRUD
    /// entity in its own right. Mirrors API.Controllers.Org.Organization.OrganizationSettingsController.</summary>
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CustomerProfileController(ISender sender) : ControllerBase
    {
        [HttpGet("GetByDealerId")]
        public virtual async Task<Result<CustomerProfileDto>> GetByDealerId(long dealerId, CancellationToken cancellationToken)
        {
            return await sender.Send(new GetCustomerProfileByDealerIdQuery(dealerId), cancellationToken);
        }

        [HttpPost("Assign")]
        public virtual async Task<Result> Assign([FromBody] AssignCustomerRoleCommand command, CancellationToken cancellationToken)
        {
            return await sender.Send(command, cancellationToken);
        }
    }
}

using Organization.Application.OrganizationSettings.Commands;
using Organization.Application.OrganizationSettings.Queries;
using OrgSys.SharedKernel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Organization
{
    /// <summary>Plain controller, not BaseController&lt;&gt; — OrganizationSettings is a
    /// singleton-per-Company value (brief §1.10), read/written by CompanyId rather than by its own
    /// Id, so the generic GetById/List/Search/Create/Delete/DeleteList shape doesn't fit. Mirrors
    /// API.Controllers.Org.Catalog.PricingController's plain-controller shape.</summary>
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class OrganizationSettingsController(ISender sender) : ControllerBase
    {
        [HttpGet("GetByCompanyId")]
        public virtual async Task<Result<OrganizationSettingsDto>> GetByCompanyId(long companyId, CancellationToken cancellationToken)
        {
            return await sender.Send(new GetOrganizationSettingsQuery(companyId), cancellationToken);
        }

        [HttpPut("Update")]
        public virtual async Task<Result> Update([FromBody] UpdateOrganizationSettingsCommand command, CancellationToken cancellationToken)
        {
            return await sender.Send(command, cancellationToken);
        }
    }
}

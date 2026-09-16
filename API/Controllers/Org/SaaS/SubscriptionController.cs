using SaaS.Application;
using SaaS.Application.Subscriptions.Commands;
using SaaS.Application.Subscriptions.Queries;
using OrgSys.SharedKernel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.SaaS
{
    /// <summary>Plain controller, not BaseController&lt;&gt; — Subscription has no generic Update
    /// (plan changes/cancellation are explicit lifecycle actions, brief §56/§71, same reasoning as
    /// Tenant's Activate/Suspend/Cancel) and Create takes a dedicated SubscribeTenant shape, not a
    /// raw SubscriptionDto. Mirrors API.Controllers.Org.Organization.OrganizationSettingsController's
    /// plain-controller shape.</summary>
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class SubscriptionController(ISender sender) : ControllerBase
    {
        [HttpGet("GetById")]
        public virtual async Task<Result<SubscriptionDto>> GetById(long id, CancellationToken cancellationToken)
            => await sender.Send(new GetByIdSubscriptionQuery(id), cancellationToken);

        [HttpGet("GetList")]
        public virtual async Task<ResultCollection<SubscriptionDto>> GetList(string? keySearch, long tenantId, long typeId, int page, int pageSize, CancellationToken cancellationToken)
            => await sender.Send(new GetListSubscriptionQuery(keySearch ?? string.Empty, tenantId, typeId, page, pageSize), cancellationToken);

        [HttpGet("Search")]
        public virtual async Task<ResultPagination<SubscriptionDto>> Search(string? keySearch, long tenantId, long typeId, int page, int pageSize, CancellationToken cancellationToken)
            => await sender.Send(new SearchSubscriptionQuery(keySearch ?? string.Empty, tenantId, typeId, page, pageSize), cancellationToken);

        [HttpPost("Subscribe")]
        public virtual async Task<Result> Subscribe([FromBody] SubscribeTenantCommand command, CancellationToken cancellationToken)
            => await sender.Send(command, cancellationToken);

        [HttpPost("ChangePlan")]
        public virtual async Task<Result> ChangePlan([FromBody] ChangeSubscriptionPlanCommand command, CancellationToken cancellationToken)
            => await sender.Send(command, cancellationToken);

        [HttpPost("Cancel")]
        public virtual async Task<Result> Cancel(long id, CancellationToken cancellationToken)
            => await sender.Send(new CancelSubscriptionCommand(id), cancellationToken);
    }
}

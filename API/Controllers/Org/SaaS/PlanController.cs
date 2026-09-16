using SaaS.Application;
using SaaS.Application.Plans.Commands;
using SaaS.Application.Plans.Queries;
using SaaS.Application.PlanFeatures.Commands;
using SaaS.Application.PlanFeatures.Queries;
using OrgSys.SharedKernel;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.SaaS
{
    [ApiController]
    [Route("[controller]")]
    public class PlanController(ISender sender) : BaseController<GetByIdPlanQuery, SearchPlanQuery, GetListPlanQuery, CreatePlanCommand, UpdatePlanCommand, DeletePlanCommand, DeleteListPlanCommand, PlanDto>(sender)
    {
        [HttpGet("GetFeatures")]
        public virtual async Task<Result<List<string>>> GetFeatures(long planId, CancellationToken cancellationToken)
            => await Sender.Send(new GetFeaturesForPlanQuery(planId), cancellationToken);

        [HttpPost("AssignFeature")]
        public virtual async Task<Result> AssignFeature(long planId, long featureId, CancellationToken cancellationToken)
            => await Sender.Send(new AssignFeatureToPlanCommand(planId, featureId), cancellationToken);

        [HttpDelete("RemoveFeature")]
        public virtual async Task<Result> RemoveFeature(long planId, long featureId, CancellationToken cancellationToken)
            => await Sender.Send(new RemoveFeatureFromPlanCommand(planId, featureId), cancellationToken);
    }
}

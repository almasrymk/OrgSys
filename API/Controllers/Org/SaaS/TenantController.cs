using SaaS.Application;
using SaaS.Application.Tenants.Commands;
using SaaS.Application.Tenants.Queries;
using OrgSys.SharedKernel;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.SaaS
{
    [ApiController]
    [Route("[controller]")]
    public class TenantController(ISender sender) : BaseController<GetByIdTenantQuery, SearchTenantQuery, GetListTenantQuery, CreateTenantCommand, UpdateTenantCommand, DeleteTenantCommand, DeleteListTenantCommand, TenantDto>(sender)
    {
        [HttpPost("Activate")]
        public virtual async Task<Result> Activate(long id, CancellationToken cancellationToken)
            => await Sender.Send(new ActivateTenantCommand(id), cancellationToken);

        [HttpPost("Suspend")]
        public virtual async Task<Result> Suspend(long id, CancellationToken cancellationToken)
            => await Sender.Send(new SuspendTenantCommand(id), cancellationToken);

        [HttpPost("Cancel")]
        public virtual async Task<Result> Cancel(long id, CancellationToken cancellationToken)
            => await Sender.Send(new CancelTenantCommand(id), cancellationToken);
    }
}

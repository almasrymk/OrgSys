namespace SaaS.Application.Tenants.Commands
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    public sealed class UpdateTenantCommand : TenantDto, ICommand, IUpdateCommand<Result>;

    /// <summary>Update covers Name only — TenantStatus transitions go through the dedicated
    /// Activate/Suspend/Cancel commands below (brief §51's own status list implies explicit
    /// lifecycle actions, not a free-form status field), so this handler re-reads the existing
    /// lifecycle fields and pins them back onto the request before mapping, regardless of what the
    /// caller sent.</summary>
    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Tenant> _Repository, IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateTenantCommand, Tenant>(_UnitOfWork, _Repository, mapper, _provider)
    {
        public override async Task<Result> Handle(UpdateTenantCommand request, CancellationToken cancellationToken)
        {
            var existing = await _Repository.GetByFilterAsync(e => e.Id == request.Id, string.Empty);
            if (existing is null)
                return new Result(HttpStatusCode.NotFound, [new Error("Tenant not found")]);

            request.TenantStatus = existing.TenantStatus;
            request.TrialEndsAt = existing.TrialEndsAt;
            request.ActivatedAt = existing.ActivatedAt;
            request.SuspendedAt = existing.SuspendedAt;

            return await base.Handle(request, cancellationToken);
        }
    }
}

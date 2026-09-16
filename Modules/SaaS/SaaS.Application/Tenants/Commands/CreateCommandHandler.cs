namespace SaaS.Application.Tenants.Commands
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Threading;
    using System.Threading.Tasks;

    public sealed class CreateTenantCommand : TenantDto, ICommand, ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Tenant> _Repository, IMapper mapper) : CreateCommandHandler<CreateTenantCommand, Tenant>(_UnitOfWork, _Repository, mapper)
    {
        /// <summary>New tenants always start in Trial, per brief §51 — TenantStatus is not
        /// caller-settable on Create (matches this codebase's "explicit lifecycle command, not a
        /// raw status field" convention already used by Journal.Post/FiscalPeriod.Close).</summary>
        public override async Task<Result> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
        {
            request.TenantStatus = TenantLifecycleStatus.Trial;
            request.TrialEndsAt ??= DateTime.UtcNow.AddDays(30);
            request.ActivatedAt = null;
            request.SuspendedAt = null;
            return await base.Handle(request, cancellationToken);
        }
    }
}

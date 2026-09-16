namespace SaaS.Application.Subscriptions.Commands
{
    using OrgSys.SharedKernel;
    using SaaS.Domain.Exceptions;
    using System.Net;

    /// <summary>brief §71 SubscribeTenant — creates the Subscription billing record for a Tenant
    /// against a Plan. Requires the Tenant to exist and be usable (not Suspended/Cancelled) —
    /// mirrors FiscalYear.EnsureOpenForPosting's "final-authority check before the write" shape.</summary>
    public sealed record SubscribeTenantCommand(long TenantId, long PlanId, DateTime StartDate, bool StartAsTrial) : ICommand, ICreateCommand<Result>;

    public sealed class SubscribeTenantCommandHandler(
        IUnitOfWork unitOfWork,
        IRepository<Subscription> repository,
        IRepository<Tenant> tenantRepository,
        IRepository<Plan> planRepository) : ICommandHandler<SubscribeTenantCommand>
    {
        public async Task<Result> Handle(SubscribeTenantCommand request, CancellationToken cancellationToken)
        {
            var tenant = await tenantRepository.GetByFilterAsync(e => e.Id == request.TenantId, string.Empty);
            if (tenant is null)
                return new Result(HttpStatusCode.NotFound, [new Error("Tenant not found")]);

            try
            {
                tenant.EnsureActive();
            }
            catch (TenantNotActiveException ex)
            {
                return new Result(HttpStatusCode.BadRequest, [new Error(ex.Message)]);
            }

            if (!await planRepository.AnyAsync(e => e.Id == request.PlanId, cancellationToken))
                return new Result(HttpStatusCode.NotFound, [new Error("Plan not found")]);

            var subscription = new Subscription
            {
                TenantId = request.TenantId,
                PlanId = request.PlanId,
                StartDate = request.StartDate,
                SubscriptionStatus = request.StartAsTrial ? SubscriptionStatus.Trial : SubscriptionStatus.Active,
                TrialEndsAt = request.StartAsTrial ? request.StartDate.AddDays(30) : null,
            };

            await repository.CreateAsync(subscription);

            return await unitOfWork.SaveChangeAsync(cancellationToken) > 0
                ? new Result(HttpStatusCode.OK, null)
                : new Result(HttpStatusCode.InternalServerError, [new Error("Error saving changes")]);
        }
    }
}

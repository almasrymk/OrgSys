namespace SaaS.Application.Subscriptions.Commands
{
    using OrgSys.SharedKernel;
    using SaaS.Domain.Exceptions;
    using System.Net;

    public sealed record ChangeSubscriptionPlanCommand(long Id, long NewPlanId) : ICommand, IUpdateCommand<Result>;

    public sealed class ChangeSubscriptionPlanCommandHandler(
        IUnitOfWork unitOfWork,
        IRepository<Subscription> repository,
        IRepository<Plan> planRepository) : ICommandHandler<ChangeSubscriptionPlanCommand>
    {
        public async Task<Result> Handle(ChangeSubscriptionPlanCommand request, CancellationToken cancellationToken)
        {
            var subscription = await repository.GetByFilterAsync(e => e.Id == request.Id, string.Empty);
            if (subscription is null)
                return new Result(HttpStatusCode.NotFound, [new Error("Subscription not found")]);
            if (!await planRepository.AnyAsync(e => e.Id == request.NewPlanId, cancellationToken))
                return new Result(HttpStatusCode.NotFound, [new Error("Plan not found")]);

            try
            {
                subscription.ChangePlan(request.NewPlanId);
            }
            catch (InvalidSubscriptionTransitionException ex)
            {
                return new Result(HttpStatusCode.BadRequest, [new Error(ex.Message)]);
            }

            await repository.UpdateAsync(subscription);
            return await unitOfWork.SaveChangeAsync(cancellationToken) >= 0
                ? new Result(HttpStatusCode.OK, null)
                : new Result(HttpStatusCode.InternalServerError, [new Error("Error saving changes")]);
        }
    }

    public sealed record CancelSubscriptionCommand(long Id) : ICommand, IUpdateCommand<Result>;

    public sealed class CancelSubscriptionCommandHandler(IUnitOfWork unitOfWork, IRepository<Subscription> repository) : ICommandHandler<CancelSubscriptionCommand>
    {
        public async Task<Result> Handle(CancelSubscriptionCommand request, CancellationToken cancellationToken)
        {
            var subscription = await repository.GetByFilterAsync(e => e.Id == request.Id, string.Empty);
            if (subscription is null)
                return new Result(HttpStatusCode.NotFound, [new Error("Subscription not found")]);

            try
            {
                subscription.Cancel(DateTime.UtcNow);
            }
            catch (InvalidSubscriptionTransitionException ex)
            {
                return new Result(HttpStatusCode.BadRequest, [new Error(ex.Message)]);
            }

            await repository.UpdateAsync(subscription);
            return await unitOfWork.SaveChangeAsync(cancellationToken) >= 0
                ? new Result(HttpStatusCode.OK, null)
                : new Result(HttpStatusCode.InternalServerError, [new Error("Error saving changes")]);
        }
    }
}

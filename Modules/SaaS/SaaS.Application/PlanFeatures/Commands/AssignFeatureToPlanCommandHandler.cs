namespace SaaS.Application.PlanFeatures.Commands
{
    using OrgSys.SharedKernel;
    using System.Net;

    public sealed record AssignFeatureToPlanCommand(long PlanId, long FeatureId) : ICommand;

    public sealed class AssignFeatureToPlanCommandHandler(
        IUnitOfWork unitOfWork,
        IRepository<PlanFeature> repository,
        IRepository<Plan> planRepository,
        IRepository<Feature> featureRepository) : ICommandHandler<AssignFeatureToPlanCommand>
    {
        public async Task<Result> Handle(AssignFeatureToPlanCommand request, CancellationToken cancellationToken)
        {
            if (!await planRepository.AnyAsync(e => e.Id == request.PlanId, cancellationToken))
                return new Result(HttpStatusCode.NotFound, [new Error("Plan not found")]);
            if (!await featureRepository.AnyAsync(e => e.Id == request.FeatureId, cancellationToken))
                return new Result(HttpStatusCode.NotFound, [new Error("Feature not found")]);
            if (await repository.AnyAsync(e => e.PlanId == request.PlanId && e.FeatureId == request.FeatureId, cancellationToken))
                return new Result(HttpStatusCode.BadRequest, [new Error("This feature is already assigned to the plan")]);

            await repository.CreateAsync(new PlanFeature { PlanId = request.PlanId, FeatureId = request.FeatureId });

            return await unitOfWork.SaveChangeAsync(cancellationToken) > 0
                ? new Result(HttpStatusCode.OK, null)
                : new Result(HttpStatusCode.InternalServerError, [new Error("Error saving changes")]);
        }
    }

    public sealed record RemoveFeatureFromPlanCommand(long PlanId, long FeatureId) : ICommand;

    public sealed class RemoveFeatureFromPlanCommandHandler(
        IUnitOfWork unitOfWork,
        IRepository<PlanFeature> repository) : ICommandHandler<RemoveFeatureFromPlanCommand>
    {
        public async Task<Result> Handle(RemoveFeatureFromPlanCommand request, CancellationToken cancellationToken)
        {
            var removed = await repository.DeleteAsync(e => e.PlanId == request.PlanId && e.FeatureId == request.FeatureId);
            if (!removed)
                return new Result(HttpStatusCode.NotFound, [new Error("This feature is not assigned to the plan")]);

            return await unitOfWork.SaveChangeAsync(cancellationToken) >= 0
                ? new Result(HttpStatusCode.OK, null)
                : new Result(HttpStatusCode.InternalServerError, [new Error("Error saving changes")]);
        }
    }
}

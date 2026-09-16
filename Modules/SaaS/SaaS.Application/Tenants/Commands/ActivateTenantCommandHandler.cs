namespace SaaS.Application.Tenants.Commands
{
    using OrgSys.SharedKernel;
    using SaaS.Domain.Exceptions;
    using System.Net;

    public sealed record ActivateTenantCommand(long Id) : ICommand, IUpdateCommand<Result>;

    public sealed class ActivateTenantCommandHandler(IUnitOfWork unitOfWork, IRepository<Tenant> repository) : ICommandHandler<ActivateTenantCommand>
    {
        public async Task<Result> Handle(ActivateTenantCommand request, CancellationToken cancellationToken)
        {
            var tenant = await repository.GetByFilterAsync(e => e.Id == request.Id, string.Empty);
            if (tenant is null)
                return new Result(HttpStatusCode.NotFound, [new Error("Tenant not found")]);

            try
            {
                tenant.Activate(DateTime.UtcNow);
            }
            catch (InvalidTenantTransitionException ex)
            {
                return new Result(HttpStatusCode.BadRequest, [new Error(ex.Message)]);
            }

            await repository.UpdateAsync(tenant);
            return await unitOfWork.SaveChangeAsync(cancellationToken) >= 0
                ? new Result(HttpStatusCode.OK, null)
                : new Result(HttpStatusCode.InternalServerError, [new Error("Error saving changes")]);
        }
    }
}

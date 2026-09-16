namespace Administration.Application.Users.Commands
{
    using OrgSys.SharedKernel;
    using Administration.Application.Security;
    using AutoMapper;
    using MediatR;
    using Organization.Contracts.Companies;
    using SaaS.Contracts.Features;
    using SaaS.Contracts.Tenancy;
    using System.Net;

    public sealed class CreateUserCommand : UserDto, ICommand, ICreateCommand<Result>;

    public sealed class CreateCommandHandler(
        IUnitOfWork _UnitOfWork,
        IRepository<Administration.Domain.User> _Repository,
        IMapper mapper,
        IPasswordHasher passwordHasher,
        ICurrentTenant currentTenant,
        ITenantFeatureService features,
        ISender sender) : CreateCommandHandler<CreateUserCommand, Administration.Domain.User>(_UnitOfWork, _Repository, mapper)
    {
        public override async Task<Result> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            UserPasswordApplier.Apply(request, passwordHasher);

            long? tenantId = currentTenant.TenantId;
            if (request.BranchId is > 0)
            {
                var ownership = (await sender.Send(new GetBranchOwnershipQuery(request.BranchId.Value), cancellationToken)).Response;
                if (ownership is null)
                    return new Result(HttpStatusCode.BadRequest, [new Error("Branch not found.")]);
                if (tenantId is long current && ownership.TenantId != current)
                    return new Result(HttpStatusCode.NotFound, [new Error("Branch not found.")]);
                tenantId ??= ownership.TenantId;
            }

            if (tenantId is long scopedTenant)
            {
                var scope = (await sender.Send(new GetTenantOrganizationScopeQuery(scopedTenant), cancellationToken)).Response;
                var branchIds = scope?.BranchIds ?? [];
                var existing = await _Repository.GetListByFilterAsync(u =>
                    u.BranchId != null && branchIds.Contains(u.BranchId.Value));
                var count = existing?.Count() ?? 0;
                if (!await features.IsWithinLimitAsync(scopedTenant, TenantLimit.Users, count, cancellationToken))
                    return new Result(HttpStatusCode.Forbidden, [new Error("User limit reached for this tenant.")]);
            }

            return await base.Handle(request, cancellationToken);
        }
    }
}

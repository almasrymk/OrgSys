namespace Inventory.Application.Stocks.Commands
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using MediatR;
    using Organization.Contracts.Companies;
    using SaaS.Contracts.Features;
    using SaaS.Contracts.Tenancy;
    using System.Net;

    public sealed record CreateStockCommand(string Name, long BranchId, long? AccountId) : ICommand, ICreateCommand<Result>;

    public sealed class CreateCommandHandler(
        IUnitOfWork _UnitOfWork,
        IRepository<Inventory.Domain.Stock> _Repository,
        IMapper mapper,
        ICurrentTenant currentTenant,
        ITenantFeatureService features,
        ISender sender) : CreateCommandHandler<CreateStockCommand, Inventory.Domain.Stock>(_UnitOfWork, _Repository, mapper)
    {
        public override async Task<Result> Handle(CreateStockCommand request, CancellationToken cancellationToken)
        {
            var ownership = (await sender.Send(new GetBranchOwnershipQuery(request.BranchId), cancellationToken)).Response;
            if (ownership is null)
                return new Result(HttpStatusCode.BadRequest, [new Error("Branch not found.")]);
            if (currentTenant.TenantId is long tenantId && ownership.TenantId != tenantId)
                return new Result(HttpStatusCode.NotFound, [new Error("Branch not found.")]);

            var scopedTenant = currentTenant.TenantId ?? ownership.TenantId;
            if (scopedTenant is long tenant)
            {
                var scope = (await sender.Send(new GetTenantOrganizationScopeQuery(tenant), cancellationToken)).Response;
                var branchIds = scope?.BranchIds ?? [];
                var existing = await _Repository.GetListByFilterAsync(s => branchIds.Contains(s.BranchId));
                var count = existing?.Count() ?? 0;
                if (!await features.IsWithinLimitAsync(tenant, TenantLimit.Warehouses, count, cancellationToken))
                    return new Result(HttpStatusCode.Forbidden, [new Error("Warehouse limit reached for this tenant.")]);
            }

            return await base.Handle(request, cancellationToken);
        }
    }
}

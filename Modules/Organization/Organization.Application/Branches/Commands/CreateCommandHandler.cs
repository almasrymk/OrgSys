namespace Organization.Application.Branches.Commands
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using SaaS.Contracts.Features;
    using SaaS.Contracts.Tenancy;
    using System.Net;

    public sealed record CreateBranchCommand(string Name, long CompanyId) : ICommand, ICreateCommand<Result>;

    public sealed class CreateCommandHandler(
        IUnitOfWork _UnitOfWork,
        IRepository<Organization.Domain.Branch> _Repository,
        IRepository<Organization.Domain.Company> companyRepository,
        IMapper mapper,
        ICurrentTenant currentTenant,
        ITenantFeatureService features) : CreateCommandHandler<CreateBranchCommand, Organization.Domain.Branch>(_UnitOfWork, _Repository, mapper)
    {
        public override async Task<Result> Handle(CreateBranchCommand request, CancellationToken cancellationToken)
        {
            var company = await companyRepository.GetByFilterAsync(e => e.Id == request.CompanyId, string.Empty);
            if (company is null || company.Id == 0)
                return new Result(HttpStatusCode.BadRequest, [new Error("Company not found.")]);

            if (currentTenant.TenantId is long tenantId && company.TenantId != tenantId)
                return new Result(HttpStatusCode.NotFound, [new Error("Company not found.")]);

            if (company.TenantId is long scopedTenant)
            {
                var companyIds = (await companyRepository.GetListByFilterAsync(c => c.TenantId == scopedTenant) ?? [])
                    .Select(c => c.Id).ToList();
                var existing = await _Repository.GetListByFilterAsync(b => companyIds.Contains(b.CompanyId));
                var count = existing?.Count() ?? 0;
                if (!await features.IsWithinLimitAsync(scopedTenant, TenantLimit.Branches, count, cancellationToken))
                    return new Result(HttpStatusCode.Forbidden, [new Error("Branch limit reached for this tenant.")]);
            }

            return await base.Handle(request, cancellationToken);
        }
    }
}

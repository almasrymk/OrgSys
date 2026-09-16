namespace Organization.Application.Companies.Commands
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using SaaS.Contracts.Features;
    using SaaS.Contracts.Tenancy;
    using System.Net;

    public sealed class CreateCompanyCommand : CompanyDto, ICommand, ICreateCommand<Result>;

    public sealed class CreateCommandHandler(
        IUnitOfWork _UnitOfWork,
        IRepository<Organization.Domain.Company> _Repository,
        IMapper mapper,
        ICurrentTenant currentTenant,
        ITenantFeatureService features) : CreateCommandHandler<CreateCompanyCommand, Organization.Domain.Company>(_UnitOfWork, _Repository, mapper)
    {
        public override async Task<Result> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
        {
            if (currentTenant.TenantId is long tenantId)
                request.TenantId = tenantId;

            if (request.TenantId is long scopedTenant)
            {
                var existing = await _Repository.GetListByFilterAsync(c => c.TenantId == scopedTenant && c.Status != Status.Deleted);
                var count = existing?.Count() ?? 0;
                if (!await features.IsWithinLimitAsync(scopedTenant, TenantLimit.Companies, count, cancellationToken))
                    return new Result(HttpStatusCode.Forbidden, [new Error("Company limit reached for this tenant.")]);
            }

            return await base.Handle(request, cancellationToken);
        }
    }
}

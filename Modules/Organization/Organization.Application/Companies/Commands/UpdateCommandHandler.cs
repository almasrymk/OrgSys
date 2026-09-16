namespace Organization.Application.Companies.Commands
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using SaaS.Contracts.Tenancy;
    using System.Net;

    public sealed class UpdateCompanyCommand : CompanyDto, ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(
        IUnitOfWork _UnitOfWork,
        IRepository<Organization.Domain.Company> _Repository,
        IMapper mapper,
        IServiceProvider _provider,
        ICurrentTenant currentTenant) : UpdateCommandHandler<UpdateCompanyCommand, Organization.Domain.Company>(_UnitOfWork, _Repository, mapper, _provider)
    {
        public override async Task<Result> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
        {
            var existing = await _Repository.GetByFilterAsync(e => e.Id == request.Id, string.Empty);
            if (existing is null || existing.Id == 0)
                return new Result(HttpStatusCode.NotFound, [new Error("Company not found.")]);

            if (currentTenant.TenantId is long tenantId && existing.TenantId != tenantId)
                return new Result(HttpStatusCode.NotFound, [new Error("Company not found.")]);

            if (currentTenant.TenantId is long scoped)
                request.TenantId = scoped;

            return await base.Handle(request, cancellationToken);
        }
    }
}

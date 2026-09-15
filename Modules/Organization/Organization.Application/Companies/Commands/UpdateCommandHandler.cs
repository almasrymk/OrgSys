namespace Organization.Application.Companies.Commands
{
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed class UpdateCompanyCommand : CompanyDto, ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Organization.Domain.Company> _Repository, IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateCompanyCommand, Organization.Domain.Company>(_UnitOfWork, _Repository, mapper, _provider)
    {

    }
}

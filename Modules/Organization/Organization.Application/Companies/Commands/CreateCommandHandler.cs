namespace Organization.Application.Companies.Commands
{
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed class CreateCompanyCommand : CompanyDto, ICommand, ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Organization.Domain.Company> _Repository, IMapper mapper) : CreateCommandHandler<CreateCompanyCommand, Organization.Domain.Company>(_UnitOfWork, _Repository, mapper)
    {

    }
}

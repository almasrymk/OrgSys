namespace Organization.Application;

using Organization.Application.Companies.Commands;
using AutoMapper;

public partial class MappingProfile : Profile
{
    public void CompanyMappingProfile()
    {
        #region Company
        CreateMap<Organization.Domain.Company, CompanyDto>();
        CreateMap<CompanyDto, Organization.Domain.Company>();

        CreateMap<Organization.Domain.Company, CreateCompanyCommand>();
        CreateMap<CreateCompanyCommand, Organization.Domain.Company>();
        CreateMap<Organization.Domain.Company, UpdateCompanyCommand>();
        CreateMap<UpdateCompanyCommand, Organization.Domain.Company>();
        CreateMap<Organization.Domain.Company, DeleteCompanyCommand>();
        CreateMap<DeleteCompanyCommand, Organization.Domain.Company>();

        CreateMap<CompanyDto, CreateCompanyCommand>();
        CreateMap<CreateCompanyCommand, CompanyDto>();
        CreateMap<CompanyDto, UpdateCompanyCommand>();
        CreateMap<UpdateCompanyCommand, CompanyDto>();
        #endregion
    }
}

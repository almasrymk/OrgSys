using Organization.Application.Companies.Commands;
using Organization.Application.Companies.Queries;
using OrgSys.SharedKernel;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Organization
{
    [ApiController]
    [Route("[controller]")]
    public class CompanyController(ISender sender) : BaseController<GetByIdCompanyQuery, SearchCompanyQuery, GetListCompanyQuery, CreateCompanyCommand, UpdateCompanyCommand, DeleteCompanyCommand, DeleteListCompanyCommand, CompanyDto>(sender)
    {

    }
}

using Organization.Application.Departments.Commands;
using Organization.Application.Departments.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Organization
{
    [ApiController]
    [Route("[controller]")]
    public class DepartmentController(ISender sender) : BaseController<GetByIdDepartmentQuery, SearchDepartmentQuery, GetListDepartmentQuery, CreateDepartmentCommand, UpdateDepartmentCommand, DeleteDepartmentCommand, DeleteListDepartmentCommand, DepartmentDto>(sender)
    {
    }
}

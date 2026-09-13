using Administration.Application.Roles.Commands;
using Administration.Application.Roles.Queries;
using OrgSys.SharedKernel;
using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Setting
{
    [ApiController]
    [Route("[controller]")]
    public class RoleController(ISender sender) : BaseController<GetByIdRoleQuery, SearchRoleQuery , GetListRoleQuery, CreateRoleCommand, UpdateRoleCommand, DeleteRoleCommand, DeleteListRoleCommand, RoleDto>(sender)
    {

    }
}
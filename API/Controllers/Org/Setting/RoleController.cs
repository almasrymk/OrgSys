using Application.Commands.Org.Setting.Preference.Queries;
using Application.Commands.Org.Setting.Role.Commands;
using Application.Commands.Org.Setting.Role.Queries;
using Application.Interfaces.CQRS;
using Azure;
using Domain.Shared;
using Entity.ModelView;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Setting
{
    [ApiController]
    [Route("[controller]")]
    public class RoleController(ISender sender) : BaseController<GetByIdRoleQuery, SearchRoleQuery , GetListRoleQuery, CreateRoleCommand, UpdateRoleCommand, DeleteRoleCommand, DeleteListRoleCommand, RoleModelView>(sender)
    {

    }
}
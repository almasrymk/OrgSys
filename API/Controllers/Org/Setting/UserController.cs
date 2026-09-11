using Application.Commands.Org.Setting.Preference.Queries;
using Administration.Application.Users.Commands;
using Administration.Application.Users.Queries;
using OrgSys.SharedKernel;
using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Setting
{
    [ApiController]
    [Route("[controller]")]
    public class UserController(ISender sender) : BaseController<GetByIdUserQuery, SearchUserQuery , GetListUserQuery, CreateUserCommand, UpdateUserCommand, DeleteUserCommand, DeleteListUserCommand, UserDto>(sender)
    {

    }
}
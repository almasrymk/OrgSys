using Application.Commands.Org.Setting.Preference.Queries;
using Application.Commands.Org.Setting.User.Commands;
using Application.Commands.Org.Setting.User.Queries;
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
    public class UserController(ISender sender) : BaseController<GetByIdUserQuery, SearchUserQuery , GetListUserQuery, CreateUserCommand, UpdateUserCommand, DeleteUserCommand, DeleteListUserCommand, UserModelView>(sender)
    {

    }
}
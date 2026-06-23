using Application.Commands.Org.Accounts.Account.Queries;
using Application.Commands.Org.Accounts.Safe.Queries;
using Application.Commands.Org.Setting.Preference.Queries;
using Application.Commands.Org.Setting.Safe.Commands;
using Application.Commands.Org.Setting.Safe.Queries;
using Application.Interfaces.CQRS;
using Domain.Shared;
using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Setting
{
    [ApiController]
    [Route("[controller]")]
    public class SafeController(ISender sender) : BaseController<GetByIdSafeQuery, SearchSafeQuery , GetListSafeQuery, CreateSafeCommand, UpdateSafeCommand, DeleteSafeCommand, DeleteListSafeCommand , GetMaxSafeQuery, SafeDto>(sender)
    {

    }
}
using Application.Commands.Org.Setting.Preference.Queries;
using Organization.Application.Tables.Commands;
using Organization.Application.Tables.Queries;
using OrgSys.SharedKernel;
using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Setting
{
    [ApiController]
    [Route("[controller]")]
    public class TableController(ISender sender) : BaseController<GetByIdTableQuery, SearchTableQuery , GetListTableQuery, CreateTableCommand, UpdateTableCommand, DeleteTableCommand, DeleteListTableCommand, TableDto>(sender)
    {

    }
}
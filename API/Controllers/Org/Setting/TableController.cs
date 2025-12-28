using Application.Commands.Org.Setting.Preference.Queries;
using Application.Commands.Org.Setting.Table.Commands;
using Application.Commands.Org.Setting.Table.Queries;
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
    public class TableController(ISender sender) : BaseController<GetByIdTableQuery, SearchTableQuery , GetListTableQuery, CreateTableCommand, UpdateTableCommand, DeleteTableCommand, DeleteListTableCommand, TableModelView>(sender)
    {

    }
}
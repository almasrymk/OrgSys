namespace OrgSys.Areas.Setting.Controllers
{
    using Organization.Application.Tables.Commands;
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using OrgSys.Controllers;

    [Area("Setting")]
    public class TableController(IConfiguration configuration, IMapper mapper) : MainController<TableDto, CreateTableCommand, UpdateTableCommand>(configuration, mapper)
    {

    }
}
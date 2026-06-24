namespace OrgSys.Areas.Setting.Controllers
{
    using Application.Commands.Org.Setting.Table.Commands;
    using AutoMapper;
    using Application.DTOs;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using OrgSys.Controllers;

    [Area("Setting")]
    public class TableController(IConfiguration configuration, IMapper mapper) : MainController<TableDto, CreateTableCommand, UpdateTableCommand>(configuration, mapper)
    {

    }
}